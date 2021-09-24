using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Employee.Models
{
    public class CustomerReviewViewModel
    {
        public int CustomerId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string ProfilePictureLink { get; set; }
        public string ProductReview { get; set; }
    }
}