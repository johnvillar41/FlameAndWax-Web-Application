using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public async Task<EmployeeModel> Fetch(int id, string connectionString)
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
                    DateBirth = DateTime.Parse(reader["BirthDaye"].ToString()),
                    HireDate = DateTime.Parse(reader["HireDate"].ToString()),
                    City = reader["City"].ToString(),
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                    Status = ServiceHelper.ConvertStringToEmployeeAccountStatus(reader["Status"].ToString())
                };
            }
            return null;
        }

        public async Task<int> LoginEmployeeAccount(EmployeeModel employeeModel, string connectionString)
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
    }
}
