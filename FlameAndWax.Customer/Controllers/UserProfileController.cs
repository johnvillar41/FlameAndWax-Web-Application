using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Customer.Models;
using FlameAndWax.Services.Repositories.Interfaces;
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
using System.Net.Http;
using Newtonsoft.Json;

namespace FlameAndWax.Customer.Controllers
{
    [Authorize(Roles = nameof(Constants.Roles.Customer))]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private string ConnectionString { get; }
        public UserProfileController(
            IUserProfileService userProfileService,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            ICustomerRepository customerRepository)
        {
            _userProfileService = userProfileService;
            _configuration = configuration;
            _customerRepository = customerRepository;
            _webHostEnvironment = webHostEnvironment;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var accountDetailServiceResult = await _userProfileService.FetchAccountDetailAsync(int.Parse(userId), ConnectionString);
            if (accountDetailServiceResult.HasError) return BadRequest(new { errorContent = accountDetailServiceResult.ErrorContent });

            if (!await _customerRepository.CheckIfCustomerHasShippingAddressAsync(int.Parse(userId), ConnectionString))
            {
                ViewData["IsShippingAddressPresent"] = false;
            }
            else
            {
                ViewData["IsShippingAddressPresent"] = true;
            }

            var userProfile = new UserProfileViewModel(accountDetailServiceResult.Result);

            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(UserProfileViewModel userProfile)
        {
            var userId = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var customerModel = new CustomerModel();
            var imageLink = await BuildProfilePictureLink(userProfile.ProfilePictureFile);
            customerModel = new CustomerModel
            {
                ProfilePictureLink = imageLink[0]
            };

            customerModel.CustomerId = int.Parse(userId);
            customerModel.CustomerName = userProfile.Fullname;
            customerModel.ContactNumber = userProfile.ContactNumber;
            customerModel.Email = userProfile.Email;
            customerModel.Username = userProfile.Username;
            customerModel.Password = userProfile.Password;

            var customerServiceResult = await _userProfileService.FetchAccountDetailAsync(int.Parse(userId), ConnectionString);
            if (customerServiceResult.HasError) return BadRequest(new { errorContent = customerServiceResult.ErrorContent });

            if (customerServiceResult.Result.ProfilePictureLink != null)
                await DeleteOldProfilePicture(customerServiceResult.Result.ProfilePictureLink);

            var modifyServiceResult = await _userProfileService.ModifyAccountDetailsAsync(customerModel, int.Parse(userId), ConnectionString);
            if (modifyServiceResult.HasError) return BadRequest(new { errorContent = modifyServiceResult.ErrorContent });

            var accountDetailServiceResult = await _userProfileService.FetchAccountDetailAsync(int.Parse(userId), ConnectionString);
            if (accountDetailServiceResult.HasError) return BadRequest(new { errorContent = accountDetailServiceResult.ErrorContent });

            var userProfileViewModel = new UserProfileViewModel(accountDetailServiceResult.Result);
            return PartialView("ProfilePartial", userProfileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveShippingAddress(ShippingAddressViewModel shippingAddressViewModel)
        {
            var userId = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var shippingAddressModel = new ShippingAddressModel
            {
                ShippingAddressId = shippingAddressViewModel.ShippingAddressId,
                CustomerId = int.Parse(userId),
                Address = shippingAddressViewModel.Address,
                PostalCode = shippingAddressViewModel.PostalCode,
                City = shippingAddressViewModel.City,
                Region = shippingAddressViewModel.Region,
                Country = shippingAddressViewModel.Country
            };
            var shippingAddressServiceResult = await _userProfileService.ModifyShippingAddressAsync(shippingAddressModel, ConnectionString);
            if (shippingAddressServiceResult.HasError) return BadRequest();

            return PartialView("ShippingAddressPartial", shippingAddressViewModel);
        }

        private async Task<string[]> BuildProfilePictureLink(IFormFile profilePictureFile)
        {
            var fileExtension = Path.GetExtension(profilePictureFile.FileName);
            var guid = Guid.NewGuid();
            if (fileExtension.Equals(".JPG", StringComparison.CurrentCultureIgnoreCase) ||
                fileExtension.Equals(".PNG", StringComparison.CurrentCultureIgnoreCase) ||
                fileExtension.Equals(".JPEG", StringComparison.CurrentCultureIgnoreCase))
            {
                using var httpClient = new HttpClient();
                var form = new MultipartFormDataContent();
                var ms = new MemoryStream();
                profilePictureFile.CopyTo(ms);
                var fileBytes = ms.ToArray();

                form.Add(new ByteArrayContent(fileBytes, 0, fileBytes.Length), "profilePictureFile", profilePictureFile.FileName);
                using HttpResponseMessage response = await httpClient.PostAsync($"{Constants.BASE_URL_API_IMAGES}/api/Images", form);

                response.EnsureSuccessStatusCode();

                dynamic jsonResult = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.ToString());
                var filePath = jsonResult.filePath;
                var fileName = jsonResult.fileName;
                var basePath = jsonResult.basePath;

                return new string[3] { filePath, fileName, basePath };
            }
            return null;
        }

        private static async Task<bool> DeleteOldProfilePicture(string imageToDelete)
        {
            using var httpClient = new HttpClient();
            var form = new MultipartFormDataContent();
            form.Add(new StringContent(imageToDelete));
            using HttpResponseMessage response = await httpClient.PostAsync($"{Constants.BASE_URL_API_IMAGES}/api/Images/DeleteProfilePicture", form);
            return response.IsSuccessStatusCode;
        }
    }
}

