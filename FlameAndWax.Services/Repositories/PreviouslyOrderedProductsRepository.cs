using FlameAndWax.Data.Constants;
using FlameAndWax.Services.Repositories.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class PreviouslyOrderedProductsRepository : IPreviouslyOrderedProductsRepository
    {
        public async Task<bool> HasCustomerOrderedAProduct(int productId, int customerId)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM PreviouslyOrderedProductsTable WHERE ProductId = @productId AND CustomerId = @customerId";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@productId", productId);
            command.Parameters.AddWithValue("@customerId", customerId);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if(await reader.ReadAsync())
            {
                return true;
            }
            return false;
        }
    }
}
