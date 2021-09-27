using System.Collections.Generic;
using System.Linq;
using FlameAndWax.Data.Models;

namespace FlameAndWax.Employee.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string PictureLink { get; set; }
        public IEnumerable<CustomerReviewViewModel> CustomerReviews { get; set; }
        public ProductViewModel(ProductModel product, IEnumerable<CustomerReviewModel> customerReviews)
        {
            ProductId = product.ProductId;
            ProductName = product.ProductName;
            ProductDescription = product.ProductDescription;
            PictureLink = product.ProductGallery.FirstOrDefault().PhotoLink;
            CustomerReviews = customerReviews.Select(x => new CustomerReviewViewModel
            {
                CustomerId = x.Customer.CustomerId,
                Fullname = x.Customer.CustomerName,
                ProfilePictureLink = x.Product.ProductGallery.FirstOrDefault().PhotoLink,
                ProductReview = x.ReviewDetail
            }).ToList();
        }
        public ProductViewModel(ProductModel productModel)
        {
            ProductId = productModel.ProductId;
            ProductName = productModel.ProductName;
            ProductDescription = productModel.ProductDescription;
            PictureLink = productModel.ProductGallery.FirstOrDefault().PhotoLink;
        }
        public ProductViewModel()
        {

        }
    }
}