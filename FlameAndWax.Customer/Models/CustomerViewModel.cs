using FlameAndWax.Data.Models;
using System.Linq;

namespace FlameAndWax.Customer.Models
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ContactNumber { get; set; }
        public string ProfilePictureLink { get; set; }
        public string Address { get; set; }
        public CustomerViewModel(CustomerModel customerModel)
        {
            CustomerId = customerModel.CustomerId;
            CustomerName = customerModel.CustomerName;
            ContactNumber = customerModel.ContactNumber;
            ProfilePictureLink = customerModel.ProfilePictureLink;
            if (customerModel.Addresses == null)
                Address = "Shipping Address is null";

            else
                Address = customerModel.Addresses.FirstOrDefault().Address;
        }
        public CustomerViewModel()
        {

        }
    }
}
