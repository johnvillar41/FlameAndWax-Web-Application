using FlameAndWax.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Models
{
    public class ProductDetailViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public string PhotoLink { get; set; }
        public double UnitPrice { get; set; }
        public int UnitsInStock { get; set; }

        public IEnumerable<CustomerReviewModel> CustomerReviews { get; set; }
    }
}
