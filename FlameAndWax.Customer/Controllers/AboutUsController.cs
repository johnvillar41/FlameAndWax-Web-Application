using Microsoft.AspNetCore.Mvc;

namespace FlameAndWax.Customer.Controllers
{
    public class AboutUsController : Controller
    {        
       
        public IActionResult Index()
        {
            return View();
        }        
    }
}
