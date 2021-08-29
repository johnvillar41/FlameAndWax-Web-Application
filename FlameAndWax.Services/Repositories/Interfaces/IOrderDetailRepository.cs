using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Data.Repositories.Interfaces
{
    public interface IOrderDetailRepository : IBaseRepository<OrderDetailModel>
    {
        Task<IEnumerable<OrderDetailModel>> FetchOrderDetails(int orderId, string connectionString);        
    }
}
