using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using FlameAndWax.Services.Services.Models;

namespace FlameAndWax.Services.Services.Interfaces
{
    public class CustomerAccountService : IAccountBaseService<CustomerModel>
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerAccountService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<ServiceResult<int>> LoginAsync(CustomerModel loginCredentials, string connectionString)
        {
            if (loginCredentials == null)
                return ServiceHelper.BuildServiceResult<int>(-1, true, "Login Credentials has no value");
            try
            {
                if (loginCredentials.Code != null)
                {
                    var codeResult = await _customerRepository.UpdateStatusCustomerAccountAsync(loginCredentials.Username, connectionString, loginCredentials.Code);
                    if (!codeResult) return ServiceHelper.BuildServiceResult<int>(-1, true, "Code is not the same");
                }
                var isLoggedIn = await _customerRepository.LoginCustomerAccountAsync(loginCredentials, connectionString);
                switch (isLoggedIn)
                {
                    case -1: return ServiceHelper.BuildServiceResult<int>(-1, true, "Account still pending");
                    case -2: return ServiceHelper.BuildServiceResult<int>(-2, true, "User not found!");
                    default: return ServiceHelper.BuildServiceResult<int>(isLoggedIn, false, "Login Successfull");
                }
            }

            catch (System.Data.SqlClient.SqlException e)
            {
                if (e.Message.Equals("The parameterized query '(@username nvarchar(4000),@password nvarchar(4000))SELECT Custom' expects the parameter '@username', which was not supplied."))
                    return ServiceHelper.BuildServiceResult<int>(-1, true, "Please enter username and password!");
                if (e.Message.Equals($"The parameterized query '(@username nvarchar(4000),@password nvarchar({(loginCredentials.Password == null ? 0 : loginCredentials.Password.Length)}))SELECT CustomerI' expects the parameter '@username', which was not supplied."))
                    return ServiceHelper.BuildServiceResult<int>(-1, true, "Please enter a valid username!");
                if (e.Message.Equals($"The parameterized query '(@username nvarchar({(loginCredentials.Username == null ? 0 : loginCredentials.Username.Length)}),@password nvarchar(4000))SELECT CustomerI' expects the parameter '@password', which was not supplied."))
                    return ServiceHelper.BuildServiceResult<int>(-1, true, "Please enter a valid password!");
                return ServiceHelper.BuildServiceResult<int>(-1, true, e.Message);
            }
        }
        public async Task<ServiceResult<bool>> RegisterAsync(CustomerModel registeredCredentials, string connectionString)
        {
            if (registeredCredentials == null)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Registered Data has no value");
            try
            {
                var customerRespositoryResult = await _customerRepository.AddAsync(registeredCredentials, connectionString);
                if (customerRespositoryResult == -1) return ServiceHelper.BuildServiceResult<bool>(false, true, "Error Adding Customer");

                return ServiceHelper.BuildServiceResult<bool>(true, false, null);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                if (e.Message.Equals($"Cannot insert duplicate key row in object 'dbo.CustomerTable' with unique index 'IX_CustomerTable'. The duplicate key value is ({registeredCredentials.Username}).\r\nThe statement has been terminated."))
                    return ServiceHelper.BuildServiceResult<bool>(false, true, "Duplicate Username! Please try a different username");

                return ServiceHelper.BuildServiceResult<bool>(false, true, "Please Fill all the missing fields!");
            }
        }
    }
}