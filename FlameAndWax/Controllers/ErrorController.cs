using FlameAndWax.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(string errorContent)
        {
            return View(new ErrorViewModel { ErrorContent = errorContent});
        }
    }
}
