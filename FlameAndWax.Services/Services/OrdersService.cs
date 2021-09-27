using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using FlameAndWax.Services.Services.Interfaces;
using FlameAndWax.Services.Services.Models;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Services.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrderRepository _orderRepository;
        public OrdersService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PagedServiceResult<IEnumerable<OrderModel>>> FetchAllOrders(OrderStatus? orderStatus, int pageNumber, int pageSize, string connectionString)
        {
            try
            {
                var orders = await _orderRepository.FetchAllOrders(pageNumber, pageSize, connectionString);
                var serviceResult = ServiceHelper.BuildServiceResult<IEnumerable<OrderModel>>(orders, false, null);
                return ServiceHelper.BuildPagedResult<IEnumerable<OrderModel>>(serviceResult, -1, -1);
            }
            catch (Exception e)
            {
                var serviceResult = ServiceHelper.BuildServiceResult<IEnumerable<OrderModel>>(null, true, e.Message);
                return ServiceHelper.BuildPagedResult<IEnumerable<OrderModel>>(serviceResult, -1, -1);
            }
        }

        public async Task<ServiceResult<IEnumerable<OrderModel>>> FetchOrdersByStatus(int pageNumber, int pageSize, int customerId, OrderStatus status, string connectionString)
        {
            try
            {
                var categorizedOrders = await _orderRepository.FetchPaginatedCategorizedOrders(pageNumber, pageSize, customerId, status, connectionString);
                return ServiceHelper.BuildServiceResult<IEnumerable<OrderModel>>(categorizedOrders, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<IEnumerable<OrderModel>>(null, true, e.Message); }
        }
        public async Task<ServiceResult<int>> FetchTotalNumberOfOrdersByOrderStatus(OrderStatus? orderStatus, string connection, int customerId)
        {
            try
            {
                var totalNumberOfProducts = await _orderRepository.FetchTotalNumberOfOrders(orderStatus, connection, customerId);
                return ServiceHelper.BuildServiceResult<int>(totalNumberOfProducts, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<int>(-1, true, e.Message); }
        }
    }
}