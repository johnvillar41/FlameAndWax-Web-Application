using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly IConfiguration _configuration;

        private string ConnectionString { get; }

        public HomeController(IHomeService homeService, IConfiguration configuration)
        {
            _homeService = homeService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }

        public async Task<IActionResult> Index()
        {
            var productServiceResult = await _homeService.FetchNewArrivedProducts(ConnectionString);
            if (productServiceResult.HasError) return BadRequest(new { errorContent = productServiceResult.ErrorContent });

            var newProducts = new List<ProductViewModel>();
            foreach (var newProduct in productServiceResult.Result)
            {
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
            return RedirectToAction("Index", "Products", new { pageNumber = 1, productCategory = category });
        }

        public IActionResult SeeMore(int id)
        {
            return RedirectToAction("Details", "Products", new { productId = id });
        }
    }
}
