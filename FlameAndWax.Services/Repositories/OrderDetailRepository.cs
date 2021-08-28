using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using FlameAndWax.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly IProductRepository _productRepository;
        //private readonly IOrderRepository _orderRepository;
        public OrderDetailRepository(IProductRepository productRepository)//, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            //_orderRepository = orderRepository;
        }
        public async Task<int> Add(OrderDetailModel Data)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "INSERT INTO OrderDetailsTable(OrderId,ProductId,TotalPrice,Quantity,Status)" +
                "VALUES(@OrderId,@ProductId,@TotalPrice,@Quantity,@Status);" +
                "SELECT SCOPE_IDENTITY() as fk;";
            using SqlCommand command = new SqlCommand(queryString, connection);
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
                var orderId = int.Parse(reader["OrderId"].ToString());
                var productId = int.Parse(reader["ProductId"].ToString());
                var totalPrice = double.Parse(reader["TotalPrice"].ToString());
                var quantity = int.Parse(reader["Quantity"].ToString());
                var status = ServiceHelper.ConvertStringtoOrderDetailStatus(reader["Status"].ToString());

                var product = await _productRepository.Fetch(productId);               
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

        public async Task<IEnumerable<OrderDetailModel>> FetchPaginatedResult(int pageNumber, int pageSize)
        {
            List<OrderDetailModel> orderDetails = new List<OrderDetailModel>();

            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM OrderDetailsTable ORDER by OrderDetailsId OFFSET (@PageNumber - 1) * @PageSize ROWS " +
                "FETCH NEXT @PageSize ROWS ONLY";
            using SqlCommand command = new SqlCommand(queryString, connection);
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
                var status = ServiceHelper.ConvertStringtoOrderDetailStatus(reader["Status"].ToString());

                var product = await _productRepository.Fetch(productId);                
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

        public async Task<IEnumerable<OrderDetailModel>> FetchOrderDetails(int orderId)
        {
            List<OrderDetailModel> orderDetails = new List<OrderDetailModel>();

            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM OrderDetailsTable WHERE OrderId = @orderId";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderDetailId = int.Parse(reader["OrderDetailsId"].ToString());
                var productId = int.Parse(reader["ProductId"].ToString());
                var totalPrice = double.Parse(reader["TotalPrice"].ToString());
                var quantity = int.Parse(reader["Quantity"].ToString());
                var status = ServiceHelper.ConvertStringtoOrderDetailStatus(reader["Status"].ToString());

                var product = await _productRepository.Fetch(productId);
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

        public async Task Update(OrderDetailModel data, int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "UPDATE OrderDetailsTable SET ProductId = @productId, TotalPrice = @totalPrice, Quantity = @quantity, Status = @status" +
                "WHERE OrderDetailsId = @orderDetailsId";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@productId", data.Product.ProductId);
            command.Parameters.AddWithValue("@totalPrice", data.TotalPrice);
            command.Parameters.AddWithValue("@quantity", data.Quantity);
            command.Parameters.AddWithValue("@orderDetailsId", id);
            command.Parameters.AddWithValue("@Status", data.Status.ToString());
            await command.ExecuteNonQueryAsync();
        }
    }
}
