using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<OrderModel>
    {
        Task<IEnumerable<OrderModel>> FetchPaginatedCategorizedOrders(int pageNumber, int pagSize, int customerId, OrderStatus category, string connectionString);
        Task<int> FetchTotalNumberOfOrders(OrderStatus? orderStatus, string connection, int customerId);
        Task<IEnumerable<OrderModel>> FetchAllOrders(int pageNumber,int pageSize,string connectionString);
    }
}
