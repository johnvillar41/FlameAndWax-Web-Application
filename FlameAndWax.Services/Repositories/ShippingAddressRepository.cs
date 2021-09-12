using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Repositories.Interfaces;

namespace FlameAndWax.Services.Repositories
{
    public class ShippingAddressRepository : IShippingAddressRepository
    {                
        public async Task<int> Add(ShippingAddressModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "INSERT INTO ShippingAddressTable(CustomerId,Address,PostalCode,City,Region,Country)" +
                "VALUES(@CustomerId,@Address,@PostalCode,@City,@Region,@Country);" +
                "SELECT SCOPE_IDENTITY() as fk;";
            using SqlCommand command = new SqlCommand(queryString, connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return int.Parse(reader["fk"].ToString());
            }
            return -1;
        }

        public async Task Delete(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "DELETE FROM ShippingAddressTable WHERE ShippingAddressId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<ShippingAddressModel> Fetch(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM ShippingAddressTable WHERE ShippingAddressId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
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

        public Task<IEnumerable<ShippingAddressModel>> FetchPaginatedResult(int pageNumber, int pageSize, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(ShippingAddressModel data, int id, string connectionString)
        {
            throw new System.NotImplementedException();
        }
    }
}