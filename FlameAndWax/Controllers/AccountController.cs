using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICustomerService _customerService;
        public AccountController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> ProcessLogin(LoginViewModel loginCredentials, string returnUrl)
        {
            var isAuthenticated = await _customerService.Login(new CustomerModel { Username = loginCredentials.Username, Password = loginCredentials.Password });
            if (isAuthenticated.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = isAuthenticated.ErrorContent
                };
                return View("Error", error);
            }
            if (isAuthenticated.Result != -1)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginCredentials.Username),
                    new Claim(ClaimTypes.Role, nameof(Constants.Roles.Customer)),
                    new Claim(ClaimTypes.NameIdentifier, isAuthenticated.Result.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                if (Url.IsLocalUrl(returnUrl))
                {                    
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            return View(nameof(Login));
        }

        public async Task<IActionResult> ProcessLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
