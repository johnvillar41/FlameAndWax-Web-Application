using Dapper;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
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
            var id = await connection.QueryFirstOrDefaultAsync<int>("AddNewCustomer",
                new
                {
                    name = Data.CustomerName,
                    number = Data.ContactNumber,
                    email = Data.Email,
                    username = Data.Username,
                    password = Data.Password,
                    status = CustomerAccountStatus.Pending.ToString(),
                    code = Data.Code
                }, commandType: CommandType.StoredProcedure);
            return id;
        }

        public async Task ChangeCustomerStatusAsync(int customerId, CustomerAccountStatus customerStatus, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync("ChangeCustomerStatus",
                new
                {
                    CustomerId = customerId,
                    CustomerStatus = customerStatus.ToString()
                }, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> CheckIfCustomerHasShippingAddressAsync(int customerId, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var result = await connection.QueryFirstOrDefaultAsync<bool>("CheckIfCustomerHasShippingAddress",
                new
                {
                    CustomerId = customerId
                }, commandType: CommandType.StoredProcedure);
            return result;
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
            var customer = await connection.QueryFirstOrDefaultAsync<CustomerModel>("FetchCustomer",
                new
                {
                    id
                }, commandType: CommandType.StoredProcedure);
            return customer;
        }

        public async Task<IEnumerable<CustomerModel>> FetchPaginatedResultAsync(int pageNumber, int pageSize, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var customers = await connection.QueryAsync<CustomerModel>("FetchPaginatedResultCustomer",
                new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }, commandType: CommandType.StoredProcedure);
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
            var result = await connection.QueryFirstOrDefaultAsync("LoginCustomerAccount",
                new
                {
                    loginCustomer.Username,
                    loginCustomer.Password
                }, commandType: CommandType.StoredProcedure);            

            if (result == null)
                return -2;

            var status = result.Status;
            if (status.Equals(nameof(CustomerAccountStatus.Pending)))
                return -1;

            var customerId = result.CustomerId;          
            return customerId;
        }

        public async Task UpdateAsync(CustomerModel data, int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            if (data.ProfilePictureLink == null)
            {
                await connection.ExecuteAsync("UpdateCustomerWithoutPicture",
                new
                {
                    data.CustomerName,
                    data.ContactNumber,
                    data.Email,
                    data.Username,
                    data.CustomerId
                }, commandType: CommandType.StoredProcedure);
            }
            else
            {
                await connection.ExecuteAsync("UpdateCustomerWithPicture",
                new
                {
                    data.CustomerName,
                    data.ContactNumber,
                    data.Email,
                    data.Username,
                    data.CustomerId,
                    data.ProfilePictureLink
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateShippingAddressIdAsync(int customerId, int shippingAddressId, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("UpdateShippingAddressId",
                new
                {
                    ShippingId = shippingAddressId,
                    CustomerId = customerId
                }, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> UpdateStatusCustomerAccountAsync(string username, string connectionString, string code)
        {
            var userCode = await GetUserCode(username, connectionString);
            if (!userCode.Item1.Equals(code))
                return false;

            var transaction = userCode.Item2;
            //var connection = userCode.Item3;
            using var connection = new SqlConnection(connectionString);
            var queryString = "UPDATE CustomerTable SET Status = 'Active' WHERE Username = @Username";
            await connection.ExecuteAsync(queryString,
                new
                {
                    Username = username
                }, transaction);           
            return true;
        }
        private async Task<Tuple<string, IDbTransaction, SqlConnection>> GetUserCode(string username, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var queryString = "SELECT Code FROM CustomerTable WHERE Username = @Username";
            var result = await connection.QueryFirstOrDefaultAsync<string>(queryString,
                new
                {
                    Username = username
                }, transaction: transaction);
            var tuple = new Tuple<string, IDbTransaction, SqlConnection>(result, transaction, connection);
            return tuple;
        }
    }
}
