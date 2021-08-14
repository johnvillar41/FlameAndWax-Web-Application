using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public async Task Add(ProductModel Data)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "INSERT INTO ProductsTable(ProductName,ProductDescription,ProductPrice,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder)" +
                "VALUES(@name,@desc,@price,@quantity,@unitprice,@stock,@order)";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@name", Data.ProductName);
            command.Parameters.AddWithValue("@desc", Data.ProductDescription);
            command.Parameters.AddWithValue("@price", Data.ProductPrice);
            command.Parameters.AddWithValue("@quantity", Data.QuantityPerUnit);
            command.Parameters.AddWithValue("@unitprice", Data.UnitPrice);
            command.Parameters.AddWithValue("@stock", Data.UnitsInStock);
            command.Parameters.AddWithValue("@order", Data.UnitsInOrder);
            await command.ExecuteNonQueryAsync();
        }

        public async Task Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "DELETE FROM ProductsTable WHERE ProductId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<ProductModel> Fetch(int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM ProductsTable WHERE ProductId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if(await reader.ReadAsync())
            {
                return new ProductModel
                {
                    ProductId = id,
                    ProductName = reader["ProductName"].ToString(),
                    ProductDescription = reader["ProductDescription"].ToString(),
                    ProductPrice = double.Parse(reader["ProductPrice"].ToString()),
                    QuantityPerUnit = int.Parse(reader["QuantityPerUnit"].ToString()),
                    UnitPrice = double.Parse(reader["UnitPrice"].ToString()),
                    UnitsInStock = int.Parse(reader["UnitsInStock"].ToString()),
                    UnitsInOrder = int.Parse(reader["UnitsOnOrder"].ToString())
                };
            }
            return null;
        }

        public async Task<IEnumerable<ProductModel>> FetchAll()
        {
            List<ProductModel> products = new List<ProductModel>();

            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM ProductsTable";
            using SqlCommand command = new SqlCommand(queryString, connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while(await reader.ReadAsync())
            {
                products.Add(
                        new ProductModel
                        {
                            ProductId = int.Parse(reader["ProductId"].ToString()),
                            ProductName = reader["ProductName"].ToString(),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            ProductPrice = double.Parse(reader["ProductPrice"].ToString()),
                            QuantityPerUnit = int.Parse(reader["QuantityPerUnit"].ToString()),
                            UnitPrice = double.Parse(reader["UnitPrice"].ToString()),
                            UnitsInStock = int.Parse(reader["UnitsInStock"].ToString()),
                            UnitsInOrder = int.Parse(reader["UnitsOnOrder"].ToString())
                        }
                    );
            }
            return products;
        }

        public async Task ModifyNumberOfStocks(int productId, int numberOfStocksToBeSubtracted)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "UPDATE ProductsTable SET UnitsInStock = @unitsToBeSubracted WHERE ProductId = @id";
            using SqlCommand command = new SqlCommand(queryString,connection);
            await command.ExecuteNonQueryAsync();
        }

        public async Task Update(ProductModel data, int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "UPDATE ProductsTable SET ProductName = @name, ProductDescription = @desc, ProductPrice = @price" +
                "QuantityPerUnit = @qty, UnitPrice = @unitPrice, UnitsInStock = @unitStock, UnitsOnOrder = @unitOrder WHERE " +
                "ProductId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@name", data.ProductName);
            command.Parameters.AddWithValue("@desc", data.ProductDescription);
            command.Parameters.AddWithValue("@price", data.ProductPrice);
            command.Parameters.AddWithValue("@qty", data.QuantityPerUnit);
            command.Parameters.AddWithValue("@unitPrice", data.UnitPrice);
            command.Parameters.AddWithValue("@unitStock", data.UnitsInStock);
            command.Parameters.AddWithValue("@unitOrder", data.UnitsInOrder);
            command.Parameters.AddWithValue("@id", data.ProductId);
            await command.ExecuteNonQueryAsync();
        }
    }
}
