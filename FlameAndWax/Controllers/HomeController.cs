using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Services;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
                            PhotoLink = newProduct.ProductGallery.FirstOrDefault().PhotoLink
                        }
                    );
            }

            return View(newProducts);
        }

        public async Task<IActionResult> ViewCategorizedProducts(string category)
        {
            var categorizedProducts = await _customerService.FetchProductByCategory(ServiceHelper.ConvertStringToConstant(category));
            if (categorizedProducts.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = categorizedProducts.ErrorContent
                };
                return View("Error", error);
            }

            List<ProductViewModel> productViewModels = new List<ProductViewModel>();
            foreach(var product in categorizedProducts.Result)
            {
                productViewModels.Add(
                        new ProductViewModel
                        {
                            ProductId = product.ProductId,
                            ProductName = product.ProductName,
                            ProductDescription = product.ProductDescription,
                            ProductPrice = product.ProductPrice,
                            PhotoLink = product.ProductGallery.FirstOrDefault().PhotoLink
                        }
                    );
            }
            
            return View("../Products/Index", productViewModels);
        }

        public IActionResult SeeMore(int id)
        {
            return RedirectToAction("Details", "Products", new { productId = id });
        }
    }
}
