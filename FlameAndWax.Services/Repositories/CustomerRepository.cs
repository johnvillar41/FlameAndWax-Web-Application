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
        public async Task<int> AddAsync(CustomerModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("AddNewCustomer", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@name", Data.CustomerName);
            command.Parameters.AddWithValue("@number", Data.ContactNumber);
            command.Parameters.AddWithValue("@email", Data.Email);
            command.Parameters.AddWithValue("@username", Data.Username);
            command.Parameters.AddWithValue("@password", Data.Password);
            command.Parameters.AddWithValue("@status", CustomerAccountStatus.Pending.ToString());
            command.Parameters.AddWithValue("@code", Data.Code);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var primaryKey = int.Parse(reader["pk"].ToString());
                return primaryKey;
            }
            return -1;
        }

        public async Task ChangeCustomerStatusAsync(int customerId, CustomerAccountStatus customerStatus, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("ChangeCustomerStatus", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@status", customerStatus.ToString());
            command.Parameters.AddWithValue("@id", customerId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> CheckIfCustomerHasShippingAddressAsync(int customerId, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("CheckIfCustomerHasShippingAddress", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@customerId", customerId);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return true;
            }
            return false;
        }

        public async Task DeleteAsync(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "DELETE FROM CustomerTable WHERE CustomerId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<CustomerModel> FetchAsync(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchCustomer", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                ShippingAddressModel shippingAddressModel = new ShippingAddressModel();
                if (!DBNull.Value.Equals(reader["ShippingAddressId"]))
                {
                    shippingAddressModel = await _shippingAddressRepository.FetchAsync(int.Parse(reader["ShippingAddressId"].ToString()), connectionString);
                }

                return new CustomerModel
                {
                    CustomerId = int.Parse(reader["CustomerId"].ToString()),
                    CustomerName = reader["CustomerName"].ToString(),
                    ContactNumber = reader["ContactNumber"].ToString(),
                    Email = reader["Email"].ToString(),
                    Username = reader["Username"].ToString(),
                    ProfilePictureLink = reader["ProfilePictureLink"].ToString(),
                    Status = ServiceHelper.ConvertStringToCustomerAccountStatus(reader["Status"].ToString()),
                    Address = shippingAddressModel
                };
            }
            return null;
        }

        public async Task<IEnumerable<CustomerModel>> FetchPaginatedResultAsync(int pageNumber, int pageSize, string connectionString)
        {
            List<CustomerModel> customers = new List<CustomerModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchPaginatedResultCustomer", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
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
                        Address = await _shippingAddressRepository.FetchAsync(int.Parse(reader["ShippingAddressId"].ToString()), connectionString)
                    }
                );
            }
            return customers;
        }
        /// <summary>
        ///     return -1 means Account is still pending
        ///     return -2 means Account not found
        ///     return > 0 means Login Successfull
        /// </summary>
        /// <param name="loginCustomer"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public async Task<int> LoginCustomerAccountAsync(CustomerModel loginCustomer, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("LoginCustomerAccount", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@username", loginCustomer.Username);
            command.Parameters.AddWithValue("@password", loginCustomer.Password);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var customerId = int.Parse(reader["CustomerId"].ToString());
                if (reader["Status"].ToString().Equals(nameof(CustomerAccountStatus.Pending)))
                {
                    return -1;
                }
                return customerId;
            }
            return -2;
        }

        public async Task UpdateAsync(CustomerModel data, int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var storedProcedure = "";

            if (data.ProfilePictureLink != null)
                storedProcedure = "UpdateCustomerWithPicture";

            else
                storedProcedure = "UpdateCustomerWithoutPicture";

            using SqlCommand command = new SqlCommand(storedProcedure, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            if (data.ProfilePictureLink != null)
                command.Parameters.AddWithValue("@dp", data.ProfilePictureLink);

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", data.CustomerName);
            command.Parameters.AddWithValue("@number", data.ContactNumber);
            command.Parameters.AddWithValue("@email", data.Email);
            command.Parameters.AddWithValue("@username", data.Username);
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateShippingAddressIdAsync(int customerId, int shippingAddressId, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("UpdateShippingAddressId", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@shippingId", shippingAddressId);
            command.Parameters.AddWithValue("@customerId", customerId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> UpdateStatusCustomerAccountAsync(string username, string connectionString, string code)
        {            
            var userCode = await GetUserCode(username, connectionString);
            if (!userCode.Equals(code))
                return false;
           
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "UPDATE CustomerTable SET Status = 'Active' WHERE Username = @username";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@username", username);
            await command.ExecuteNonQueryAsync();
            return true;
        }
        private async Task<string> GetUserCode(string username, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT Code FROM CustomerTable WHERE Username = @username";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@username", username);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader["Code"].ToString();
            }
            return string.Empty;
        }
    }
}
