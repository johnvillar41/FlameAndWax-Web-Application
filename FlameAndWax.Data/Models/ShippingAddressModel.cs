namespace FlameAndWax.Data.Models
{
    public class ShippingAddressModel
    {
        public int ShippingAddressId { get; set; }
        public int CustomerId { get; set; }
        public string Address { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }
}