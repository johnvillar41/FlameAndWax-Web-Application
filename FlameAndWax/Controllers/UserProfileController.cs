using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configuration;

        private string ConnectionString { get; set; }
        public UserProfileController(ICustomerService customerService, IConfiguration configuration)
        {
            _customerService = customerService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var accountDetailServiceResult = await _customerService.FetchAccountDetail(int.Parse(userId), ConnectionString);
            if (accountDetailServiceResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = accountDetailServiceResult.ErrorContent });

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
        public async Task<IActionResult> Save(UserProfileViewModel userProfile)
        {
            var userId = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var customerModel = new CustomerModel
            {
                CustomerId = int.Parse(userId),
                CustomerName = userProfile.Fullname,
                ContactNumber = userProfile.ContactNumber,
                Email = userProfile.Email,
                Username = userProfile.Username,
                Password = userProfile.Password,
                Address = userProfile.Address
            };

            var modifyServiceResult = await _customerService.ModifyAccountDetails(customerModel, int.Parse(userId), ConnectionString);
            if (modifyServiceResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = modifyServiceResult.ErrorContent });

            var accountDetailServiceResult = await _customerService.FetchAccountDetail(int.Parse(userId), ConnectionString);
            if (accountDetailServiceResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = accountDetailServiceResult.ErrorContent });

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
    }
}

