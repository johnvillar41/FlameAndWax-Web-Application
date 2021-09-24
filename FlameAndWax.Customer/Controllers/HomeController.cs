using FlameAndWax.Customer.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Customer.Controllers
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
            var productServiceResult = await _homeService.FetchNewArrivedProductsAndTopCustomerReviews(ConnectionString);
            if (productServiceResult.HasError) return BadRequest(new { errorContent = productServiceResult.ErrorContent });

            var newProducts = new List<ProductViewModel>();
            foreach (var newProduct in productServiceResult.Result.Item1)
            {
                newProducts.Add(new ProductViewModel(newProduct));
            }

            var topCustomerReviews = new List<CustomerReviewViewModel>();
            foreach (var topReview in productServiceResult.Result.Item2)
            {
                topCustomerReviews.Add(new CustomerReviewViewModel(topReview));
            }

            var homeViewModel = new HomeViewModel
            {
                NewProducts = newProducts,
                TopCustomerReviews = topCustomerReviews
            };

            return View(homeViewModel);
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
