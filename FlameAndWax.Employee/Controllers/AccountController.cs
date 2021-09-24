using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Employee.Models;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FlameAndWax.Employee.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountBaseService<EmployeeModel> _accountService;
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; }
        public AccountController(IAccountBaseService<EmployeeModel> accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessLogin(LoginViewModel login, string returnUrl)
        {
            var employeeModel = new EmployeeModel
            {
                Username = login.Username,
                Password = login.Password
            };

            var loginServiceResult = await _accountService.Login(employeeModel, ConnectionString);
            if (loginServiceResult.HasError)
                return BadRequest(loginServiceResult.ErrorContent);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, employeeModel.Username),
                    new Claim(ClaimTypes.Role, nameof(Constants.Roles.Employee)),
                    new Claim(ClaimTypes.NameIdentifier, loginServiceResult.Result.ToString())
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
                return Ok(returnUrl);
            }
            returnUrl = "/Home/Index";
            return Ok(returnUrl);
        }
    }
}