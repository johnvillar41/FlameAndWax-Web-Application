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
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("AddNewEmployee", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FirstName", Data.FirstName);
            command.Parameters.AddWithValue("@LastName", Data.LastName);
            command.Parameters.AddWithValue("@Email", Data.Email);
            command.Parameters.AddWithValue("@PhotoLink", Data.PhotoLink);
            command.Parameters.AddWithValue("@Bday", Data.DateBirth);
            command.Parameters.AddWithValue("@HireDate", Data.HireDate);
            command.Parameters.AddWithValue("@City", Data.City);
            command.Parameters.AddWithValue("@Username", Data.Username);
            command.Parameters.AddWithValue("@Password", Data.Password);
            command.Parameters.AddWithValue("@Status", Data.Status);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return int.Parse(reader["pk"].ToString());
            }
            return -1;
        }

        public async Task DeleteAsync(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("DeleteEmployee", connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<EmployeeModel> FetchAsync(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchEmployee", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new EmployeeModel
                {
                    EmployeeId = int.Parse(reader["EmployeeId"].ToString()),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Email = reader["Email"].ToString(),
                    PhotoLink = reader["PhotoLink"].ToString(),
                    DateBirth = DateTime.Parse(reader["BirthDate"].ToString()),
                    HireDate = DateTime.Parse(reader["HireDate"].ToString()),
                    City = reader["City"].ToString(),
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                    Status = ServiceHelper.ConvertStringToEmployeeAccountStatus(reader["Status"].ToString())
                };
            }
            return null;
        }

        public async Task<IEnumerable<EmployeeModel>> FetchPaginatedResultAsync(int pageNumber, int pageSize, string connectionString)
        {
            List<EmployeeModel> employeeModels = new List<EmployeeModel>();
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchPaginatedResultEmployee", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                employeeModels.Add(
                    new EmployeeModel
                    {
                        EmployeeId = int.Parse(reader["EmployeeId"].ToString()),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhotoLink = reader["PhotoLink"].ToString(),
                        DateBirth = DateTime.Parse(reader["BirthDate"].ToString()),
                        HireDate = DateTime.Parse(reader["HireDate"].ToString()),
                        City = reader["City"].ToString(),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Status = ServiceHelper.ConvertStringToEmployeeAccountStatus(reader["Status"].ToString())
                    }
                );
            }
            return employeeModels;
        }

        public async Task<int> FetchTotalEmployeesCountAsync(string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchTotalEmployeeCount", connection);
            command.CommandType = CommandType.StoredProcedure;
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return int.Parse(reader["total"].ToString());
            }
            return 0;
        }

        public async Task<int> LoginEmployeeAccountAsync(EmployeeModel employeeModel, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT EmployeeId FROM EmployeesTable WHERE Username = @user AND Password = @pass";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@user", employeeModel.Username);
            command.Parameters.AddWithValue("@pass", employeeModel.Password);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return int.Parse(reader["EmployeeId"].ToString());
            }
            return -1;
        }

        public Task UpdateAsync(EmployeeModel data, int id, string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}
