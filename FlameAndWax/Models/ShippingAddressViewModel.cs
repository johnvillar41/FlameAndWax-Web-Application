using FlameAndWax.Data.Models;

namespace FlameAndWax.Models
{
    public class ShippingAddressViewModel
    {
        public int ShippingAddressId { get; set; }
        public string Address { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public ShippingAddressViewModel(ShippingAddressModel shippingAddressModel)
        {
            Address = shippingAddressModel.Address;
            PostalCode = shippingAddressModel.PostalCode;
            City = shippingAddressModel.City;
            Region = shippingAddressModel.Region;
            Country = shippingAddressModel.Country;
            ShippingAddressId = shippingAddressModel.ShippingAddressId;
        }
        public ShippingAddressViewModel()
        {

        }
    }
}