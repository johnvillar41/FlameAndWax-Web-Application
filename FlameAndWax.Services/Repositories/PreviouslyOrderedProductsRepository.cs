using FlameAndWax.Data.Models;
using FlameAndWax.Services.Repositories.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class PreviouslyOrderedProductsRepository : IPreviouslyOrderedProductsRepository
    {
        public async Task<int> AddPreviouslyOrderedProducts(PreviouslyOrderedProductModel previouslyOrderedProduct,string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "INSERT INTO PreviouslyOrderedProductsTable(ProductId,CustomerUsername)" +
                "VALUES(@productId,@customerUsername);" +
                "SELECT SCOPE_IDENTITY() as fk;";
            using SqlCommand command = new SqlCommand(queryString, connection);
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

        public async Task<bool> HasCustomerOrderedAProduct(int productId, string customerUsername,string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM PreviouslyOrderedProductsTable WHERE ProductId = @productId AND CustomerUsername = @customerUsername";
            using SqlCommand command = new SqlCommand(queryString, connection);
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
