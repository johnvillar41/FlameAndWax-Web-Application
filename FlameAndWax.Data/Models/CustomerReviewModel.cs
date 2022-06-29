using System;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Data.Models
{
    public class CustomerReviewModel
    {
        public int ReviewId { get; set; }
        public ProductModel Product { get; set; }
        public CustomerModel Customer { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int ReviewScore { get; set; }
        public string ReviewDetail { get; set; }
        public DateTime Date { get; set; }        
    }
}
