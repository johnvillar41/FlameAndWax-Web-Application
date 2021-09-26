using FlameAndWax.Data.Models;

namespace FlameAndWax.Employee.Controllers
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string ProfilePictureLink { get; set; }
        public CustomerViewModel(CustomerModel customer)
        {
            CustomerId = customer.CustomerId;
            CustomerName = customer.CustomerName;
            ContactNumber = customer.ContactNumber;
            Email = customer.Email;
            Username = customer.Username;
            ProfilePictureLink = customer.ProfilePictureLink;
        }
        public CustomerViewModel()
        {

        }
    }
}