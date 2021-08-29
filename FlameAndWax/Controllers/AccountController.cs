using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        private string ConnectionString { get; set; }

        public AccountController(ICustomerService customerService, IConfiguration configuration)
        {
            _customerService = customerService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> ProcessLogin(LoginViewModel loginCredentials, string returnUrl)
        {
            var isAuthenticatedServiceResult = await _customerService.Login(new CustomerModel { Username = loginCredentials.Username, Password = loginCredentials.Password }, ConnectionString);
            if (isAuthenticatedServiceResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = isAuthenticatedServiceResult.ErrorContent
                };
                return PartialView("Error", error);
            }
            if (isAuthenticatedServiceResult.Result != -1)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginCredentials.Username),
                    new Claim(ClaimTypes.Role, nameof(Constants.Roles.Customer)),
                    new Claim(ClaimTypes.NameIdentifier, isAuthenticatedServiceResult.Result.ToString())
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
            return RedirectToAction("Index", "Home");
        }
    }
}
