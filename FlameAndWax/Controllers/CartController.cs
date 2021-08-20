using FlameAndWax.Data.Constants;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class CartController : Controller
    {
        private readonly ICustomerService _customerService;
        public CartController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize(Roles = nameof(Constants.Roles.Customer))]
        public async Task<IActionResult> Index(int productId = 0, string user = "")
        {
            if(productId == 0)
            {
                return View(Cart.GetCartItems(user));
            }

            var productResult = await _customerService.FetchProductDetail(productId);
            if (productResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = productResult.ErrorContent
                };
                return View("Error", error);
            }

            var productViewModel = new ProductViewModel
            {
                ProductId = productId,
                ProductName = productResult.Result.ProductName,
                ProductDescription = productResult.Result.ProductDescription,
                ProductPrice = productResult.Result.ProductPrice,
                PhotoLink = productResult.Result.ProductGallery.FirstOrDefault().PhotoLink,
                StockQuantity = productResult.Result.QuantityPerUnit * productResult.Result.UnitsInStock
            };

            Cart.AddCartItem(productViewModel, user);
            return View(Cart.GetCartItems(user));
        }
    }
}
