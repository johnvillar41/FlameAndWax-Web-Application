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
        public double ProductPrice { get; set; }
        public int QuantityPerUnit { get; set; }
        public int Stocks { get; set; }
        public int UnitsInOrder { get; set; }
        public IEnumerable<CustomerReviewViewModel> CustomerReviews { get; set; }
        public ProductViewModel(ProductModel product, IEnumerable<CustomerReviewModel> customerReviews)
        {
            ProductId = product.ProductId;
            ProductName = product.ProductName;
            ProductDescription = product.ProductDescription;
            PictureLink = product.ProductGallery.FirstOrDefault().ProductPhotoLink;
            ProductPrice = product.ProductPrice;
            QuantityPerUnit = product.QuantityPerUnit;
            Stocks = product.UnitsInStock;
            UnitsInOrder = product.UnitsInOrder;
            CustomerReviews = customerReviews.Select(customerReviewModel => new CustomerReviewViewModel(customerReviewModel)).ToList();
        }
        public ProductViewModel(ProductModel productModel)
        {
            ProductId = productModel.ProductId;
            ProductName = productModel.ProductName;
            ProductDescription = productModel.ProductDescription;
            PictureLink = productModel.ProductGallery.FirstOrDefault().ProductPhotoLink;
            ProductPrice = productModel.ProductPrice;
            QuantityPerUnit = productModel.QuantityPerUnit;
            Stocks = productModel.UnitsInStock;
            UnitsInOrder = productModel.UnitsInOrder;
        }
        public ProductViewModel()
        {

        }
    }
}