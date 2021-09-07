using Microsoft.AspNetCore.Mvc;

namespace FlameAndWax.Controllers
{
    public class AboutUsController : Controller
    {        
       
        public IActionResult Index()
        {
            return View();
        }        
    }
}
