using FlameAndWax.Data.Models;
using System.Collections;
using System.Collections.Generic;

namespace FlameAndWax.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public double UnitPrice { get; set; }
        public int UnitsInStock { get; set; }

        public IEnumerable<CustomerReviewModel> CustomerReviews { get; set; }
    }
}
