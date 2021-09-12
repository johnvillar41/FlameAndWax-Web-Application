using FlameAndWax.Data.Models;
using Microsoft.AspNetCore.Http;

namespace FlameAndWax.Models
{
    public class UserProfileViewModel
    {
        public string Fullname { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string VerifyPassword { get; set; }
        public string Username { get; set; }
        public string ProfilePictureLink { get; set; }
        public IFormFile ProfilePictureFile { get; set; }
        public UserProfileViewModel(CustomerModel customerModel)
        {
            Fullname = customerModel.CustomerName;
            ContactNumber = customerModel.ContactNumber;
            Email = customerModel.Email;
            Address = customerModel.Address.Address;
            Password = customerModel.Password;
            Username = customerModel.Username;
            ProfilePictureLink = customerModel.ProfilePictureLink;
        }
        public UserProfileViewModel()
        {

        }
    }
}
