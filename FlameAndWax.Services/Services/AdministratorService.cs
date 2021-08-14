using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using FlameAndWax.Services.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public AdministratorService(
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IEmployeeRepository employeeRepository)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _employeeRepository = employeeRepository;
        }
        public async Task<ServiceResult<bool?>> DeactivateCustomerAccount(int customerId = 0)
        {
            if (customerId == 0)
                return new ServiceResult<bool?>
                {
                    Result = false,
                    HasError = true,
                    ErrorContent = "Customer Id is not defined!"
                };

            await _customerRepository.ChangeCustomerStatus(customerId, Constants.AccountStatus.Deactivated);
            return new ServiceResult<bool?>
            {
                Result = true,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<bool?>> DeleteCustomerAccount(int employeeId = 0)
        {
            if (employeeId == 0)
                return new ServiceResult<bool?>
                {
                    Result = false,
                    HasError = true,
                    ErrorContent = "Employee Id not defined!"
                };

            await _customerRepository.Delete(employeeId);
            return new ServiceResult<bool?>
            {
                Result = true,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<IEnumerable<CustomerModel>>> FetchAllCustomerAccounts()
        {
            var customers = await _customerRepository.FetchAll();
            return new ServiceResult<IEnumerable<CustomerModel>>
            {
                Result = customers,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<IEnumerable<ProductModel>>> FetchAllProducts()
        {
            var products = await _productRepository.FetchAll();
            return new ServiceResult<IEnumerable<ProductModel>>
            {
                Result = products,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<bool?>> Login(EmployeeModel loginCredentials)
        {
            if (loginCredentials == null)
                return new ServiceResult<bool?>
                {
                    Result = null,
                    HasError = true,
                    ErrorContent = "Empty Employee credentials!"
                };

            await _employeeRepository.Login(loginCredentials);
            return new ServiceResult<bool?>
            {
                Result = true,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<bool?>> MarkEmployeeAsTerminated(int employeeId = 0)
        {
            if (employeeId == 0)
                return new ServiceResult<bool?>
                {
                    Result = false,
                    HasError = true,
                    ErrorContent = "Employee Id is not defined"
                };

            await _employeeRepository.ModifyEmployeeStatus(employeeId, Constants.AccountStatus.Deactivated);
            return new ServiceResult<bool?>
            {
                Result = true,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<bool?>> ModifyProduct(ProductModel updatedProduct, int productId = 0)
        {
            if (updatedProduct == null)
                return new ServiceResult<bool?>
                {
                    Result = false,
                    HasError = true,
                    ErrorContent = "Updated product is empty!"
                };
            if (productId == 0)
                return new ServiceResult<bool?>
                {
                    Result = false,
                    HasError = true,
                    ErrorContent = "Product Id is not defined!"
                };

            await _productRepository.Update(updatedProduct, productId);
            return new ServiceResult<bool?>
            {
                Result = true,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<bool?>> Register(EmployeeModel registeredCredentials)
        {
            if (registeredCredentials == null)
                return new ServiceResult<bool?>
                {
                    Result = false,
                    HasError = true,
                    ErrorContent = "Employee object is empty!"
                };

            await _employeeRepository.Add(registeredCredentials);
            return new ServiceResult<bool?>
            {
                Result = true,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<bool?>> RemoveEmployee(int employeeId = 0)
        {
            if (employeeId == 0)
                return new ServiceResult<bool?>
                {
                    Result = false,
                    HasError = true,
                    ErrorContent = "Employee Id is not defined"
                };

            await _employeeRepository.Delete(employeeId);
            return new ServiceResult<bool?>
            {
                Result = true,
                HasError = false,
                ErrorContent = null
            };
        }
    }
}
