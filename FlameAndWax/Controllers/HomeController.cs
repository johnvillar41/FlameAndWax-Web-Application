using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Services;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configuration;

        private string ConnectionString { get; set; }

        public HomeController(ICustomerService customerService, IConfiguration configuration)
        {
            _customerService = customerService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }

        public async Task<IActionResult> Index()
        {
            var productServiceResult = await _customerService.FetchNewArrivedProducts(ConnectionString);
            if (productServiceResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = productServiceResult.ErrorContent
                };
                return View("Error", error);
            }


            var newProducts = new List<ProductViewModel>();
            foreach (var newProduct in productServiceResult.Result)
            {
                var reviewResult = await _customerService.FetchCustomerReviewsInAProduct(newProduct.ProductId, ConnectionString);
                if (reviewResult.HasError)
                {
                    var error = new ErrorViewModel
                    {
                        ErrorContent = productServiceResult.ErrorContent
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

        public IActionResult ViewCategorizedProducts(string category)
        {           
            return RedirectToAction("Index" , "Products" , new { pageNumber = 1, productCategory = category});
        }

        public IActionResult SeeMore(int id)
        {
            return RedirectToAction("Details", "Products", new { productId = id });
        }
    }
}
