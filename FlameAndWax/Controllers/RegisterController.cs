using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string ConnectionString { get; }
        public RegisterController(
            ICustomerService customerService,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            _customerService = customerService;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserProfileViewModel newUser)
        {
            var customerModel = new CustomerModel
            {
                CustomerName = newUser.Fullname,
                ContactNumber = newUser.ContactNumber,
                Email = newUser.Email,
                Address = newUser.Address,
                Username = newUser.Username,
                Password = newUser.Password,
                Status = CustomerAccountStatus.Pending,
                ProfilePictureLink = await BuildProfilePictureLink(newUser.ProfilePictureFile)
            };

            await _customerService.Register(customerModel, ConnectionString);
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> BuildProfilePictureLink(IFormFile profilePictureFile)
        {
            if (profilePictureFile != null)
            {
                var fileExtension = Path.GetExtension(profilePictureFile.FileName);
                var guid = Guid.NewGuid();
                if (fileExtension.Equals(".JPG", StringComparison.CurrentCultureIgnoreCase) ||
                    fileExtension.Equals(".PNG", StringComparison.CurrentCultureIgnoreCase) ||
                    fileExtension.Equals(".JPEG", StringComparison.CurrentCultureIgnoreCase))
                {
                    var saveImage = Path.Combine(_webHostEnvironment.WebRootPath, @"images\customers", $"{guid}{profilePictureFile.FileName}");
                    var stream = new FileStream(saveImage, FileMode.Create);
                    await profilePictureFile.CopyToAsync(stream);
                    return @$"images\customers\{guid}{profilePictureFile.FileName}";
                }
            }
            return string.Empty;
        }
    }
}
