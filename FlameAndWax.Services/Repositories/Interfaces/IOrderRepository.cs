using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<OrderModel>
    {
        Task<IEnumerable<OrderModel>> FetchPaginatedCategorizedOrdersAsync(int pageNumber, int pagSize, int customerId, OrderStatus category, string connectionString);
        Task<int> FetchTotalNumberOfOrdersAsync(OrderStatus? orderStatus, string connection, int customerId);
        Task<IEnumerable<OrderModel>> FetchAllOrdersAsync(int pageNumber, int pageSize, int customerId, string connectionString);
    }
}
