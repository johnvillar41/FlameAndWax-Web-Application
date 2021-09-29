using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;

namespace FlameAndWax.Employee.Models
{
    public class CustomerReviewViewModel
    {
        public int CustomerId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string ProfilePictureLink { get; set; }
        public string ProductReview { get; set; }
        public CustomerReviewViewModel(CustomerReviewModel customerReviewModel)
        {
            CustomerId = customerReviewModel.Customer.CustomerId;
            Fullname = customerReviewModel.Customer.CustomerName;
            Username = customerReviewModel.Customer.Username;
            ProfilePictureLink = customerReviewModel.Customer.ProfilePictureLink;
            ProductReview = customerReviewModel.ReviewDetail;
        }
        public CustomerReviewViewModel()
        {
            
        }
    }
}