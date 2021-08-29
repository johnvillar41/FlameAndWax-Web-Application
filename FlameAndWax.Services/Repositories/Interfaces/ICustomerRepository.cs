using FlameAndWax.Data.Models;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Data.Repositories.Interfaces
{
    public interface ICustomerRepository : IBaseRepository<CustomerModel>
    {
        Task<int> LoginCustomerAccount(CustomerModel loginCustomer, string connectionString);
        Task ChangeCustomerStatus(int customerId, CustomerAccountStatus customerStatus, string connectionString);
    }
}
