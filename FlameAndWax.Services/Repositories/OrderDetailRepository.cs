using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly IProductRepository _productRepository;
        public OrderDetailRepository(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task Add(OrderDetailModel Data)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "INSERT INTO OrderDetailsTable(ProductId,TotalPrice,Quantity)" +
                "VALUES(@ProductId,@TotalPrice,@Quantity)";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@ProductId", Data.Product.ProductId);
            command.Parameters.AddWithValue("@TotalPrice", Data.TotalPrice);
            command.Parameters.AddWithValue("@Quantity", Data.Quantity);
            await command.ExecuteNonQueryAsync();
        }

        public async Task Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "DELETE FROM OrderDetailsTable WHERE OrderDetailsId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<OrderDetailModel> Fetch(int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM OrderDetailsTable WHERE OrderDetailsId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var orderDetailId = int.Parse(reader["OrderDetailsId"].ToString());
                var productId = int.Parse(reader["ProductId"].ToString());
                var totalPrice = double.Parse(reader["TotalPrice"].ToString());
                var quantity = int.Parse(reader["Quantity"].ToString());

                var product = await _productRepository.Fetch(productId);

                return new OrderDetailModel
                {
                    OrderDetailId = orderDetailId,
                    Product = product,
                    TotalPrice = totalPrice,
                    Quantity = quantity
                };
            }
            return null;
        }

        public async Task<IEnumerable<OrderDetailModel>> FetchAll()
        {
            List<OrderDetailModel> orderDetails = new List<OrderDetailModel>();

            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM OrderDetailsTable";
            using SqlCommand command = new SqlCommand(queryString, connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderDetailId = int.Parse(reader["OrderDetailsId"].ToString());
                var productId = int.Parse(reader["ProductId"].ToString());
                var totalPrice = double.Parse(reader["TotalPrice"].ToString());
                var quantity = int.Parse(reader["Quantity"].ToString());

                var product = await _productRepository.Fetch(productId);
                orderDetails.Add(
                        new OrderDetailModel
                        {
                            OrderDetailId = orderDetailId,
                            Product = product,
                            TotalPrice = totalPrice,
                            Quantity = quantity
                        }
                    );
            }
            return orderDetails;
        }

        public async Task Update(OrderDetailModel data, int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "UPDATE OrderDetailsTable SET ProductId = @productId, TotalPrice = @totalPrice, Quantity = @quantity" +
                "WHERE OrderDetailsId = @orderDetailsId";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@productId", data.Product.ProductId);
            command.Parameters.AddWithValue("@totalPrice", data.TotalPrice);
            command.Parameters.AddWithValue("@quantity", data.Quantity);
            command.Parameters.AddWithValue("@orderDetailsId", id);
            await command.ExecuteNonQueryAsync();
        }
    }
}
