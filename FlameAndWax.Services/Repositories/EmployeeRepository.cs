using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using FlameAndWax.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public async Task<int> Add(EmployeeModel Data)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "INSERT INTO EmployeesTable(FirstName,LastName,Email,PhotoLink,BirthDate,HireDate,City,Username,Password,Status)" +
                "VALUES(@FirstName,@LastName,@Email,@PhotoLink,@Bday,@HireDate,@City,@Username,@Password,@Status);" +
                "SELECT SCOPE_IDENTITY() as pk;";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@FirstName", Data.FirstName);
            command.Parameters.AddWithValue("@LastName", Data.LastName);
            command.Parameters.AddWithValue("@Email", Data.Email);
            command.Parameters.AddWithValue("@PhotoLink", Data.PhotoLink);
            command.Parameters.AddWithValue("@Bday", Data.BirthDate);
            command.Parameters.AddWithValue("@HireDate", Data.HireDate);
            command.Parameters.AddWithValue("@City", Data.City);
            command.Parameters.AddWithValue("@Username", Data.Username);
            command.Parameters.AddWithValue("@Password", Data.Password);
            command.Parameters.AddWithValue("@Status", Data.Status);

            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if(await reader.ReadAsync())
            {
                var primaryKey = int.Parse(reader["pk"].ToString());
                return primaryKey;
            }
            return -1;
        }

        public async Task Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "DELETE FROM EmployeesTable WHERE EmployeeId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<EmployeeModel> Fetch(int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM EmployeesTable WHERE EmployeeId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
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
                    BirthDate = DateTime.Parse(reader["BirthDaye"].ToString()),
                    HireDate = DateTime.Parse(reader["HireDate"].ToString()),
                    City = reader["City"].ToString(),
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                    Status = ServiceHelper.ConvertStringToEmployeeAccountStatus(reader["Status"].ToString())
                };
            }
            return null;
        }

        public async Task<IEnumerable<EmployeeModel>> FetchPaginatedResult(int pageNumber, int pageSize)
        {
            List<EmployeeModel> employees = new List<EmployeeModel>();

            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM EmployeesTable ORDER by EmployeeId OFFSET (@PageNumber - 1) * @PageSize ROWS " +
                "FETCH NEXT @PageSize ROWS ONLY";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                employees.Add(
                        new EmployeeModel
                        {
                            EmployeeId = int.Parse(reader["EmployeeId"].ToString()),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            PhotoLink = reader["PhotoLink"].ToString(),
                            BirthDate = DateTime.Parse(reader["BirthDaye"].ToString()),
                            HireDate = DateTime.Parse(reader["HireDate"].ToString()),
                            City = reader["City"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                            Status = ServiceHelper.ConvertStringToEmployeeAccountStatus(reader["Status"].ToString())
                        }
                    );
            }
            return employees;
        }

        public async Task<int> Login(EmployeeModel employee)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM EmployeesTable WHERE Username = @username AND Password = @password";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@username", employee.Username);
            command.Parameters.AddWithValue("@username", employee.Password);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return int.Parse(reader["EmployeeId"].ToString());
            }
            return -1;
        }

        public async Task ModifyEmployeeStatus(int employeeId, Constants.EmployeeAccountStatus accountStatus)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "UPDATE EmployeesTable SET Status = @status WHERE EmployeeId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@status", accountStatus.ToString());
            command.Parameters.AddWithValue("@id", employeeId);
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Updates the employee credentials excluding the PhotoLink property
        /// </summary>       
        public async Task Update(EmployeeModel data, int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "UPDATE EmployeesTable SET FirstName = @firstName, LastName = @lastName, Email = @email, " +
                "Username = @username, Password = @password ," +
                "BirthDate = @bday, HireDate = @hireDate, City = @city WHERE EmployeeId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@firstName", data.FirstName);
            command.Parameters.AddWithValue("@lastName", data.LastName);
            command.Parameters.AddWithValue("@email", data.Email);
            command.Parameters.AddWithValue("@bday", data.BirthDate);
            command.Parameters.AddWithValue("@hireDate", data.HireDate);
            command.Parameters.AddWithValue("@city", data.City);
            command.Parameters.AddWithValue("@username", data.Username);
            command.Parameters.AddWithValue("@password", data.Password);
            command.Parameters.AddWithValue("@id", data.EmployeeId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateProfilePicture(string profileLink)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "UPDATE EmployeesTable SET PhotoLink = @link";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@firstName", profileLink);
            await command.ExecuteNonQueryAsync();
        }
    }
}
