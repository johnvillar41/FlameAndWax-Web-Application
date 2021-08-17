using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class ContactUsController : Controller
    {
        ICustomerService _customerService;
        public ContactUsController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public IActionResult Index(bool isSuccess = false)
        {
            ViewBag.IsSuccess = isSuccess;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(MessageModel messageModel)
        {
            var customerResult = await _customerService.SendMessage(messageModel);
            if (customerResult.HasError)
            {
                var error = new ErrorViewModel
                {
                    ErrorContent = customerResult.ErrorContent
                };
                return View("Error", error);
            }
            return RedirectToAction(nameof(Index), new { isSuccess = true });
        }
    }
}
