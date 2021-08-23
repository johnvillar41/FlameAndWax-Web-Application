using System.Diagnostics;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Data.Models
{
    public class OrderDetailModel
    {
        public int OrderDetailsId { get; set; }
        public OrderModel Order { get; set; }
        public ProductModel Product { get; set; }       
        public double TotalPrice { get; set; }
        public int Quantity { get; set; }
        public OrderDetailStatus Status { get; set; }
    }
}
