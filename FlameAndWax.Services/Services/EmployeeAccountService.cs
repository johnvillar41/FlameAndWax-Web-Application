using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using FlameAndWax.Services.Services.Models;

namespace FlameAndWax.Services.Services
{
    public class EmployeeAccountService : IAccountBaseService<EmployeeModel>
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeAccountService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task<ServiceResult<int>> Login(EmployeeModel loginCredentials, string connectionString)
        {
            var result = await _employeeRepository.LoginEmployeeAccount(loginCredentials, connectionString);
            if (result > 0)
                return ServiceHelper.BuildServiceResult<int>(result, false, null);
            else
                return ServiceHelper.BuildServiceResult<int>(result, true, "Employee not found!");
        }

        public Task<ServiceResult<bool>> Register(EmployeeModel registeredCredentials, string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}