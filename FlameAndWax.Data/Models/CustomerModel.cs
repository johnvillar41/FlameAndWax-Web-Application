using static FlameAndWax.Data.Constants.Constants;
namespace FlameAndWax.Data.Models
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ProfilePictureLink { get; set; }
        public CustomerAccountStatus Status { get; set; }
        public ShippingAddressModel Address { get; set; }
        public string Code { get; set; }
    }
}
