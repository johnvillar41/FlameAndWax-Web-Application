using FlameAndWax.Data.Constants;
using FlameAndWax.Models;
using FlameAndWax.Services.Helpers;
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

        public async Task<IActionResult> Index(List<ProductViewModel> products)
        {
            if (products.Count() == 0)
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

                var productsViewModel = new List<ProductViewModel>();
                foreach (var product in productResult.Result)
                {
                    productsViewModel.Add(new ProductViewModel
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ProductDescription = product.ProductDescription,
                        ProductPrice = product.ProductPrice,
                        PhotoLink = product.ProductGallery.FirstOrDefault().PhotoLink
                    });
                }
                return View(productsViewModel);
            }

            return View(products);
        }

        public async Task<IActionResult> AddToCart(int productId = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Sort(string category)
        {
            var categorizedProducts = await _customerService.FetchProductByCategory(ServiceHelper.ConvertStringToConstant(category));
            if (categorizedProducts.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = categorizedProducts.ErrorContent
                };
                return View("Error", error);
            }

            var products = new List<ProductViewModel>();
            foreach (var product in categorizedProducts.Result)
            {
                products.Add(new ProductViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductPrice = product.ProductPrice,
                    PhotoLink = product.ProductGallery.FirstOrDefault().PhotoLink
                });

            }

            return PartialView("ProductsPartial", products);
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

            var customerReviewViewModels = new List<CustomerReviewViewModel>();
            foreach (var customerReview in customerReviewResult.Result)
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

            return View(new ProductDetailViewModel
            {
                ProductId = productId,
                ProductName = productResult.Result.ProductName,
                ProductDescription = productResult.Result.ProductDescription,
                ProductPrice = productResult.Result.ProductPrice,
                UnitPrice = productResult.Result.UnitPrice,
                UnitsInStock = productResult.Result.UnitsInStock,
                CustomerReviews = customerReviewViewModels,
                ProductGallery = productResult.Result.ProductGallery
            });
        }
    }
}
