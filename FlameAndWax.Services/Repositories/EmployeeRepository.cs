using Dapper;
using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public async Task<int> AddAsync(EmployeeModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var result = await connection.ExecuteScalarAsync<int>("AddNewEmployee",
                Data, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task DeleteAsync(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DeleteEmployee",
                new { id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<EmployeeModel> FetchAsync(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var employee = await connection.QueryFirstOrDefaultAsync<EmployeeModel>("FetchEmployee",
                new { id }, commandType: CommandType.StoredProcedure);
            return employee;
        }

        public async Task<IEnumerable<EmployeeModel>> FetchPaginatedResultAsync(int pageNumber, int pageSize, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var employees = await connection.QueryAsync<EmployeeModel>("FetchPaginatedResultEmployee",
                new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }, commandType: CommandType.StoredProcedure);
            return employees;
        }

        public async Task<int> FetchTotalEmployeesCountAsync(string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var employeeCount = await connection.ExecuteScalarAsync<int>("FetchTotalEmployeeCount", commandType: CommandType.StoredProcedure);
            return employeeCount;
        }

        public async Task<int> LoginEmployeeAccountAsync(EmployeeModel employeeModel, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var queryString = "SELECT EmployeeId FROM EmployeesTable WHERE Username = @user AND Password = @pass";
            var employeeId = await connection.ExecuteScalarAsync<int>(queryString,
                new
                {
                    user = employeeModel.Username,
                    pass = employeeModel.Password
                });
            return employeeId;
        }

        public Task UpdateAsync(EmployeeModel data, int id, string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}
