using System;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using FlameAndWax.Services.Services.Interfaces;
using FlameAndWax.Services.Services.Models;

namespace FlameAndWax.Services.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IShippingAddressRepository _shippingAddressRepository;
        public UserProfileService(ICustomerRepository customerRepository, IShippingAddressRepository shippingAddressRepository)
        {
            _customerRepository = customerRepository;
            _shippingAddressRepository = shippingAddressRepository;
        }

        public async Task<ServiceResult<int>> AddNewShippingAddressAsync(ShippingAddressModel shippingAddressModel, string connectionString)
        {
            try
            {
                var result = await _shippingAddressRepository.AddAsync(shippingAddressModel, connectionString);
                if (result > 0) return ServiceHelper.BuildServiceResult(result, false, null);
                else return ServiceHelper.BuildServiceResult(result, true, "An Error has occured");
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<int>(-1, true, "Error Adding Shipping Address"); }

        }

        public async Task<ServiceResult<CustomerModel>> FetchAccountDetailAsync(int customerId, string connectionString)
        {
            try
            {
                var customer = await _customerRepository.FetchAsync(customerId, connectionString);
                var addresses = await _shippingAddressRepository.FetchPaginatedResultAsync(1, 100, connectionString);
                customer.Addresses = addresses;

                if (customer == null) return ServiceHelper.BuildServiceResult<CustomerModel>(new CustomerModel(), true, "Customer not found!");
                return ServiceHelper.BuildServiceResult<CustomerModel>(customer, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<CustomerModel>(null, false, e.Message); }
        }
        public async Task<ServiceResult<bool>> ModifyAccountDetailsAsync(CustomerModel modifiedAccount, int customerId, string connectionString)
        {
            if (modifiedAccount == null)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Modified Account details has no value!");
            try
            {
                await _customerRepository.UpdateAsync(modifiedAccount, customerId, connectionString);
                return ServiceHelper.BuildServiceResult<bool>(true, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<bool>(false, true, e.Message); }
        }

        public async Task<ServiceResult<bool>> ModifyShippingAddressAsync(ShippingAddressModel shippingAddressModel, string connectionString)
        {
            try
            {
                var isShippingAddressAvailable = await _customerRepository.CheckIfCustomerHasShippingAddressAsync(shippingAddressModel.CustomerId, connectionString);
                if (isShippingAddressAvailable)
                    await _shippingAddressRepository.UpdateAsync(shippingAddressModel, shippingAddressModel.ShippingAddressId, connectionString);
                else
                {
                    var primaryKey = await _shippingAddressRepository.AddAsync(shippingAddressModel, connectionString);
                    await _customerRepository.UpdateShippingAddressIdAsync(shippingAddressModel.CustomerId, primaryKey, connectionString);
                }

                return ServiceHelper.BuildServiceResult<bool>(true, false, null);
            }
            catch (Exception e)
            {
                return ServiceHelper.BuildServiceResult<bool>(false, true, e.Message);
            }
        }
    }
}