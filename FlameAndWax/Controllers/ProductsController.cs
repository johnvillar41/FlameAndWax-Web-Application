using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Models;
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

namespace FlameAndWax.Controllers
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
            var totalNumberOfProductsServiceResult = await _productService.FetchTotalNumberOfProductsByCategory(null, ConnectionString);
            if (totalNumberOfProductsServiceResult.HasError) return BadRequest(new { errorContent = totalNumberOfProductsServiceResult.ErrorContent });

            //Determine total count of Products for pagination
            var totalNumberOfPages = Math.Ceiling((decimal)totalNumberOfProductsServiceResult.Result / pageSize);
            ViewData["ProductCount"] = (int)totalNumberOfPages;
            ViewData["ProductCategory"] = productCategory;

            var productsViewModel = new List<ProductViewModel>();

            var productServiceResult = await _productService.FetchAllProducts(pageNumber, pageSize, ConnectionString);
            if (productServiceResult.HasError) return BadRequest(new { errorContent = productServiceResult.ErrorContent });
            BuildProductViewModels(productsViewModel, productServiceResult.Result);
            return View(productsViewModel);
        }

        public async Task<IActionResult> PageProducts(int pageNumber = 1, int pageSize = 9, string category = Constants.ALL_PRODUCTS)
        {
            //Determine total number of products for pagination numbers
            ServiceResult<int> totalProductCount;
            if (category.Equals(Constants.ALL_PRODUCTS))
                totalProductCount = await _productService.FetchTotalNumberOfProductsByCategory(null, ConnectionString);
            else
                totalProductCount = await _productService.FetchTotalNumberOfProductsByCategory(ServiceHelper.ConvertStringToConstant(category), ConnectionString);

            if (totalProductCount.HasError) return BadRequest(new { errorContent = totalProductCount.ErrorContent });

            var totalNumberOfPages = Math.Ceiling((decimal)totalProductCount.Result / pageSize);
            ViewData["ProductCount"] = (int)totalNumberOfPages;
            ViewData["ProductCategory"] = category;

            var productsViewModel = new List<ProductViewModel>();
            ServiceResult<IEnumerable<ProductModel>> productModels;
            if (category.Equals(Constants.ALL_PRODUCTS))
            {
                productModels = await _productService.FetchAllProducts(pageNumber, pageSize, ConnectionString);
                if (productModels.HasError) return BadRequest(new { errorContent = productModels.ErrorContent });
                BuildProductViewModels(productsViewModel, productModels.Result);
                return PartialView("ProductsPartial", productsViewModel);
            }

            productModels = await _productService.FetchProductByCategory(pageNumber, pageSize, ServiceHelper.ConvertStringToConstant(category), ConnectionString);
            if (productModels.HasError) return BadRequest(new { errorContent = productModels.ErrorContent });
            BuildProductViewModels(productsViewModel, productModels.Result);
            return PartialView("ProductsPartial", productsViewModel);
        }

        [Authorize(Roles = nameof(Constants.Roles.Customer))]
        public IActionResult AddToCart(int _productId)
        {
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
                ReviewScore = (Constants.ReviewScore)(int)ServiceHelper.BuildReviewScore(rate),
                ReviewDetail = reviewDetail,
                Customer = new CustomerModel { CustomerId = int.Parse(customerIdLoggedIn) },
                Product = new ProductModel { ProductId = productId }
            };
            var reviewServiceResult = await _productService.AddCustomerReview(customerReview, ConnectionString);
            if (reviewServiceResult.HasError) return BadRequest(new { errorContent = reviewServiceResult.ErrorContent });

            var customerServiceReviewResult = await _productService.FetchCustomerReviewsInAProduct(1, 5, productId, ConnectionString);
            if (customerServiceReviewResult.HasError) return BadRequest(new { errorContent = customerServiceReviewResult.ErrorContent });

            var totalNumberOfPagesObjectResult = await FetchTotalNumberOfPages(productId);
            if (totalNumberOfPagesObjectResult != null) return totalNumberOfPagesObjectResult;

            var customerReviewViewModels = new List<CustomerReviewViewModel>();
            foreach (var customerReviewResult in customerServiceReviewResult.Result) BuildReviewViewModels(customerReviewViewModels, customerReviewResult);

            return PartialView("ProductReviewPartial", customerReviewViewModels);
        }

        public async Task<IActionResult> PageCustomerReviews(int pageNumber = 1, int pageSize = 5, int productId = -1)
        {
            var customerServiceReviewResult = await _productService.FetchCustomerReviewsInAProduct(pageNumber, pageSize, productId, ConnectionString);
            if (customerServiceReviewResult.HasError) return BadRequest(new { errorContent = customerServiceReviewResult.ErrorContent });

            var totalNumberOfPagesObjectResult = await FetchTotalNumberOfPages(productId);
            if (totalNumberOfPagesObjectResult != null) return totalNumberOfPagesObjectResult;

            var customerReviewViewModels = new List<CustomerReviewViewModel>();
            foreach (var customerReviewResult in customerServiceReviewResult.Result) BuildReviewViewModels(customerReviewViewModels, customerReviewResult);

            return PartialView("ProductReviewPartial", customerReviewViewModels);
        }

        public async Task<IActionResult> Details(int productId)
        {
            var productServiceResult = await _productService.FetchProductDetail(productId, ConnectionString);
            if (productServiceResult.HasError) return BadRequest(new { errorContent = productServiceResult.ErrorContent });

            var customerReviewServiceResult = await _productService.FetchCustomerReviewsInAProduct(1, 5, productId, ConnectionString);
            if (customerReviewServiceResult.HasError) return BadRequest(new { errorContent = customerReviewServiceResult.ErrorContent });

            var totalNumberOfPagesObjectResult = await FetchTotalNumberOfPages(productId);
            if (totalNumberOfPagesObjectResult != null) return totalNumberOfPagesObjectResult;

            var customerReviewViewModels = new List<CustomerReviewViewModel>();
            foreach (var customerReview in customerReviewServiceResult.Result) BuildReviewViewModels(customerReviewViewModels, customerReview);

            var productDetailViewModel = new ProductDetailViewModel
            {
                ProductId = productId,
                ProductName = productServiceResult.Result.ProductName,
                ProductDescription = productServiceResult.Result.ProductDescription,
                ProductPrice = productServiceResult.Result.ProductPrice,
                UnitPrice = productServiceResult.Result.UnitPrice,
                UnitsInStock = productServiceResult.Result.UnitsInStock,
                CustomerReviews = customerReviewViewModels,
                ProductGallery = productServiceResult.Result.ProductGallery,

            };

            if (User.Identity.IsAuthenticated)
            {
                var loggedInUser = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
                var hasCustomerOrderedServiceResult = _productService.CheckIfCustomerHasOrderedAProduct(loggedInUser, productId, ConnectionString).Result;
                productDetailViewModel.IsProductBoughtByLoggedInCustomer = hasCustomerOrderedServiceResult.Result;
                return View(productDetailViewModel);
            }

            return View(productDetailViewModel);
        }

        private async Task<ObjectResult> FetchTotalNumberOfPages(int productId)
        {
            //Determine total number of pages for reviews
            var totalReviewCountServiceResult = await _productService.FetchTotalNumberOfReviews(productId, ConnectionString);
            if (totalReviewCountServiceResult.HasError) return BadRequest(new { errorContent = totalReviewCountServiceResult.ErrorContent });
            var totalNumberOfPages = Math.Ceiling((decimal)totalReviewCountServiceResult.Result / 5);
            ViewData["CustomerReviewCount"] = (int)totalNumberOfPages;
            ViewData["ProductId"] = productId;

            return null;
        }

        private void BuildProductViewModels(List<ProductViewModel> productsViewModel, IEnumerable<ProductModel> productServiceResult)
        {
            foreach (var product in productServiceResult)
            {
                productsViewModel.Add(new ProductViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductPrice = product.ProductPrice,
                    PhotoLink = product.ProductGallery.FirstOrDefault().PhotoLink,
                    StockQuantity = product.UnitsInStock * product.QuantityPerUnit,
                    QuantityPerUnit = product.QuantityPerUnit,
                    UnitsInStock = product.UnitsInStock
                });
            }
        }

        private void BuildReviewViewModels(List<CustomerReviewViewModel> customerReviewViewModels, CustomerReviewModel customerReviewResult)
        {
            customerReviewViewModels.Add(
                new CustomerReviewViewModel
                {
                    ReviewId = customerReviewResult.ReviewId,
                    ProductId = customerReviewResult.Product.ProductId,
                    ReviewDetail = customerReviewResult.ReviewDetail,
                    ReviewScore = customerReviewResult.ReviewScore,
                    Customer = new CustomerViewModel
                    {
                        CustomerId = customerReviewResult.Customer.CustomerId,
                        CustomerName = customerReviewResult.Customer.CustomerName,
                        ContactNumber = customerReviewResult.Customer.ContactNumber,
                        ProfilePictureLink = customerReviewResult.Customer.ProfilePictureLink
                    }
                }
            );
        }
    }
}
