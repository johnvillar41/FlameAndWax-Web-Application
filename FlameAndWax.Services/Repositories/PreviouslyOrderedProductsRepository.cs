using FlameAndWax.Data.Models;
using FlameAndWax.Services.Repositories.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class PreviouslyOrderedProductsRepository : IPreviouslyOrderedProductsRepository
    {
        public async Task<int> AddPreviouslyOrderedProductsAsync(PreviouslyOrderedProductModel previouslyOrderedProduct,string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("AddPreviouslyOrderedProducts", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@productId", previouslyOrderedProduct.ProductId);
            command.Parameters.AddWithValue("@customerUsername", previouslyOrderedProduct.CustomerUsername);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var primaryKey = int.Parse(reader["fk"].ToString());
                return primaryKey;
            }
            return -1;
        }

        public async Task<bool> HasCustomerOrderedAProductAsync(int productId, string customerUsername,string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("HasCustomerOrderedAProduct", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@productId", productId);
            command.Parameters.AddWithValue("@customerUsername", customerUsername);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return true;
            }
            return false;
        }
    }
}
