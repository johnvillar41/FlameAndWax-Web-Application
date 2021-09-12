using System;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using FlameAndWax.Services.Services.Models;

namespace FlameAndWax.Services.Services.Interfaces
{
    public class UserProfileService : IUserProfileService
    {
        private ICustomerRepository _customerRepository;
        public UserProfileService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<ServiceResult<CustomerModel>> FetchAccountDetail(int customerId, string connectionString)
        {
            try
            {
                var customer = await _customerRepository.Fetch(customerId, connectionString);
                if (customer == null) return ServiceHelper.BuildServiceResult<CustomerModel>(new CustomerModel(), true, "Customer not found!");
                return ServiceHelper.BuildServiceResult<CustomerModel>(customer, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<CustomerModel>(null, false, e.Message); }
        }
        public async Task<ServiceResult<bool>> ModifyAccountDetails(CustomerModel modifiedAccount, int customerId, string connectionString)
        {
            if (modifiedAccount == null)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Modified Account details has no value!");
            try
            {
                await _customerRepository.Update(modifiedAccount, customerId, connectionString);
                return ServiceHelper.BuildServiceResult<bool>(true, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<bool>(false, true, e.Message); }
        }    
    }
}