using System;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.Models;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<ServiceResult<Boolean>> ModifyAccountDetailsAsync(CustomerModel modifiedAccount,
                                                          int customerId,
                                                          string connectionString);
        Task<ServiceResult<CustomerModel>> FetchAccountDetailAsync(int customerId, string connectionString);
        Task<ServiceResult<Boolean>> ModifyShippingAddressAsync(ShippingAddressModel shippingAddressModel, string connectionString);
    }
}