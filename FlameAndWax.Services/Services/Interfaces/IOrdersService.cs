using System.Collections.Generic;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.Models;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface IOrdersService
    {
        Task<ServiceResult<IEnumerable<OrderModel>>> FetchOrdersByStatusAsync(int pageNumber, int pageSize, int customerId, OrderStatus status, string connectionString);
        Task<ServiceResult<int>> FetchTotalNumberOfOrdersByOrderStatusAsync(OrderStatus? orderStatus, string connection, int customerId);
        Task<PagedServiceResult<IEnumerable<OrderModel>>> FetchAllOrdersAsync(OrderStatus? orderStatus, int pageNumber, int pageSize, string connectionString);
    }
}