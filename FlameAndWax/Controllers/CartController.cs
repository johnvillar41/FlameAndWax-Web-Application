using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    [Authorize(Roles = nameof(Constants.Roles.Customer))]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IConfiguration _configuration;

        private string ConnectionString { get; }

        public CartController(ICartService cartService, IConfiguration configuration)
        {
            _cartService = cartService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }

        public IActionResult Index()
        {
            var userLoggedIn = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
            return View(Cart.GetCartItems(userLoggedIn));
        }

        public IActionResult IncrementProductCount(int productId)
        {
            var userLoggedIn = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
            var result = Cart.IncrementProductCount(productId, userLoggedIn);

            if (!result) return BadRequest("Not Enough Products!");
            return PartialView("CartTablePartial", new CartViewModel { CartProducts = Cart.GetCartItems(userLoggedIn) });
        }

        public IActionResult DecrementProductCount(int productId)
        {
            var userLoggedIn = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
            Cart.DecrementProductCount(productId, userLoggedIn);
            return PartialView("CartTablePartial", new CartViewModel { CartProducts = Cart.GetCartItems(userLoggedIn) });
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

        public async Task<IActionResult> Checkout()
        {
            var cart = JsonConvert.DeserializeObject<CartViewModel>(TempData["CartViewModel"].ToString());
            if (cart.CartProducts == null) return RedirectToAction("Index", "Error", new { ErrorContent = "Cart Products is null" });

            var userLoggedInID = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.NameIdentifier).Value;
            var userLoggedInUsername = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;

            var cartItems = cart.CartProducts;            
            var orderDetails = new List<OrderDetailModel>();
            var result = await BuildOrderDetails(orderDetails, cartItems, userLoggedInUsername);
            var totalOrderCost = orderDetails.Select(x => x.TotalPrice).Sum();

            if (result != null) return result;

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
            var primaryKeyServiceResult = await _cartService.CheckoutOrder(order, userLoggedInUsername, ConnectionString);

            if (primaryKeyServiceResult.HasError) return BadRequest(new { errorContent = primaryKeyServiceResult.ErrorContent });

            Cart.ClearCartItems(userLoggedInUsername);
            return Ok("Successfully Inserted Order!");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CartSummary(CartViewModel cart)
        {
            var userLoggedInUsername = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
            var totalCost = Cart.GetTotalCartCost(userLoggedInUsername);
            CartSummaryViewModel cartSummary = new CartSummaryViewModel(totalCost, cart);

            return View(cartSummary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FinalizeCheckout(CartViewModel cart)
        {
            var userLoggedInUsername = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
            var cartViewModel = new CartViewModel(
                cart.ModeOfPayment,
                cart.Courier,
                Cart.GetCartItems(userLoggedInUsername));
            TempData["CartViewModel"] = JsonConvert.SerializeObject(cartViewModel);
            return RedirectToAction(nameof(Checkout), nameof(Cart));
        }

        public async Task<IActionResult> AddToCart(int productId = 0, string user = "")
        {
            if (productId == 0) return View(Cart.GetCartItems(user));

            var productServiceResult = await _cartService.FetchProductDetail(productId, ConnectionString);
            if (productServiceResult.HasError) return RedirectToAction("Index", "Error", new { productServiceResult.ErrorContent });

            var canStillOrderProduct = true;
            if (productServiceResult.Result.UnitsInStock == 0) canStillOrderProduct = false;
            var productsLeft = productServiceResult.Result.UnitsInStock - productServiceResult.Result.UnitsInOrder;
            if (productsLeft == 0) canStillOrderProduct = false;

            if (!canStillOrderProduct) return BadRequest("Cannot Order! No more products left!");

            var productViewModel = new ProductViewModel(productServiceResult.Result);

            Cart.AddCartItem(productViewModel, user);
            var cartItems = Cart.GetCartItems(user);
            var cartItemCount = cartItems.Count();
            return Ok(cartItemCount);
        }
        private async Task<ObjectResult> BuildOrderDetails(
            List<OrderDetailModel> orderDetails,
            List<ProductViewModel> cartItems,            
            string userLoggedInUsername)
        {
            foreach (var cartItem in cartItems)
            {
                var subTotalCost = Cart.CalculateTotalCartCost(userLoggedInUsername, cartItem.QuantityOrdered);                
                var productPriceServiceResult = await _cartService.FetchProductPrice(cartItem.ProductId, ConnectionString);
                if (productPriceServiceResult.HasError) return BadRequest(productPriceServiceResult.ErrorContent);
                orderDetails.Add(
                    new OrderDetailModel
                    {
                        Product = new ProductModel
                        {
                            ProductId = cartItem.ProductId,
                            ProductPrice = productPriceServiceResult.Result
                        },
                        TotalPrice = subTotalCost,
                        Quantity = cartItem.QuantityOrdered,
                    }
                );
            }
            return null;
        }
    }
}
