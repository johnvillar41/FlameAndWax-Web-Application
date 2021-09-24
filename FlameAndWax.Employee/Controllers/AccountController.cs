using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlameAndWax.Employee.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlameAndWax.Employee.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessLogin(LoginViewModel login)
        {
            throw new NotImplementedException();
        }
    }
}