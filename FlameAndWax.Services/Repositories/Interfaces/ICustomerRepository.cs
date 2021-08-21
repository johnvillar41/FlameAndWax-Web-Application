using FlameAndWax.Data.Models;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Data.Repositories.Interfaces
{
    public interface ICustomerRepository : IBaseRepository<CustomerModel>
    {
        Task<int> LoginCustomerAccount(CustomerModel loginCustomer);
        Task ChangeCustomerStatus(int customerId, AccountStatus customerStatus);
    }
}
