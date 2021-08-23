using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public ProductsController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index(List<ProductViewModel> products)
        {
            if (products.Count() == 0)
            {
                var productServiceResult = await _customerService.FetchAllProducts();
                if (productServiceResult.HasError)
                {
                    var error = new ErrorViewModel
                    {
                        ErrorContent = productServiceResult.ErrorContent
                    };
                    return View("Error", error);
                }

                var productsViewModel = new List<ProductViewModel>();
                foreach (var product in productServiceResult.Result)
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
                return View(productsViewModel);
            }

            return View(products);
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
                ReviewScore = (Constants.ReviewScore)(int)ServiceHelper.BuildReviewScore(rate), //Default value for now to be fixed later,
                ReviewDetail = reviewDetail,
                Customer = new CustomerModel
                {
                    CustomerId = int.Parse(customerIdLoggedIn)
                },
                Product = new ProductModel
                {
                    ProductId = productId
                }
            };
            var reviewServiceResult = await _customerService.AddCustomerReview(customerReview);
            var customerServiceReviewResult = await _customerService.FetchCustomerReviewsInAProduct(productId);
            if (customerServiceReviewResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = customerServiceReviewResult.ErrorContent
                };
                return View("Error", error);
            }

            var customerReviewModels = new List<CustomerReviewViewModel>();
            foreach (var customerReviewResult in customerServiceReviewResult.Result)
            {
                customerReviewModels.Add(
                        new CustomerReviewViewModel
                        {
                            ReviewId = customerReviewResult.ReviewId,
                            ProductId = customerReviewResult.Product.ProductId,
                            ReviewDetail = customerReviewResult.ReviewDetail,
                            ReviewScore = ServiceHelper.BuildReviewScore(rate),//TODO FIX THIS STATIC VALUE
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
            customerReviewModels.Reverse();
            return PartialView("ProductReviewPartial", customerReviewModels);
        }

        public async Task<IActionResult> Sort(string category)
        {
            var categorizedProductsServiceResult = await _customerService.FetchProductByCategory(ServiceHelper.ConvertStringToConstant(category));
            if (categorizedProductsServiceResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = categorizedProductsServiceResult.ErrorContent
                };
                return View("Error", error);
            }

            var products = new List<ProductViewModel>();
            foreach (var product in categorizedProductsServiceResult.Result)
            {
                products.Add(new ProductViewModel
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

            return PartialView("ProductsPartial", products);
        }

        public async Task<IActionResult> Details(int productId)
        {
            var productServiceResult = await _customerService.FetchProductDetail(productId);
            if (productServiceResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = productServiceResult.ErrorContent
                };
                return View("Error", error);
            }

            var customerReviewServiceResult = await _customerService.FetchCustomerReviewsInAProduct(productId);
            if (customerReviewServiceResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = customerReviewServiceResult.ErrorContent
                };
                return View("Error", error);
            }

            var customerReviewViewModels = new List<CustomerReviewViewModel>();
            foreach (var customerReview in customerReviewServiceResult.Result)
            {
                customerReviewViewModels.Add(
                        new CustomerReviewViewModel
                        {
                            ReviewId = customerReview.ReviewId,
                            ProductId = customerReview.Product.ProductId,
                            ReviewDetail = customerReview.ReviewDetail,
                            ReviewScore = customerReview.ReviewScore,
                            Customer = new CustomerViewModel
                            {
                                CustomerId = customerReview.Customer.CustomerId,
                                CustomerName = customerReview.Customer.CustomerName,
                                ContactNumber = customerReview.Customer.ContactNumber,
                                ProfilePictureLink = customerReview.Customer.ProfilePictureLink
                            }
                        }
                    );
            }
            customerReviewViewModels.Reverse();
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
                var hasCustomerOrderedServiceResult = _customerService.CheckIfCustomerHasOrderedAProduct(loggedInUser, productId).Result;
                productDetailViewModel.IsProductBoughtByLoggedInCustomer = hasCustomerOrderedServiceResult.Result;
                return View(productDetailViewModel);
            }

            return View(productDetailViewModel);
        }
    }
}
