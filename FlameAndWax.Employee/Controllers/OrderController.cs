using System.Linq;
using System.Threading.Tasks;
using FlameAndWax.Employee.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FlameAndWax.Employee.Controllers
{
    public class OrderController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IOrdersService _ordersService;
        public string ConnectionString { get; }
        public OrderController(IConfiguration configuration, IOrdersService ordersService)
        {
            _configuration = configuration;
            _ordersService = ordersService;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 9)
        {
            var ordersServiceResult = await _ordersService.FetchAllOrders(null, pageNumber, pageSize, ConnectionString);
            if (ordersServiceResult.HasError) return BadRequest(ordersServiceResult.ErrorContent);

            var orderViewModels = ordersServiceResult.Result.Select(orderModel => new OrderViewModel(orderModel)).ToList();
            return View(orderViewModels);
        }
    }
}