using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Data.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<OrderModel>
    {
        Task<IEnumerable<OrderModel>> FetchOrdersFromCustomer(int customerId, string connectionString);
    }
}
