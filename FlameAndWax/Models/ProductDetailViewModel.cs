using FlameAndWax.Data.Models;
using System.Collections.Generic;

namespace FlameAndWax.Models
{
    public class ProductDetailViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public double UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public bool IsProductBoughtByLoggedInCustomer { get; set; }
        public string AddReviewDetail { get; set; }
        public IEnumerable<ProductGalleryModel> ProductGallery { get; set; }
        public IEnumerable<CustomerReviewViewModel> CustomerReviews { get; set; }
        public ProductDetailViewModel(ProductModel productModel, IEnumerable<CustomerReviewViewModel> customerReviewViewModels)
        {
            ProductId = productModel.ProductId;
            ProductName = productModel.ProductName;
            ProductDescription = productModel.ProductDescription;
            ProductPrice = productModel.ProductPrice;
            UnitPrice = productModel.UnitPrice;
            UnitsInStock = productModel.UnitsInStock;
            CustomerReviews = customerReviewViewModels;
            ProductGallery = productModel.ProductGallery;
        }
        public ProductDetailViewModel()
        {

        }
    }
}
