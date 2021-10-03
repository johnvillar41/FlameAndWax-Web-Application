using FlameAndWax.Data.Models;
using FlameAndWax.Customer.Models;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;
using FlameAndWax.Services.Helpers;

namespace FlameAndWax.Customer.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IAccountBaseService<CustomerModel> _accountService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string ConnectionString { get; }
        public RegisterController(
            IAccountBaseService<CustomerModel> accountService,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            _accountService = accountService;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(UserProfileViewModel newUser)
        {
            EmailSender emailSender = new EmailSender(newUser.Email);
            var code = emailSender.RandomString();

            var customerModel = new CustomerModel
            {
                CustomerName = newUser.Fullname,
                ContactNumber = newUser.ContactNumber,
                Email = newUser.Email,
                Address = null,
                Username = newUser.Username,
                Password = newUser.Password,
                Status = CustomerAccountStatus.Pending,
                ProfilePictureLink = await BuildProfilePictureLink(newUser.ProfilePictureFile),
                Code = code
            };

            var registerServiceResult = await _accountService.RegisterAsync(customerModel, ConnectionString);
            if (registerServiceResult.HasError) return BadRequest(new { errorContent = registerServiceResult.ErrorContent });

            await Task.Run(() =>
            {
                emailSender.SendCode(code);
            });

            return Ok();
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
