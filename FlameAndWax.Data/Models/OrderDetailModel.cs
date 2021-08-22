using System.Diagnostics;

namespace FlameAndWax.Data.Models
{
    public class OrderDetailModel
    {
        public int OrderDetailId { get; set; }
        public OrderModel Order { get; set; }
        public ProductModel Product { get; set; }       
        public double TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
