using System.Collections.Generic;

namespace FlameAndWax.Customer.Models
{
    public class HomeViewModel
    {
        public IEnumerable<ProductViewModel> NewProducts { get; set; }
        public IEnumerable<CustomerReviewViewModel> TopCustomerReviews { get; set; }
    }
}