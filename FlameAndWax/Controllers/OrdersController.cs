using FlameAndWax.Data.Constants;
using FlameAndWax.Models;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ICustomerService _customerService;
        public OrdersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public async Task<IActionResult> Index()
        {
            var customerIdLoggedIn = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var ordersServiceResult = await _customerService.FetchOrders(int.Parse(customerIdLoggedIn));

            if (ordersServiceResult.HasError)
                return PartialView("Error", new ErrorViewModel { ErrorContent = ordersServiceResult.ErrorContent });

            
            var totalCost = 0.0;
            var orderStatus = Constants.OrderDetailStatus.Processing;
            foreach(var order in ordersServiceResult.Result)
            {                
                foreach(var cost in order.OrderDetails)
                {
                    totalCost += cost.TotalPrice;
                    if (cost.Status == orderStatus)
                        orderStatus = Constants.OrderDetailStatus.Processing;
                }
            }

            

            var orderViewModels = new List<OrderViewModel>();
            foreach (var order in ordersServiceResult.Result)
            {
                var orderDetailsServiceResult = await _customerService.FetchOrderDetails(order.OrderId);
                if (orderDetailsServiceResult.HasError)
                    return View("Error", new ErrorViewModel { ErrorContent = orderDetailsServiceResult.ErrorContent });

                var orderDetails = new List<OrderDetailViewModel>();
                foreach(var orderDetail in orderDetailsServiceResult.Result)
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
                            TotalCost = totalCost,
                            OrderStatus = orderStatus,
                            OrderDetails = orderDetails
                        }
                    );
            }
            return View(orderViewModels);
        }
    }
}
