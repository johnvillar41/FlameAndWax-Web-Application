using System;
using System.Linq;
using FlameAndWax.Data.Models;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Customer.Models
{
    public class CustomerReviewViewModel
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public string ProductPictureLink { get; set; }
        public string ReviewDetail { get; set; }
        public ReviewScore ReviewScore { get; set; }
        public CustomerViewModel Customer { get; set; }
        public DateTime Date { get; set; }
        public CustomerReviewViewModel(CustomerReviewModel customerReviewModel)
        {
            ReviewId = customerReviewModel.ReviewId;
            ProductId = customerReviewModel.Product.ProductId;
            ReviewDetail = customerReviewModel.ReviewDetail;
            ReviewScore = (ReviewScore)customerReviewModel.ReviewScore;
            Customer = new CustomerViewModel(customerReviewModel.Customer);
            ProductPictureLink = customerReviewModel.Product.ProductGallery.FirstOrDefault().ProductPhotoLink;
            Date = customerReviewModel.Date;
        }
        public CustomerReviewViewModel()
        {

        }
    }
}
