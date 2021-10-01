using FlameAndWax.Data.Models;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface ICustomerRepository : IBaseRepository<CustomerModel>
    {
        Task<int> LoginCustomerAccountAsync(CustomerModel loginCustomer, string connectionString);
        Task ChangeCustomerStatusAsync(int customerId, CustomerAccountStatus customerStatus, string connectionString);
        Task<bool> CheckIfCustomerHasShippingAddressAsync(int customerId, string connectionString);
        Task UpdateShippingAddressIdAsync(int customerId, int shippingAddressId, string connectionStrin);
        Task<bool> UpdateStatusCustomerAccountAsync(string username, string connectionString, string code);
    }
}
