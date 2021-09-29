using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using FlameAndWax.Services.Services.Interfaces;
using FlameAndWax.Services.Services.Models;

namespace FlameAndWax.Services.Services
{
    public class EmployeeAccountService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeAccountService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<PagedServiceResult<IEnumerable<EmployeeModel>>> FetchAllEmployees(int pageNumber, int pageSize, string connectionString)
        {
            var employees = await _employeeRepository.FetchPaginatedResult(pageNumber, pageSize, connectionString);
            var employeesTotalCount = await _employeeRepository.FetchTotalEmployeesCount(connectionString);

            var serviceResult = ServiceHelper.BuildServiceResult<IEnumerable<EmployeeModel>>(employees, false, null);
            return ServiceHelper.BuildPagedResult<IEnumerable<EmployeeModel>>(serviceResult, pageNumber, employeesTotalCount);
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