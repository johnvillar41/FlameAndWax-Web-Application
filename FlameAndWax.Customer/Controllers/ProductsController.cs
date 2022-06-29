using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Customer.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Services.Interfaces;
using FlameAndWax.Services.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;

namespace FlameAndWax.Customer.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _productService;
        private readonly IConfiguration _configaration;

        private string ConnectionString { get; }

        public ProductsController(IProductsService customerService, IConfiguration configuration)
        {
            _productService = customerService;
            _configaration = configuration;
            ConnectionString = _configaration.GetConnectionString("FlameAndWaxDBConnection");
        }

        public IActionResult Sort(string category)
        {
            return RedirectToAction("PageProducts", new { category });
        }

        public async Task<IActionResult> Index(string productCategory = Constants.ALL_PRODUCTS, int pageNumber = 1, int pageSize = 9)
        {
            var productServiceResult = await _productService.FetchAllProductsAsync(pageNumber, pageSize, ConnectionString);
            if (productServiceResult.HasError) return BadRequest(new { errorContent = productServiceResult.ErrorContent });

            //Determine total count of Products for pagination
            var totalNumberOfPages = Math.Ceiling((decimal)productServiceResult.TotalProductCount / pageSize);
            ViewData["ProductCount"] = (int)totalNumberOfPages;
            ViewData["ProductCategory"] = productCategory;

            var productViewModels = productServiceResult.Result.Select(ProductModel => new ProductViewModel(ProductModel)).ToList();
            return View(productViewModels);
        }

        public async Task<IActionResult> PageProducts(int pageNumber = 1, int pageSize = 9, string category = Constants.ALL_PRODUCTS)
        {            
            PagedServiceResult<IEnumerable<ProductModel>> productModels;
            if (category.Equals(Constants.ALL_PRODUCTS))
            {
                productModels = await _productService.FetchAllProductsAsync(pageNumber, pageSize, ConnectionString);
                if (productModels.HasError) return BadRequest(new { errorContent = productModels.ErrorContent });               
            }
            else
            {
                productModels = await _productService.FetchProductByCategoryAsync(pageNumber, pageSize, ServiceHelper.ConvertStringToConstant(category), ConnectionString);
                if (productModels.HasError) return BadRequest(new { errorContent = productModels.ErrorContent });
            }
            var productViewModels = productModels.Result.Select(productModel => new ProductViewModel(productModel)).ToList();
            var totalNumberOfPages = Math.Ceiling((decimal)productModels.TotalProductCount / pageSize);
            ViewData["ProductCount"] = (int)totalNumberOfPages;
            ViewData["ProductCategory"] = category;

            return PartialView("ProductsPartial", productViewModels);
        }


        public IActionResult AddToCart(int _productId)
        {            
            if (!User.Identity.IsAuthenticated) return Unauthorized($"/Account/Login/?returnUrl=/Products/Details?productId={_productId}");

            var userLoggedIn = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
            return RedirectToAction("AddToCart", "Cart", new { productId = _productId, user = userLoggedIn });
        }

        [HttpGet]
        [Authorize(Roles = nameof(Constants.Roles.Customer))]
        public async Task<IActionResult> AddProductReview(string reviewDetail, int productId, int rate)
        {
            var customerIdLoggedIn = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var customerReview = new CustomerReviewModel
            {
                ReviewScore = (int)ServiceHelper.BuildReviewScore(rate),
                ReviewDetail = reviewDetail,
                Customer = new CustomerModel { CustomerId = int.Parse(customerIdLoggedIn) },
                Product = new ProductModel { ProductId = productId }
            };
            var reviewServiceResult = await _productService.AddCustomerReviewAsync(customerReview, ConnectionString);
            if (reviewServiceResult.HasError) return BadRequest(new { errorContent = reviewServiceResult.ErrorContent });

            var customerServiceReviewResult = await _productService.FetchCustomerReviewsInAProductAsync(1, 5, productId, ConnectionString);
            if (customerServiceReviewResult.HasError) return BadRequest(new { errorContent = customerServiceReviewResult.ErrorContent });

            var totalNumberOfPages = Math.Ceiling((decimal)customerServiceReviewResult.TotalProductCount / 5);
            ViewData["CustomerReviewCount"] = (int)totalNumberOfPages;
            ViewData["ProductId"] = productId;

            var customerReviewViewModels = customerServiceReviewResult.Result.Select(customerReview => new CustomerReviewViewModel(customerReview)).ToList();           
            return PartialView("ProductReviewPartial", customerReviewViewModels);
        }

        public async Task<IActionResult> PageCustomerReviews(int pageNumber = 1, int pageSize = 5, int productId = -1)
        {
            var customerServiceReviewResult = await _productService.FetchCustomerReviewsInAProductAsync(pageNumber, pageSize, productId, ConnectionString);
            if (customerServiceReviewResult.HasError) return BadRequest(new { errorContent = customerServiceReviewResult.ErrorContent });

            var totalNumberOfPages = Math.Ceiling((decimal)customerServiceReviewResult.TotalProductCount / pageSize);
            ViewData["CustomerReviewCount"] = (int)totalNumberOfPages;
            ViewData["ProductId"] = productId;

            var customerReviewViewModels = customerServiceReviewResult.Result.Select(customerReview => new CustomerReviewViewModel(customerReview)).ToList();
            return PartialView("ProductReviewPartial", customerReviewViewModels);
        }

        public async Task<IActionResult> Details(int productId)
        {
            var productServiceResult = await _productService.FetchProductDetailAsync(productId, ConnectionString);
            if (productServiceResult.HasError) return BadRequest(new { errorContent = productServiceResult.ErrorContent });

            var customerReviewServiceResult = await _productService.FetchCustomerReviewsInAProductAsync(1, 5, productId, ConnectionString);
            if (customerReviewServiceResult.HasError) return BadRequest(new { errorContent = customerReviewServiceResult.ErrorContent });

            var totalNumberOfPages = Math.Ceiling((decimal)customerReviewServiceResult.TotalProductCount / 5);
            ViewData["CustomerReviewCount"] = (int)totalNumberOfPages;
            ViewData["ProductId"] = productId;

            var customerReviewViewModels = customerReviewServiceResult.Result.Select(customerReview => new CustomerReviewViewModel(customerReview)).ToList();

            var productDetailViewModel = new ProductDetailViewModel(productServiceResult.Result, customerReviewViewModels);

            if (User.Identity.IsAuthenticated)
            {
                var loggedInUser = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
                var hasCustomerOrderedServiceResult = _productService.CheckIfCustomerHasOrderedAProductAsync(loggedInUser, productId, ConnectionString).Result;
                productDetailViewModel.IsProductBoughtByLoggedInCustomer = hasCustomerOrderedServiceResult.Result;
                return View(productDetailViewModel);
            }

            return View(productDetailViewModel);
        }        
    }
}
