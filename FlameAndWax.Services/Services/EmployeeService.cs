using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IEmployeeRepository employeeRepository)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _employeeRepository = employeeRepository;
        }
        public async Task<ServiceResult<bool>> DeactivateCustomerAccount(int customerId = 0)
        {
            if (customerId == 0)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Employee Id not defined");

            await _customerRepository.ChangeCustomerStatus(customerId, Constants.CustomerAccountStatus.Banned);
            return ServiceHelper.BuildServiceResult<bool>(true, false, null);
        }

        public async Task<ServiceResult<bool>> DeleteCustomerAccount(int employeeId = 0)
        {
            if (employeeId == 0)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Employee Id is not defined!");

            await _customerRepository.Delete(employeeId);
            return ServiceHelper.BuildServiceResult<bool>(true, false, null);
        }

        public async Task<ServiceResult<IEnumerable<CustomerModel>>> FetchAllCustomerAccounts(int pageNumber, int pageSize)
        {
            var customers = await _customerRepository.FetchPaginatedResult(pageNumber,pageSize);
            return ServiceHelper.BuildServiceResult<IEnumerable<CustomerModel>>(customers, false, null);
        }

        public async Task<ServiceResult<IEnumerable<ProductModel>>> FetchAllProducts(int pageNumber, int pageSize)
        {
            var products = await _productRepository.FetchPaginatedResult(pageNumber,pageSize);
            return ServiceHelper.BuildServiceResult<IEnumerable<ProductModel>>(products, false, null);
        }

        public async Task<ServiceResult<int>> Login(EmployeeModel loginCredentials)
        {
            var isLoggedIn = await _employeeRepository.Login(loginCredentials);
            if (isLoggedIn == -1)
                return ServiceHelper.BuildServiceResult<int>(-1, true, "Invalid credentials!");

            return ServiceHelper.BuildServiceResult<int>(isLoggedIn, false, null);
        }

        public async Task<ServiceResult<bool>> ModifyProduct(ProductModel updatedProduct, int productId = 0)
        {
            if (updatedProduct == null)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Empty Product");
            if (productId == 0)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Product Id is not defined!");

            await _productRepository.Update(updatedProduct, productId);
            return ServiceHelper.BuildServiceResult<bool>(true, false, null);
        }

        public async Task<ServiceResult<bool>> Register(EmployeeModel registeredCredentials)
        {
            if (registeredCredentials == null)
                return ServiceHelper.BuildServiceResult<bool>(false, false, "Employee data is empty!");

            await _employeeRepository.Add(registeredCredentials);
            return ServiceHelper.BuildServiceResult<bool>(true, false, null);
        }
    }
}
