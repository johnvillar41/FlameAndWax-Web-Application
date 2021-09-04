using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Services.Interfaces;
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
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configaration;

        private string ConnectionString { get; set; }

        public ProductsController(ICustomerService customerService, IConfiguration configuration)
        {
            _customerService = customerService;
            _configaration = configuration;
            ConnectionString = _configaration.GetConnectionString("FlameAndWaxDBConnection");
        }

        public async Task<IActionResult> Index(List<ProductViewModel> products, string productCategory = "", int pageNumber = 1, int pageSize = 9)
        {
            var totalNumberOfProductsServiceResult = await _customerService.FetchTotalNumberOfProducts(ConnectionString);
            if (totalNumberOfProductsServiceResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = totalNumberOfProductsServiceResult.ErrorContent });

            var totalNumberOfPages = Math.Ceiling((decimal)totalNumberOfProductsServiceResult.Result / 9);
            ViewData["ProductCount"] = (int)totalNumberOfPages;

            var productsViewModel = new List<ProductViewModel>();
            if (products.Count() == 0 && productCategory.Length == 0)
            {
                var productServiceResult = await _customerService.FetchAllProducts(pageNumber, pageSize, ConnectionString);
                if (productServiceResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = productServiceResult.ErrorContent });
                BuildProductViewModels(productsViewModel, productServiceResult.Result);
                return View(productsViewModel);
            }

            var categorizedProductsServiceResult = await _customerService.FetchProductByCategory(ServiceHelper.ConvertStringToConstant(productCategory), ConnectionString);
            if (categorizedProductsServiceResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = categorizedProductsServiceResult.ErrorContent });

            BuildProductViewModels(productsViewModel, categorizedProductsServiceResult.Result);
            return View(productsViewModel);
        }

        public async Task<IActionResult> PageProducts(int pageNumber = 1, int pageSize = 9)
        {
            //Determine total number of products for pagination numbers
            var totalNumberOfProductsServiceResult = await _customerService.FetchTotalNumberOfProducts(ConnectionString);
            if (totalNumberOfProductsServiceResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = totalNumberOfProductsServiceResult.ErrorContent });

            var totalNumberOfPages = Math.Ceiling((decimal)totalNumberOfProductsServiceResult.Result / 9);
            ViewData["ProductCount"] = (int)totalNumberOfPages;

            var productServiceResult = await _customerService.FetchAllProducts(pageNumber, pageSize, ConnectionString);
            if (productServiceResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = productServiceResult.ErrorContent });

            var productsViewModel = new List<ProductViewModel>();
            BuildProductViewModels(productsViewModel, productServiceResult.Result);
            return PartialView("ProductsPartial", productsViewModel);
        }

        [Authorize(Roles = nameof(Constants.Roles.Customer))]
        public IActionResult AddToCart(int _productId)
        {
            var userLoggedIn = User.Claims.FirstOrDefault(user => user.Type == ClaimTypes.Name).Value;
            return RedirectToAction("AddToCart", "Cart", new { productId = _productId, user = userLoggedIn });
        }

        [HttpGet]
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
            var reviewServiceResult = await _customerService.AddCustomerReview(customerReview, ConnectionString);
            var customerServiceReviewResult = await _customerService.FetchCustomerReviewsInAProduct(productId, ConnectionString);
            if (customerServiceReviewResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = customerServiceReviewResult.ErrorContent });

            var customerReviewViewModels = new List<CustomerReviewViewModel>();
            foreach (var customerReviewResult in customerServiceReviewResult.Result) BuildReviewViewModels(customerReviewViewModels, customerReviewResult);

            return PartialView("ProductReviewPartial", customerReviewViewModels);
        }

        public async Task<IActionResult> Sort(string category)
        {
            var categorizedProductsServiceResult = await _customerService.FetchProductByCategory(ServiceHelper.ConvertStringToConstant(category), ConnectionString);
            if (categorizedProductsServiceResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = categorizedProductsServiceResult.ErrorContent });

            var productsViewModel = new List<ProductViewModel>();
            BuildProductViewModels(productsViewModel, categorizedProductsServiceResult.Result);
            return PartialView("ProductsPartial", productsViewModel);
        }

        public async Task<IActionResult> Details(int productId)
        {
            var productServiceResult = await _customerService.FetchProductDetail(productId, ConnectionString);
            if (productServiceResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = productServiceResult.ErrorContent });

            var customerReviewServiceResult = await _customerService.FetchCustomerReviewsInAProduct(productId, ConnectionString);
            if (customerReviewServiceResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = customerReviewServiceResult.ErrorContent });

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
                var hasCustomerOrderedServiceResult = _customerService.CheckIfCustomerHasOrderedAProduct(loggedInUser, productId, ConnectionString).Result;
                productDetailViewModel.IsProductBoughtByLoggedInCustomer = hasCustomerOrderedServiceResult.Result;
                return View(productDetailViewModel);
            }

            return View(productDetailViewModel);
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
