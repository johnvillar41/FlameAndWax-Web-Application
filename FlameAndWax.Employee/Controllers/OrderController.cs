using System.Linq;
using System.Security.Claims;
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
        private readonly IUserProfileService _userProfileService;
        public string ConnectionString { get; }
        public OrderController(IConfiguration configuration, IOrdersService ordersService, IUserProfileService userProfileService)
        {
            _configuration = configuration;
            _ordersService = ordersService;
            _userProfileService = userProfileService;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 9)
        {
            var userId = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var accountDetailServiceResult = await _userProfileService.FetchAccountDetailAsync(int.Parse(userId), ConnectionString);
            if (accountDetailServiceResult.HasError) return BadRequest(accountDetailServiceResult.ErrorContent);

            var ordersServiceResult = await _ordersService.FetchAllOrdersAsync(null, pageNumber, pageSize, accountDetailServiceResult.Result.CustomerId, ConnectionString);
            if (ordersServiceResult.HasError) return BadRequest(ordersServiceResult.ErrorContent);

            var orderViewModels = ordersServiceResult.Result.Select(orderModel => new OrderViewModel(orderModel)).ToList();
            return View(orderViewModels);
        }
    }
}