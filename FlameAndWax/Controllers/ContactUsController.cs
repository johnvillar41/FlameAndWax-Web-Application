using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configuration;

        private string ConnectionString { get; set; }

        public ContactUsController(ICustomerService customerService, IConfiguration configuration)
        {
            _customerService = customerService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(MessageModel messageModel)
        {
            var customerServiceResult = await _customerService.SendMessage(messageModel, ConnectionString);
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
