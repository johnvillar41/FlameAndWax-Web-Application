using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlameAndWax.Employee.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FlameAndWax.Employee.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IEmployeeService _employeeService;
        public string ConnectionString { get; }
        public EmployeeController(IConfiguration configuration, IEmployeeService employeeService)
        {
            _configuration = configuration;
            _employeeService = employeeService;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 9)
        {
            var employeeServiceResult = await _employeeService.FetchAllEmployees(pageNumber, pageSize, ConnectionString);
            if (employeeServiceResult.HasError) return BadRequest(employeeServiceResult.ErrorContent);

            var employeeViewModels = employeeServiceResult.Result.Select(employee => new EmployeeViewModel(employee)).ToList();
            return View(employeeViewModels);
        }
    }
}