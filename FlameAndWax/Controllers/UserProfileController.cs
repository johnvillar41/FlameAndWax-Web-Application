using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
        public UserProfileController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public IActionResult Index()
        {
            var userId = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.Name).Value;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(UserProfileViewModel userProfile)
        {
            var userId = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.Name).Value;
            var customerModel = new CustomerModel
            {
                CustomerId = int.Parse(userId),
                CustomerName = userProfile.Fullname,
                ContactNumber = userProfile.ContactNumber,
                Email = userProfile.Email,
                Username = userProfile.Username,
                Password = userProfile.Password,
                ProfilePictureLink = null, //TODO FIX
                Address = userProfile.Address
            };
            var modifyServiceResult = await _customerService.ModifyAccountDetails(customerModel,int.Parse(userId));
            if (modifyServiceResult.HasError) return View("Error", new ErrorViewModel { ErrorContent = modifyServiceResult.ErrorContent });
            return RedirectToAction("Index");
        }
    }
}

