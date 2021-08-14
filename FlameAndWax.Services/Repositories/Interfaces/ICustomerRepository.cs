using FlameAndWax.Data.Models;
using System.Threading.Tasks;

namespace FlameAndWax.Data.Repositories.Interfaces
{
    public interface ICustomerRepository : IBaseRepository<CustomerModel>
    {
        Task<bool> LoginCustomerAccount(CustomerModel loginCustomer);
    }
}
