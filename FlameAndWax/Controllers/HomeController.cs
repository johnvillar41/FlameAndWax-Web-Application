using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Services;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerService _customerService;
        public HomeController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var productResult = await _customerService.FetchNewArrivedProducts();
            if (productResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = productResult.ErrorContent
                };
                return View("Error", error);
            }
                

            var newProducts = new List<ProductViewModel>();
            foreach (var newProduct in productResult.Result)
            {
                var reviewResult = await _customerService.FetchCustomerReviewsInAProduct(newProduct.ProductId);
                if (reviewResult.HasError)
                {
                    var error = new ErrorViewModel
                    {
                        ErrorContent = productResult.ErrorContent
                    };
                    return View("Error", error);
                }
                newProducts.Add(
                        new ProductViewModel
                        {
                            ProductId = newProduct.ProductId,
                            ProductName = newProduct.ProductName,
                            ProductDescription = newProduct.ProductDescription,
                            ProductPrice = newProduct.ProductPrice,
                            PhotoLink = newProduct.PhotoLink
                        }
                    );
            }

            return View(newProducts);
        }
    }
}
