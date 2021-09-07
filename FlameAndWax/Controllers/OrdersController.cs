using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    [Authorize(Roles = nameof(Constants.Roles.Customer))]
    public class OrdersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }

        public OrdersController(ICustomerService customerService, IConfiguration configuration)
        {
            _customerService = customerService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
        {
            var customerIdLoggedIn = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var ordersServiceResult = await _customerService.FetchOrders(pageNumber, pageSize, int.Parse(customerIdLoggedIn), ConnectionString);
            if (ordersServiceResult.HasError) return BadRequest(new { errorContent = ordersServiceResult.ErrorContent });

            var totalNumberOfOrdersServiceResult = await _customerService.FetchTotalNumberOfOrdersByOrderStatus(null, ConnectionString);

            var totalNumberOfPages = Math.Ceiling((decimal)totalNumberOfOrdersServiceResult.Result / 9);
            ViewData["OrderCount"] = (int)totalNumberOfPages;
            ViewData["OrderStatus"] = Constants.ALL_ORDERS;

            var orderViewModels = new List<OrderViewModel>();
            foreach (var order in ordersServiceResult.Result)
            {
                var orderDetailsServiceResult = await _customerService.FetchOrderDetails(order.OrderId, ConnectionString);
                if (orderDetailsServiceResult.HasError) return BadRequest(new { errorContent = orderDetailsServiceResult.ErrorContent });

                BuildOrderViewModels(orderDetailsServiceResult.Result, orderViewModels, order);
            }
            return View(orderViewModels);
        }

        public async Task<IActionResult> PageOrders(string orderStatus, int pageNumber = 1, int pageSize = 5)
        {
            var customerIdLoggedIn = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var categorizedOrdersServiceResult = await _customerService.FetchOrdersByStatus(int.Parse(customerIdLoggedIn), ServiceHelper.ConvertStringtoOrderStatus(orderStatus), ConnectionString);
            if (categorizedOrdersServiceResult.HasError) return BadRequest(new { errorContent = categorizedOrdersServiceResult.ErrorContent });

            var totalNumberOfOrdersServiceResult = await _customerService.FetchTotalNumberOfOrdersByOrderStatus(ServiceHelper.ConvertStringtoOrderStatus(orderStatus), ConnectionString);

            var totalNumberOfPages = Math.Ceiling((decimal)totalNumberOfOrdersServiceResult.Result / pageSize);
            ViewData["OrderCount"] = (int)totalNumberOfPages;
            ViewData["OrderStatus"] = orderStatus;

            var orderViewModels = new List<OrderViewModel>();
            foreach (var order in categorizedOrdersServiceResult.Result)
            {
                var orderDetailsServiceResult = await _customerService.FetchOrderDetails(order.OrderId, ConnectionString);
                if (orderDetailsServiceResult.HasError) return BadRequest(new { errorContent = orderDetailsServiceResult.ErrorContent });

                BuildOrderViewModels(orderDetailsServiceResult.Result, orderViewModels, order);
            }
            return PartialView("OrdersPartialView", orderViewModels);
        }

        public IActionResult Sort(string status)
        {           
            return RedirectToAction(nameof(PageOrders), new{orderStatus = status});
        }

        private void BuildOrderViewModels(
            IEnumerable<OrderDetailModel> orderDetailServiceResult,
            List<OrderViewModel> orderViewModels,
            OrderModel order)
        {

            var orderDetails = new List<OrderDetailViewModel>();
            foreach (var orderDetail in orderDetailServiceResult)
            {
                orderDetails.Add(
                        new OrderDetailViewModel
                        {
                            ProductId = orderDetail.Product.ProductId,
                            ProductPictureLink = orderDetail.Product.ProductGallery.FirstOrDefault().PhotoLink,
                            ProductQuantityOrdered = orderDetail.Quantity,
                            SubTotalPrice = orderDetail.TotalPrice,
                            Status = orderDetail.Status
                        }
                    );
            }

            orderViewModels.Add(
                    new OrderViewModel
                    {
                        OrderId = order.OrderId,
                        Date = order.DateOrdered,
                        ModeOfPayment = order.ModeOfPayment,
                        Courier = order.Courier,
                        TotalCost = order.TotalCost,
                        Status = order.Status,
                        OrderDetails = orderDetails
                    }
                );
        }
    }
}
