using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    [Authorize(Roles = nameof(Constants.Roles.Customer))]
    public class CartController : Controller
    {
        private readonly ICustomerService _customerService;
        public CartController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public IActionResult Index(List<ProductViewModel> cartItems)
        {
            if (cartItems.Count() == 0)
            {
                var userLoggedIn = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
                return View(Cart.GetCartItems(userLoggedIn));
            }

            return View(cartItems);
        }

        public IActionResult DeleteCartItem(int productId)
        {            
            var userLoggedIn = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
            Cart.RemoveCartItem(productId, userLoggedIn);            
            return PartialView("CartTablePartial", new CartViewModel { CartProducts = Cart.GetCartItems(userLoggedIn) });
        }

        public IActionResult RefreshCart()
        {            
            var userLoggedIn = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
            return PartialView("CartTablePartial", new CartViewModel { CartProducts = Cart.GetCartItems(userLoggedIn) });
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartViewModel cart)
        {
            if (cart.CartProducts != null)
            {
                var userLoggedInID = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.NameIdentifier).Value;
                var userLoggedInUsername = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
               
                var cartItems = cart.CartProducts;
                var totalOrderCost = 0.0;
                var orderDetails = new List<OrderDetailModel>();
                
                foreach (var cartItem in cartItems)
                {                    
                    var subTotalCost = Cart.CalculateTotalCartCost(userLoggedInUsername, cartItem.QuantityOrdered);
                    totalOrderCost += subTotalCost;
                    var productPriceServiceResult = await _customerService.FetchProductPrice(cartItem.ProductId);
                    if (productPriceServiceResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = productPriceServiceResult.ErrorContent });
                    orderDetails.Add(
                        new OrderDetailModel
                        {
                            Product = new ProductModel
                            {
                                ProductId = cartItem.ProductId,
                                ProductPrice = productPriceServiceResult.Result //Fetch ProductPrice from service layer not from ui for safety                     
                            },
                            TotalPrice = subTotalCost,
                            Quantity = cartItems.Count(),
                        }
                    );

                    //should be refratored
                    var previouslyOrderedModel = new PreviouslyOrderedProductModel
                    {
                        ProductId = cartItem.ProductId,
                        CustomerUsername = userLoggedInUsername
                    };

                    var previousOrderedServiceResult = await _customerService.InsertPreviouslyOrderedProduct(previouslyOrderedModel);
                    if (previousOrderedServiceResult.HasError)
                        return View("Error", new ErrorViewModel { ErrorContent = previousOrderedServiceResult.ErrorContent });
                }

                var modeOfPayment = ServiceHelper.BuildModeOfPayment(cart.ModeOfPayment.ToString());
                var courierType = ServiceHelper.BuildCourier(cart.Courier.ToString());
                var order = new OrderModel
                {
                    Customer = new CustomerModel { CustomerId = int.Parse(userLoggedInID) },
                    Employee = new EmployeeModel { EmployeeId = -1 },
                    DateOrdered = DateTime.UtcNow,
                    TotalCost = totalOrderCost,
                    ModeOfPayment = modeOfPayment,
                    OrderDetails = orderDetails,
                    Courier = courierType
                };
                var primaryKeyServiceResult = await _customerService.InsertNewOrder(order);

                if (primaryKeyServiceResult.Result == -1)
                {
                    return View("Error", new ErrorViewModel { ErrorContent = primaryKeyServiceResult.ErrorContent });
                }              
                
                //Update ProductsTablee column units on order

                Cart.ClearCartItems(userLoggedInUsername);
                return PartialView("CartTablePartial", new CartViewModel { CartProducts = Cart.GetCartItems(userLoggedInID) });
            }

            return PartialView("Error", new ErrorViewModel { ErrorContent = "Empty Cart" });
        }

        public async Task<IActionResult> AddToCart(int productId = 0, string user = "")
        {
            if (productId == 0)
            {
                return View(Cart.GetCartItems(user));
            }

            var productServiceResult = await _customerService.FetchProductDetail(productId);
            if (productServiceResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = productServiceResult.ErrorContent
                };
                return View("Error", error);
            }

            var productViewModel = new ProductViewModel
            {
                ProductId = productId,
                ProductName = productServiceResult.Result.ProductName,
                ProductDescription = productServiceResult.Result.ProductDescription,
                ProductPrice = productServiceResult.Result.ProductPrice,
                PhotoLink = productServiceResult.Result.ProductGallery.FirstOrDefault().PhotoLink,
                StockQuantity = productServiceResult.Result.QuantityPerUnit * productServiceResult.Result.UnitsInStock,
                QuantityPerUnit = productServiceResult.Result.QuantityPerUnit
            };

            Cart.AddCartItem(productViewModel, user);
            return View(nameof(Index), Cart.GetCartItems(user));
        }
    }
}
