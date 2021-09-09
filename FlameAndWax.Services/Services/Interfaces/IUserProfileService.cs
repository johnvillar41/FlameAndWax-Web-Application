using System;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<ServiceResult<Boolean>> ModifyAccountDetails(CustomerModel modifiedAccount,
                                                          int customerId,
                                                          string connectionString);
        Task<ServiceResult<CustomerModel>> FetchAccountDetail(int customerId, string connectionString);
    }
}