using Microsoft.AspNetCore.Mvc;

namespace FlameAndWax.Employee.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}