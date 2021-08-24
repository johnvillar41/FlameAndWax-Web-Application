namespace FlameAndWax.Models
{
    public class OrderDetailViewModel
    {
        public int ProductId { get; set; }
        public string ProductPictureLink { get; set; }
        public int ProductQuantityOrdered { get; set; }
        public double SubTotalPrice { get; set; }
    }
}