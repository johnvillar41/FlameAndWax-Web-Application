using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using FlameAndWax.Services.Helpers;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public async Task<int> Add(CustomerModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "INSERT INTO CustomerTable(CustomerName,ContactNumber,Email,Username,Password,Status,Address)" +
                "VALUES(@name,@number,@email,@username,@password,@status,@address);" +
                "SELECT SCOPE_IDENTITY() as pk;";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@name", Data.CustomerName);
            command.Parameters.AddWithValue("@number", Data.ContactNumber);
            command.Parameters.AddWithValue("@email", Data.Email);
            command.Parameters.AddWithValue("@username", Data.Username);
            command.Parameters.AddWithValue("@password", Data.Password);
            //command.Parameters.AddWithValue("@link", Data.ProfilePictureLink);
            command.Parameters.AddWithValue("@status", Constants.Constants.CustomerAccountStatus.Pending.ToString());
            command.Parameters.AddWithValue("@address", Data.Address);

            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var primaryKey = int.Parse(reader["pk"].ToString());
                return primaryKey;
            }
            return -1;
        }

        public async Task ChangeCustomerStatus(int customerId, CustomerAccountStatus customerStatus, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "UPDATE CustomerTable SET Status = @status WHERE CustomerId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@status", customerStatus.ToString());
            command.Parameters.AddWithValue("@id", customerId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task Delete(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "DELETE FROM CustomerTable WHERE CustomerId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<CustomerModel> Fetch(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM CustomerTable WHERE CustomerId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new CustomerModel
                {
                    CustomerId = int.Parse(reader["CustomerId"].ToString()),
                    CustomerName = reader["CustomerName"].ToString(),
                    ContactNumber = reader["ContactNumber"].ToString(),
                    Email = reader["Email"].ToString(),
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                    ProfilePictureLink = reader["ProfilePictureLink"].ToString(),
                    Status = ServiceHelper.ConvertStringToCustomerAccountStatus(reader["Status"].ToString()),
                    Address = reader["Address"].ToString()
                };
            }
            return null;
        }

        public async Task<IEnumerable<CustomerModel>> FetchPaginatedResult(int pageNumber, int pageSize, string connectionString)
        {
            List<CustomerModel> customers = new List<CustomerModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM CustomerTable ORDER BY CustomerId OFFSET (@PageNumber - 1) * @PageSize ROWS " +
                "FETCH NEXT @PageSize ROWS ONLY";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                customers.Add(
                    new CustomerModel
                    {
                        CustomerId = int.Parse(reader["CustomerId"].ToString()),
                        CustomerName = reader["CustomerName"].ToString(),
                        ContactNumber = reader["ContactNumber"].ToString(),
                        Email = reader["Email"].ToString(),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        ProfilePictureLink = reader["ProfilePictureLink"].ToString(),
                        Status = ServiceHelper.ConvertStringToCustomerAccountStatus(reader["Status"].ToString()),
                        Address = reader["Address"].ToString()
                    }
                );
            }
            return customers;
        }

        public async Task<int> LoginCustomerAccount(CustomerModel loginCustomer, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT CustomerId FROM CustomerTable WHERE Username = @username COLLATE SQL_Latin1_General_CP1_CS_AS AND Password = @password COLLATE SQL_Latin1_General_CP1_CS_AS";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@username", loginCustomer.Username);
            command.Parameters.AddWithValue("@password", loginCustomer.Password);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var customerId = int.Parse(reader["CustomerId"].ToString());
                return customerId;
            }
            return -1;
        }

        public async Task Update(CustomerModel data, int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "UPDATE CustomerTable SET CustomerName = @name, ContactNumber = @number, Email = @email, Username = @username, " +
                "Password = @password, ProfilePictureLink = @dp, Address = @address WHERE CustomerId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@dp", data.ProfilePictureLink);
            command.Parameters.AddWithValue("@name", data.CustomerName);
            command.Parameters.AddWithValue("@number", data.ContactNumber);
            command.Parameters.AddWithValue("@email", data.Email);
            command.Parameters.AddWithValue("@username", data.Username);
            command.Parameters.AddWithValue("@password", data.Password);
            command.Parameters.AddWithValue("@address", data.Address);
            await command.ExecuteNonQueryAsync();
        }
    }
}
