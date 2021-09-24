using FlameAndWax.Data.Constants;
using FlameAndWax.Employee.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Employee.Controllers
{
    [Authorize(Roles = nameof(Constants.Roles.Employee))]
    public class CustomerReviewController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; }

        public CustomerReviewController(IProductsService productsService, IConfiguration configuration)
        {
            _productsService = productsService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 9)
        {
            var productServiceResult = await _productsService.FetchAllProducts(pageNumber, pageSize, ConnectionString);
            if (productServiceResult.HasError)
                return BadRequest(productServiceResult.ErrorContent);

            var productViewModels = new List<ProductViewModel>();
            foreach (var item in productServiceResult.Result)
            {
                var commentServiceResult = await _productsService.FetchCustomerReviewsInAProduct(pageNumber, pageSize, item.ProductId, ConnectionString);

                productViewModels.Add(
                    new ProductViewModel(item, commentServiceResult.Result)
                );
            }
            
            return View(productViewModels);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
