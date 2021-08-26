using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ICustomerService _customerService;

        public RegisterController(ICustomerService customerService)
        {
            _customerService = customerService;
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
                Status = CustomerAccountStatus.Pending
            };

            await _customerService.Register(customerModel);
            return RedirectToAction(nameof(Index));
        }
    }
}
