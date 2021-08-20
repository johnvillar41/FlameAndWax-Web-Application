using FlameAndWax.Data.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class CartController : Controller
    {
        [Authorize(Roles = nameof(Constants.Roles.Customer))]
        public IActionResult Index(int productId, string user)
        {
            
            return View();
        }
    }
}
