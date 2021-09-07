using FlameAndWax.Data.Models;
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

        private string ConnectionString { get; }

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(MessageModel messageModel)
        {
            var customerServiceResult = await _customerService.SendMessage(messageModel, ConnectionString);
            if (customerServiceResult.HasError) return BadRequest(new { errorContent = customerServiceResult.ErrorContent});
           
            return Ok();
        }
    }
}
