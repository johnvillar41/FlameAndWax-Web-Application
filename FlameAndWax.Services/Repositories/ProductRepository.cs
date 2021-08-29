using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IProductGalleryRepository _productGalleryRepository;
        public ProductRepository(IProductGalleryRepository productGalleryRepository)
        {
            _productGalleryRepository = productGalleryRepository;
        }
        public async Task<int> Add(ProductModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "INSERT INTO ProductsTable(ProductName,ProductDescription,ProductPrice,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,Category)" +
                "VALUES(@name,@desc,@price,@quantity,@unitprice,@stock,@order,@category)";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@name", Data.ProductName);
            command.Parameters.AddWithValue("@desc", Data.ProductDescription);
            command.Parameters.AddWithValue("@price", Data.ProductPrice);
            command.Parameters.AddWithValue("@quantity", Data.QuantityPerUnit);
            command.Parameters.AddWithValue("@unitprice", Data.UnitPrice);
            command.Parameters.AddWithValue("@stock", Data.UnitsInStock);
            command.Parameters.AddWithValue("@order", Data.UnitsInOrder);
            command.Parameters.AddWithValue("@category", nameof(Data.Category));
            await command.ExecuteNonQueryAsync();
            return Data.ProductId;
        }

        public async Task Delete(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "DELETE FROM ProductsTable WHERE ProductId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<ProductModel> Fetch(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM ProductsTable WHERE ProductId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var categoryString = reader["Category"].ToString();
                return new ProductModel
                {
                    ProductId = id,
                    ProductName = reader["ProductName"].ToString(),
                    ProductDescription = reader["ProductDescription"].ToString(),
                    ProductPrice = double.Parse(reader["ProductPrice"].ToString()),
                    QuantityPerUnit = int.Parse(reader["QuantityPerUnit"].ToString()),
                    UnitPrice = double.Parse(reader["UnitPrice"].ToString()),
                    UnitsInStock = int.Parse(reader["UnitsInStock"].ToString()),
                    UnitsInOrder = int.Parse(reader["UnitsOnOrder"].ToString()),
                    ProductGallery = await _productGalleryRepository.FetchAllPicturesForProduct(id,connectionString),
                    Category = ServiceHelper.ConvertStringToConstant(categoryString)
                };
            }
            return null;
        }

        public async Task<IEnumerable<ProductModel>> FetchPaginatedResult(int pageNumber, int pageSize, string connectionString)
        {
            List<ProductModel> products = new List<ProductModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM ProductsTable ORDER by ProductId OFFSET (@PageNumber - 1) * @PageSize ROWS " +
                "FETCH NEXT @PageSize ROWS ONLY";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var categoryString = reader["Category"].ToString();
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
                            UnitsInOrder = int.Parse(reader["UnitsOnOrder"].ToString()),
                            ProductGallery = await _productGalleryRepository.FetchAllPicturesForProduct(int.Parse(reader["ProductId"].ToString()),connectionString),
                            Category = ServiceHelper.ConvertStringToConstant(categoryString)
                        }
                    );
            }
            return products;
        }

        public async Task<IEnumerable<ProductModel>> FetchCategorizedProducts(Constants.Category category,string connectionString)
        {
            List<ProductModel> categorizedProducts = new List<ProductModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM ProductsTable WHERE Category = @category";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@category", category.ToString());
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var categoryString = reader["Category"].ToString();

                categorizedProducts.Add(
                        new ProductModel
                        {
                            ProductId = int.Parse(reader["ProductId"].ToString()),
                            ProductName = reader["ProductName"].ToString(),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            ProductPrice = int.Parse(reader["ProductPrice"].ToString()),
                            QuantityPerUnit = int.Parse(reader["QuantityPerUnit"].ToString()),
                            UnitPrice = double.Parse(reader["UnitPrice"].ToString()),
                            ProductGallery = await _productGalleryRepository.FetchAllPicturesForProduct(int.Parse(reader["ProductId"].ToString()),connectionString),
                            UnitsInStock = int.Parse(reader["UnitsInStock"].ToString()),
                            UnitsInOrder = int.Parse(reader["UnitsOnOrder"].ToString()),
                            Category = ServiceHelper.ConvertStringToConstant(categoryString),
                        }
                    );
            }
            return categorizedProducts;
        }

        public async Task<IEnumerable<ProductModel>> FetchNewArrivedProducts(string connectionString)
        {
            List<ProductModel> newArrivals = new List<ProductModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT TOP 6 * FROM ProductsTable Order By ProductId DESC;";
            using SqlCommand command = new SqlCommand(queryString, connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var categoryString = reader["Category"].ToString();
                newArrivals.Add(
                        new ProductModel
                        {
                            ProductId = int.Parse(reader["ProductId"].ToString()),
                            ProductName = reader["ProductName"].ToString(),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            ProductPrice = double.Parse(reader["ProductPrice"].ToString()),
                            QuantityPerUnit = int.Parse(reader["QuantityPerUnit"].ToString()),
                            UnitPrice = double.Parse(reader["UnitPrice"].ToString()),
                            UnitsInStock = int.Parse(reader["UnitsInStock"].ToString()),
                            UnitsInOrder = int.Parse(reader["UnitsOnOrder"].ToString()),
                            ProductGallery = await _productGalleryRepository.FetchAllPicturesForProduct(int.Parse(reader["ProductId"].ToString()), connectionString),
                            Category = ServiceHelper.ConvertStringToConstant(categoryString)
                        }
                    );
            }
            return newArrivals;
        }

        public async Task ModifyNumberOfStocks(int productId, int numberOfStocksToBeSubtracted, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "UPDATE ProductsTable SET UnitsInStock = @unitsToBeSubracted WHERE ProductId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            await command.ExecuteNonQueryAsync();
        }
        //TODO: Fix the updating should be adding
        public async Task ModifyNumberOfUnitsInOrder(int productId, int numberOfUnitsToBeAdded, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "UPDATE ProductsTable SET UnitsOnOrder = @unitOrder WHERE ProductId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@unitOrder", numberOfUnitsToBeAdded);
            command.Parameters.AddWithValue("@id", productId);
            command.Parameters.AddWithValue("@uniditOrder", productId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task Update(ProductModel data, int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "UPDATE ProductsTable SET ProductName = @name, ProductDescription = @desc, ProductPrice = @price" +
                "QuantityPerUnit = @qty, UnitPrice = @unitPrice, UnitsInStock = @unitStock, UnitsOnOrder = @unitOrder , Category = @category WHERE " +
                "ProductId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@name", data.ProductName);
            command.Parameters.AddWithValue("@desc", data.ProductDescription);
            command.Parameters.AddWithValue("@price", data.ProductPrice);
            command.Parameters.AddWithValue("@qty", data.QuantityPerUnit);
            command.Parameters.AddWithValue("@unitPrice", data.UnitPrice);
            command.Parameters.AddWithValue("@unitStock", data.UnitsInStock);
            command.Parameters.AddWithValue("@unitOrder", data.UnitsInOrder);
            command.Parameters.AddWithValue("@category", nameof(data.Category));
            command.Parameters.AddWithValue("@id", data.ProductId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAddUnitsOnOrder(int productId, int quantity, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "UPDATE ProductsTable SET UnitsOnOrder = UnitsOnOrder + @quantity WHERE ProductId = @productId";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@productId", productId);
            await command.ExecuteNonQueryAsync();
        }
    }
}
