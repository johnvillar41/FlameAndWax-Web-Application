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

        public Task Delete(int id, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public Task<ShippingAddressModel> Fetch(int id, string connectionString)
        {
            throw new System.NotImplementedException();
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