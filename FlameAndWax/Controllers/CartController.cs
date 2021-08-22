using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            Thread.Sleep(3000);
            var userLoggedIn = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
            Cart.RemoveCartItem(productId, userLoggedIn);
            return PartialView("CartTablePartial", Cart.GetCartItems(userLoggedIn));
        }

        public IActionResult RefreshCart()
        {
            Thread.Sleep(3000);
            var userLoggedIn = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
            return PartialView("CartTablePartial", Cart.GetCartItems(userLoggedIn));
        }

        public async Task<IActionResult> Checkout()
        {
            var userLoggedInID = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.NameIdentifier).Value;
            var userLoggedInUsername = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
            var orderDetails = new List<OrderDetailModel>();
            foreach (var cartItem in Cart.GetCartItems(userLoggedInUsername))
            {
                orderDetails.Add(
                        new OrderDetailModel
                        {
                            Product = new ProductModel
                            {
                                ProductId = cartItem.ProductId,
                                ProductPrice = cartItem.ProductPrice,
                            },
                            Quantity = cartItem.QuantityOrdered
                        }
                    );
            }

            
            var orderDetailForeignKey = -1;
            foreach (var orderDetail in orderDetails)
            {
                var orderDetailServiceResult = await _customerService.InsertOrderDetail(orderDetail);
                orderDetailForeignKey = orderDetailServiceResult.Result;
                if (orderDetailServiceResult.HasError)
                {
                    return View("Error", new ErrorViewModel { ErrorContent = orderDetailServiceResult.ErrorContent });
                }
            }
            var orderModel = new OrderModel
            {
                Customer = new CustomerModel { CustomerId = int.Parse(userLoggedInID) },
                Employee = new EmployeeModel { EmployeeId = -1 },
                ModeOfPayment = Constants.ModeOfPayment.Cash, //static values for now fix later
                Courier = Constants.Courier.FoodPanda,
                OrderDetails = orderDetails,
                OrderDetailPk = orderDetailForeignKey
            };
            if (orderDetailForeignKey != -1)
            {
                var orderTransactionServiceResult = await _customerService.AddOrderTransaction(orderModel);
                if (orderTransactionServiceResult.HasError)
                {
                    return View("Error", new ErrorViewModel { ErrorContent = orderTransactionServiceResult.ErrorContent });
                }
            }

            return PartialView("CartTablePartial", Cart.GetCartItems(userLoggedInID));
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
