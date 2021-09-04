using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Data.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<OrderModel>
    {
        Task<IEnumerable<OrderModel>> FetchOrdersFromCustomer(int customerId, string connectionString);
        Task<IEnumerable<OrderModel>> FetchCategorizedOrders(int customerId, OrderStatus category, string connectionString);
    }
}
