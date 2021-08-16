using Microsoft.AspNetCore.Mvc;

namespace FlameAndWax.Controllers
{
    public class HomeController : Controller
    {        
        public HomeController()
        {
          
        }

        public IActionResult Index()
        {           
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }       

        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
