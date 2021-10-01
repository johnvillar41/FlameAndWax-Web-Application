using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
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
        public async Task<int> AddAsync(OrderDetailModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("AddNewOrderDetail", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@OrderId", Data.OrderId);
            command.Parameters.AddWithValue("@ProductId", Data.Product.ProductId);
            command.Parameters.AddWithValue("@TotalPrice", Data.TotalPrice);
            command.Parameters.AddWithValue("@Quantity", Data.Quantity);
            command.Parameters.AddWithValue("@Status", Data.Status.ToString());
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var primaryKey = int.Parse(reader["fk"].ToString());
                return primaryKey;
            }
            return -1;
        }

        public async Task DeleteAsync(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("DeleteOrderDetail", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            await command.ExecuteNonQueryAsync();
        }

        public async Task<OrderDetailModel> FetchAsync(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchAllOrderDetails", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var orderDetailId = int.Parse(reader["OrderDetailsId"].ToString());
                var orderId = int.Parse(reader["OrderId"].ToString());
                var productId = int.Parse(reader["ProductId"].ToString());
                var totalPrice = double.Parse(reader["TotalPrice"].ToString());
                var quantity = int.Parse(reader["Quantity"].ToString());
                var status = ServiceHelper.ConvertStringtoOrderStatus(reader["Status"].ToString());

                var product = await _productRepository.FetchAsync(productId, connectionString);
                return new OrderDetailModel
                {
                    OrderDetailsId = orderDetailId,
                    OrderId = orderId,
                    Product = product,
                    TotalPrice = totalPrice,
                    Quantity = quantity,
                    Status = status
                };
            }
            return null;
        }

        public async Task<IEnumerable<OrderDetailModel>> FetchPaginatedResultAsync(int pageNumber, int pageSize, string connectionString)
        {
            List<OrderDetailModel> orderDetails = new List<OrderDetailModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchPaginatedResultOrderDetail", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderDetailId = int.Parse(reader["OrderDetailsId"].ToString());
                var orderId = int.Parse(reader["OrderId"].ToString());
                var productId = int.Parse(reader["ProductId"].ToString());
                var totalPrice = double.Parse(reader["TotalPrice"].ToString());
                var quantity = int.Parse(reader["Quantity"].ToString());
                var status = ServiceHelper.ConvertStringtoOrderStatus(reader["Status"].ToString());

                var product = await _productRepository.FetchAsync(productId, connectionString);
                orderDetails.Add(
                        new OrderDetailModel
                        {
                            OrderDetailsId = orderDetailId,
                            OrderId = orderId,
                            Product = product,
                            TotalPrice = totalPrice,
                            Quantity = quantity,
                            Status = status
                        }
                    );
            }
            return orderDetails;
        }

        public async Task<IEnumerable<OrderDetailModel>> FetchOrderDetailsAsync(int orderId, string connectionString)
        {
            List<OrderDetailModel> orderDetails = new List<OrderDetailModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchOrderDetailsGivenOrderId", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@orderId", orderId);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderDetailId = int.Parse(reader["OrderDetailsId"].ToString());
                var productId = int.Parse(reader["ProductId"].ToString());
                var totalPrice = double.Parse(reader["TotalPrice"].ToString());
                var quantity = int.Parse(reader["Quantity"].ToString());
                var status = ServiceHelper.ConvertStringtoOrderStatus(reader["Status"].ToString());

                var product = await _productRepository.FetchAsync(productId, connectionString);
                orderDetails.Add(
                        new OrderDetailModel
                        {
                            OrderDetailsId = orderDetailId,
                            OrderId = orderId,
                            Product = product,
                            TotalPrice = totalPrice,
                            Quantity = quantity,
                            Status = status
                        }
                    );
            }
            return orderDetails;
        }

        public async Task UpdateAsync(OrderDetailModel data, int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("UpdateOrderDetail", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@productId", data.Product.ProductId);
            command.Parameters.AddWithValue("@totalPrice", data.TotalPrice);
            command.Parameters.AddWithValue("@quantity", data.Quantity);
            command.Parameters.AddWithValue("@orderDetailsId", id);
            command.Parameters.AddWithValue("@Status", data.Status.ToString());
            await command.ExecuteNonQueryAsync();
        }
    }
}
