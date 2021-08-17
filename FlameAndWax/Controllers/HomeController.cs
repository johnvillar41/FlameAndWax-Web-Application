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
    }
}
