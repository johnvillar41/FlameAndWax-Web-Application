using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    [Authorize(Roles = nameof(Constants.Roles.Customer))]
    public class UserProfileController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private string ConnectionString { get; }
        public UserProfileController(
            ICustomerService customerService,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            _customerService = customerService;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var accountDetailServiceResult = await _customerService.FetchAccountDetail(int.Parse(userId), ConnectionString);
            if (accountDetailServiceResult.HasError) return BadRequest(new { errorContent = accountDetailServiceResult.ErrorContent });

            var userProfile = new UserProfileViewModel
            {
                Fullname = accountDetailServiceResult.Result.CustomerName,
                ContactNumber = accountDetailServiceResult.Result.ContactNumber,
                Email = accountDetailServiceResult.Result.Email,
                Address = accountDetailServiceResult.Result.Address,
                Password = accountDetailServiceResult.Result.Password,
                Username = accountDetailServiceResult.Result.Username,
                ProfilePictureLink = accountDetailServiceResult.Result.ProfilePictureLink
            };

            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(UserProfileViewModel userProfile)
        {
            var userId = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var customerModel = new CustomerModel();
            if (userProfile.ProfilePictureFile != null)
            {
                var imageLink = await BuildProfilePictureLink(userProfile.ProfilePictureFile);
                customerModel = new CustomerModel
                {
                    ProfilePictureLink = imageLink
                };
            }

            customerModel.CustomerId = int.Parse(userId);
            customerModel.CustomerName = userProfile.Fullname;
            customerModel.ContactNumber = userProfile.ContactNumber;
            customerModel.Email = userProfile.Email;
            customerModel.Username = userProfile.Username;
            customerModel.Password = userProfile.Password;
            customerModel.Address = userProfile.Address;

            var customerServiceResult = await _customerService.FetchAccountDetail(int.Parse(userId), ConnectionString);
            if (customerServiceResult.HasError) return BadRequest(new { errorContent = customerServiceResult.ErrorContent });

            if (userProfile.ProfilePictureFile != null)
                DeleteOldProfilePicture(customerServiceResult.Result.ProfilePictureLink);

            var modifyServiceResult = await _customerService.ModifyAccountDetails(customerModel, int.Parse(userId), ConnectionString);
            if (modifyServiceResult.HasError) return BadRequest(new { errorContent = modifyServiceResult.ErrorContent });

            var accountDetailServiceResult = await _customerService.FetchAccountDetail(int.Parse(userId), ConnectionString);
            if (accountDetailServiceResult.HasError) return BadRequest(new { errorContent = accountDetailServiceResult.ErrorContent });

            var userProfileViewModel = new UserProfileViewModel
            {
                Fullname = accountDetailServiceResult.Result.CustomerName,
                ContactNumber = accountDetailServiceResult.Result.ContactNumber,
                Email = accountDetailServiceResult.Result.Email,
                Address = accountDetailServiceResult.Result.Address,
                Password = accountDetailServiceResult.Result.Password,
                Username = accountDetailServiceResult.Result.Username,
                ProfilePictureLink = accountDetailServiceResult.Result.ProfilePictureLink
            };
            return PartialView("ProfilePartial", userProfileViewModel);
        }

        private async Task<string> BuildProfilePictureLink(IFormFile profilePictureFile)
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
            return string.Empty;
        }

        private void DeleteOldProfilePicture(string fileToDelete)
        {
            fileToDelete = Path.Combine(_webHostEnvironment.WebRootPath, fileToDelete);
            FileInfo fileInfo = new FileInfo(fileToDelete);
            if (fileInfo != null && fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            GC.Collect();
        }
    }
}

