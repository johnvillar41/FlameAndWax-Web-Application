using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        public OrderRepository(
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository,
            IOrderDetailRepository orderDetailRepository)
        {
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _orderDetailRepository = orderDetailRepository;
        }
        public async Task<int> AddAsync(OrderModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("AddNewOrder", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@customerId", Data.Customer.CustomerId);
            command.Parameters.AddWithValue("@dateOrdered", DateTime.UtcNow);
            command.Parameters.AddWithValue("@totalCost", Data.TotalCost);
            command.Parameters.AddWithValue("@modeOfPayment", Data.ModeOfPayment.ToString());
            command.Parameters.AddWithValue("@courier", Data.Courier.ToString());
            command.Parameters.AddWithValue("@status", Constants.OrderStatus.Pending.ToString());
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
            using SqlCommand command = new SqlCommand("DeleteOrder", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<OrderModel> FetchAsync(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchOrder", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var orderId = int.Parse(reader["OrderId"].ToString());
                var customerId = int.Parse(reader["CustomerId"].ToString());
                var employeeId = int.Parse(reader["EmployeeId"].ToString());
                var totalCost = double.Parse(reader["TotalCost"].ToString());

                var customer = await _customerRepository.FetchAsync(customerId, connectionString);
                var employee = await _employeeRepository.FetchAsync(employeeId, connectionString);
                var orderDetails = await _orderDetailRepository.FetchOrderDetailsAsync(orderId, connectionString);

                var modeOfPayment = ServiceHelper.BuildModeOfPayment(reader["ModeOfPayment"].ToString());
                var courier = ServiceHelper.BuildCourier(reader["Courier"].ToString());
                var status = ServiceHelper.ConvertStringtoOrderStatus(reader["Status"].ToString());
                return new OrderModel
                {
                    OrderId = orderId,
                    Customer = customer,
                    Employee = employee,
                    OrderDetails = orderDetails,
                    DateOrdered = DateTime.Parse(reader["DateOrdered"].ToString()),
                    TotalCost = totalCost,
                    ModeOfPayment = modeOfPayment,
                    Courier = courier,
                    Status = status
                };
            }
            return null;
        }

        public async Task<IEnumerable<OrderModel>> FetchPaginatedResultAsync(int pageNumber, int pageSize, string connectionString)
        {
            List<OrderModel> orders = new List<OrderModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchPaginatedResultOrder", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderId = int.Parse(reader["OrderId"].ToString());
                var customerId = int.Parse(reader["CustomerId"].ToString());
                var employeeId = int.Parse(reader["EmployeeId"].ToString());
                var totalCost = double.Parse(reader["TotalCost"].ToString());

                var customer = await _customerRepository.FetchAsync(customerId, connectionString);
                var employee = await _employeeRepository.FetchAsync(employeeId, connectionString);
                var orderDetails = await _orderDetailRepository.FetchOrderDetailsAsync(orderId, connectionString);

                var modeOfPayment = ServiceHelper.BuildModeOfPayment(reader["ModeOfPayment"].ToString());
                var courier = ServiceHelper.BuildCourier(reader["Courier"].ToString());
                var status = ServiceHelper.ConvertStringtoOrderStatus(reader["Status"].ToString());

                orders.Add(
                        new OrderModel
                        {
                            OrderId = orderId,
                            Customer = customer,
                            Employee = employee,
                            OrderDetails = orderDetails,
                            DateOrdered = DateTime.Parse(reader["DateOrdered"].ToString()),
                            TotalCost = totalCost,
                            ModeOfPayment = modeOfPayment,
                            Courier = courier,
                            Status = status
                        }
                    );
            }
            return orders;
        }

        public async Task<IEnumerable<OrderModel>> FetchPaginatedCategorizedOrdersAsync(int pageNumber, int pageSize, int customerId, Constants.OrderStatus orderStatus, string connectionString)
        {
            List<OrderModel> orders = new List<OrderModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchPaginatedCategorizedOrders", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@customerId", customerId);
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.AddWithValue("@status", orderStatus.ToString());
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderId = int.Parse(reader["OrderId"].ToString());

                var employeeId = -1;
                EmployeeModel employee = new EmployeeModel();
                if (!reader.IsDBNull(2))
                {
                    employeeId = int.Parse(reader["EmployeeId"].ToString());
                    employee = await _employeeRepository.FetchAsync(employeeId, connectionString);
                }

                var totalCost = double.Parse(reader["TotalCost"].ToString());

                var customer = await _customerRepository.FetchAsync(customerId, connectionString);
                var orderDetails = await _orderDetailRepository.FetchOrderDetailsAsync(orderId, connectionString);

                var modeOfPayment = ServiceHelper.BuildModeOfPayment(reader["ModeOfPayment"].ToString());
                var courier = ServiceHelper.BuildCourier(reader["Courier"].ToString());
                var status = ServiceHelper.ConvertStringtoOrderStatus(reader["Status"].ToString());

                orders.Add(
                        new OrderModel
                        {
                            OrderId = orderId,
                            Customer = customer,
                            Employee = employee,
                            OrderDetails = orderDetails,
                            DateOrdered = DateTime.Parse(reader["DateOrdered"].ToString()),
                            TotalCost = totalCost,
                            ModeOfPayment = modeOfPayment,
                            Courier = courier,
                            Status = status
                        }
                    );
            }
            return orders;
        }

        public Task UpdateAsync(OrderModel data, int id, string connectionString)
        {
            throw new NotImplementedException();
        }

        public async Task<int> FetchTotalNumberOfOrdersAsync(Constants.OrderStatus? orderStatus, string connectionString, int customerId)
        {
            var totalNumberOfProducts = 0;
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var storedProcedure = "";
            if (orderStatus == null)
                storedProcedure = "FetchTotalNumberOfOrdersWithoutOrderStatus";
            else
                storedProcedure = "FetchTotalNumberOfOrdersWithOrderStatus";

            using SqlCommand command = new SqlCommand(storedProcedure, connection);
            if (orderStatus != null)
                command.Parameters.AddWithValue("@orderStatus", orderStatus.ToString());

            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@customerId", customerId);
            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                totalNumberOfProducts = int.Parse(reader["total"].ToString());
            }
            return totalNumberOfProducts;
        }

        public async Task<IEnumerable<OrderModel>> FetchAllOrdersAsync(int pageNumber, int pageSize, string connectionString)
        {
            List<OrderModel> orders = new List<OrderModel>();
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM OrdersTable ORDER BY OrderId OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderId = int.Parse(reader["OrderId"].ToString());
                var customerId = int.Parse(reader["CustomerId"].ToString());
                var employeeId = -1;
                EmployeeModel employee = new EmployeeModel();
                if (!reader.IsDBNull(2))
                {
                    employeeId = int.Parse(reader["EmployeeId"].ToString());
                    employee = await _employeeRepository.FetchAsync(employeeId, connectionString);
                }

                var totalCost = double.Parse(reader["TotalCost"].ToString());

                var customer = await _customerRepository.FetchAsync(customerId, connectionString);
                var orderDetails = await _orderDetailRepository.FetchOrderDetailsAsync(orderId, connectionString);

                var modeOfPayment = ServiceHelper.BuildModeOfPayment(reader["ModeOfPayment"].ToString());
                var courier = ServiceHelper.BuildCourier(reader["Courier"].ToString());
                var status = ServiceHelper.ConvertStringtoOrderStatus(reader["Status"].ToString());

                orders.Add(new OrderModel
                {
                    OrderId = orderId,
                    Customer = customer,
                    Employee = employee,
                    OrderDetails = orderDetails,
                    DateOrdered = DateTime.Parse(reader["DateOrdered"].ToString()),
                    TotalCost = totalCost,
                    ModeOfPayment = modeOfPayment,
                    Courier = courier,
                    Status = status
                });
            }
            return orders;
        }
    }
}
