using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ICustomerService _customerService;
        public ProductsController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var productResult = await _customerService.FetchAllProducts();
            if (productResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = productResult.ErrorContent
                };
                return View("Error", error);
            }

            var products = new List<ProductViewModel>();
            foreach (var product in productResult.Result)
            {               
                products.Add(new ProductViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductPrice = product.ProductPrice,                    
                    PhotoLink = product.PhotoLink                    
                });
            }
            return View(products);
        }

        public async Task<IActionResult> Details(int productId)
        {
            var productResult = await _customerService.FetchProductDetail(productId);
            if (productResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = productResult.ErrorContent
                };
                return View("Error", error);
            }

            var customerReviewResult = await _customerService.FetchCustomerReviewsInAProduct(productId);
            if (customerReviewResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = customerReviewResult.ErrorContent
                };
                return View("Error", error);
            }
            var productDetailModel = new ProductDetailViewModel
            {
                ProductId = productId,
                ProductName = productResult.Result.ProductName,
                ProductDescription = productResult.Result.ProductDescription,
                ProductPrice = productResult.Result.ProductPrice,
                PhotoLink = productResult.Result.PhotoLink,
                UnitPrice = productResult.Result.UnitPrice,
                UnitsInStock = productResult.Result.UnitsInStock,
                CustomerReviews = customerReviewResult.Result
            };
            return View(productDetailModel);
        }
    }
}
