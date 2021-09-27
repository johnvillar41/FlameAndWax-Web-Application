using System.Collections.Generic;

namespace FlameAndWax.Customer.Models
{
    public class HomeViewModel
    {
        public IEnumerable<ProductViewModel> NewProducts { get; set; }
        public IEnumerable<CustomerReviewViewModel> TopCustomerReviews { get; set; }
        public HomeViewModel(IEnumerable<ProductViewModel> newProducts, IEnumerable<CustomerReviewViewModel> topReviews)
        {
            NewProducts = newProducts;
            TopCustomerReviews = topReviews;
        }
        public HomeViewModel()
        {
            
        }
    }
}