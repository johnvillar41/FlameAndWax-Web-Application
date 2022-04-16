using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Repositories.Interfaces;

namespace FlameAndWax.Services.Repositories
{
    public class ShippingAddressRepository : IShippingAddressRepository
    {
        public async Task<int> AddAsync(ShippingAddressModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("AddNewShippingAddress", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CustomerId", Data.CustomerId);
            command.Parameters.AddWithValue("@Address", Data.Address);
            command.Parameters.AddWithValue("@PostalCode", Data.PostalCode);
            command.Parameters.AddWithValue("@City", Data.City);
            command.Parameters.AddWithValue("@Region", Data.Region);
            command.Parameters.AddWithValue("@Country", Data.Country);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return int.Parse(reader["fk"].ToString());
            }
            return -1;
        }

        public async Task DeleteAsync(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("DeleteShippingAddress", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public Task<IEnumerable<ShippingAddressModel>> FetchAll(int customerId, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ShippingAddressModel> FetchAsync(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchShippingAddress", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {

                return new ShippingAddressModel
                {
                    ShippingAddressId = int.Parse(reader["ShippingAddressId"].ToString()),
                    CustomerId = int.Parse(reader["CustomerId"].ToString()),
                    Address = reader["Address"].ToString(),
                    PostalCode = int.Parse(reader["PostalCode"].ToString()),
                    City = reader["City"].ToString(),
                    Region = reader["Region"].ToString(),
                    Country = reader["Country"].ToString()
                };
            }
            return null;
        }

        public Task<IEnumerable<ShippingAddressModel>> FetchPaginatedResultAsync(int pageNumber, int pageSize, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateAsync(ShippingAddressModel data, int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("UpdateShippingAddress", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@address", data.Address);
            command.Parameters.AddWithValue("@postalCode", data.PostalCode);
            command.Parameters.AddWithValue("@city", data.City);
            command.Parameters.AddWithValue("@region", data.Region);
            command.Parameters.AddWithValue("@country", data.Country);
            command.Parameters.AddWithValue("@shippingId", id);
            await command.ExecuteNonQueryAsync();
        }
    }
}