using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IShippingAddressRepository _shippingAddressRepository;
        public CustomerRepository(IShippingAddressRepository shippingAddressRepository)
        {
            _shippingAddressRepository = shippingAddressRepository;
        }
        public async Task<int> Add(CustomerModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "INSERT INTO CustomerTable(CustomerName,ContactNumber,Email,Username,Password,Status)" +
                "VALUES(@name,@number,@email,@username,@password,@status);" +
                "SELECT SCOPE_IDENTITY() as pk;";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@name", Data.CustomerName);
            command.Parameters.AddWithValue("@number", Data.ContactNumber);
            command.Parameters.AddWithValue("@email", Data.Email);
            command.Parameters.AddWithValue("@username", Data.Username);
            command.Parameters.AddWithValue("@password", Data.Password);
            command.Parameters.AddWithValue("@status", Constants.Constants.CustomerAccountStatus.Pending.ToString());

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

        public async Task<bool> CheckIfCustomerHasShippingAddress(int customerId, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT ShippingAddressId FROM CustomerTable WHERE CustomerId = @customerId AND ShippingAddressId is NOT NULL";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@customerId", customerId);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return true;
            }
            return false;
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
                ShippingAddressModel shippingAddressModel = new ShippingAddressModel();
                if (!DBNull.Value.Equals(reader["ShippingAddressId"]))
                {
                    shippingAddressModel = await _shippingAddressRepository.Fetch(int.Parse(reader["ShippingAddressId"].ToString()), connectionString);
                }

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
                    Address = shippingAddressModel
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
                        Address = await _shippingAddressRepository.Fetch(int.Parse(reader["ShippingAddressId"].ToString()), connectionString)
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
            var queryString = "";

            if (data.ProfilePictureLink != null)
                queryString = "UPDATE CustomerTable SET CustomerName = @name, ContactNumber = @number, Email = @email, Username = @username, " +
                    "Password = @password, ProfilePictureLink = @dp WHERE CustomerId = @id";

            else
                queryString = "UPDATE CustomerTable SET CustomerName = @name, ContactNumber = @number, Email = @email, Username = @username, " +
                    "Password = @password WHERE CustomerId = @id";

            using SqlCommand command = new SqlCommand(queryString, connection);

            if (data.ProfilePictureLink != null)
                command.Parameters.AddWithValue("@dp", data.ProfilePictureLink);

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", data.CustomerName);
            command.Parameters.AddWithValue("@number", data.ContactNumber);
            command.Parameters.AddWithValue("@email", data.Email);
            command.Parameters.AddWithValue("@username", data.Username);
            command.Parameters.AddWithValue("@password", data.Password);
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateShippingAddressId(int customerId, int shippingAddressId, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "UPDATE CustomerTable SET ShippingAddressId = @shippingId WHERE CustomerId = @customerId";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@shippingId", shippingAddressId);
            command.Parameters.AddWithValue("@customerId", customerId);
            await command.ExecuteNonQueryAsync();
        }
    }
}
