namespace FlameAndWax.Data.Models
{
    public class OrderDetailModel
    {
        public int OrderDetailId { get; set; }
        public ProductModel Product { get; set; }
        public double TotalPrice
        {
            get
            {
                return TotalPrice;
            }
            set
            {
                TotalPrice = Product.ProductPrice * Quantity;
            }
        }
        public int Quantity { get; set; }
    }
}
