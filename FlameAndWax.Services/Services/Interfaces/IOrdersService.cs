using System.Collections.Generic;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.Models;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface IOrdersService
    {
        Task<ServiceResult<IEnumerable<OrderModel>>> FetchOrdersByStatus(int pageNumber, int pageSize, int customerId, OrderStatus status, string connectionString);
        Task<ServiceResult<int>> FetchTotalNumberOfOrdersByOrderStatus(OrderStatus? orderStatus, string connection, int customerId);
        Task<PagedServiceResult<IEnumerable<OrderModel>>> FetchAllOrders(OrderStatus? orderStatus, int pageNumber, int pageSize, string connectionString);
    }
}