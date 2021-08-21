using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly ICustomerService _customerService;
        public ContactUsController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public IActionResult Index()
        {           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(MessageModel messageModel)
        {
            var customerServiceResult = await _customerService.SendMessage(messageModel);
            if (customerServiceResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = customerServiceResult.ErrorContent
                };
                return View("Error", error);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
