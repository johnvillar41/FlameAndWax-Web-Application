using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface ICustomerService : ILoginService<CustomerModel>
    {
        Task<IEnumerable<OrderDetailModel>> FetchOrderDetails(int orderId);
        Task AddOrderTransaction(OrderModel newOrder);
    }
}
