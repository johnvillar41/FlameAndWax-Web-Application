using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlameAndWax.Employee.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FlameAndWax.Employee.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; }
        public ProductController(IProductsService productsService, IConfiguration configuration)
        {
            _productsService = productsService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 9)
        {
            var productsServiceResult = await _productsService.FetchAllProducts(pageNumber, pageSize, ConnectionString);
            if (productsServiceResult.HasError) return BadRequest(productsServiceResult.ErrorContent);

            var productViewModels = productsServiceResult.Result.Select(productModel => new ProductViewModel(productModel)).ToList();
            return View(productViewModels);
        }
    }
}