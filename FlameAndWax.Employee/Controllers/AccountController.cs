using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Employee.Models;
using FlameAndWax.Services.Services.BaseInterface.Interface;
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
        public async Task<IActionResult> ProcessLogin(LoginViewModel login)
        {
            var employeeModel = new EmployeeModel
            {
                Username = login.Username,
                Password = login.Password
            };
            var loginServiceResult = await _accountService.Login(employeeModel, ConnectionString);
            if (loginServiceResult.HasError)
            {
                return BadRequest(loginServiceResult.ErrorContent);
            }
            
            return Ok();
        }
    }
}