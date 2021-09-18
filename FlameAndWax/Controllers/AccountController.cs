using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountBaseService<CustomerModel> _accountService;
        private readonly IConfiguration _configuration;

        private string ConnectionString { get; }

        public AccountController(IAccountBaseService<CustomerModel> accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessLogin(LoginViewModel loginCredentials, string returnUrl)
        {
            var isAuthenticatedServiceResult = await _accountService.Login(new CustomerModel { Username = loginCredentials.Username, Password = loginCredentials.Password }, ConnectionString);
            if (isAuthenticatedServiceResult.HasError) return BadRequest(new { errorContent = isAuthenticatedServiceResult.ErrorContent });

            if (isAuthenticatedServiceResult.Result != -1)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginCredentials.Username),
                    new Claim(ClaimTypes.Role, nameof(Constants.Roles.Customer)),
                    new Claim(ClaimTypes.NameIdentifier, isAuthenticatedServiceResult.Result.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    IsPersistent = false,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Json(returnUrl);
                }
            }
            returnUrl = "/Home/Index";
            return Json(returnUrl);
        }

        public async Task<IActionResult> ProcessLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
