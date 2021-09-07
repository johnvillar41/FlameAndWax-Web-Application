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
        public async Task<int> Add(OrderModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "INSERT INTO OrdersTable(CustomerId,DateOrdered,TotalCost,ModeOfPayment,Courier,Status)" +
                "VALUES(@customerId,@dateOrdered,@totalCost,@modeOfPayment,@courier,@status);" +
                "SELECT SCOPE_IDENTITY() as fk;";
            using SqlCommand command = new SqlCommand(queryString, connection);
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

        public async Task Delete(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "DELETE FROM OrdersTable WHERE OrderId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<OrderModel> Fetch(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM OrdersTable WHERE OrderId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var orderId = int.Parse(reader["OrderId"].ToString());
                var customerId = int.Parse(reader["CustomerId"].ToString());
                var employeeId = int.Parse(reader["EmployeeId"].ToString());
                var totalCost = double.Parse(reader["TotalCost"].ToString());

                var customer = await _customerRepository.Fetch(customerId, connectionString);
                var employee = await _employeeRepository.Fetch(employeeId, connectionString);
                var orderDetails = await _orderDetailRepository.FetchOrderDetails(orderId, connectionString);

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

        public async Task<IEnumerable<OrderModel>> FetchPaginatedResult(int pageNumber, int pageSize, string connectionString)
        {
            List<OrderModel> orders = new List<OrderModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM OrdersTable ORDER by OrderId OFFSET (@PageNumber - 1) * @PageSize ROWS " +
                "FETCH NEXT @PageSize ROWS ONLY";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderId = int.Parse(reader["OrderId"].ToString());
                var customerId = int.Parse(reader["CustomerId"].ToString());
                var employeeId = int.Parse(reader["EmployeeId"].ToString());
                var totalCost = double.Parse(reader["TotalCost"].ToString());

                var customer = await _customerRepository.Fetch(customerId, connectionString);
                var employee = await _employeeRepository.Fetch(employeeId, connectionString);
                var orderDetails = await _orderDetailRepository.FetchOrderDetails(orderId, connectionString);

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

        public async Task<IEnumerable<OrderModel>> FetchPaginatedOrdersFromCustomer(int pageNumber, int pageSize, int customerId, string connectionString)
        {
            List<OrderModel> orders = new List<OrderModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM OrdersTable  WHERE CustomerId = @customerId ORDER by OrderId OFFSET (@PageNumber - 1) * @PageSize ROWS " +
                "FETCH NEXT @PageSize ROWS ONLY";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@customerId", customerId);
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderId = int.Parse(reader["OrderId"].ToString());

                var employeeId = -1;
                EmployeeModel employee = null;
                if (!reader.IsDBNull(2))
                {
                    employeeId = int.Parse(reader["EmployeeId"].ToString());
                    employee = await _employeeRepository.Fetch(employeeId, connectionString);
                }

                var totalCost = double.Parse(reader["TotalCost"].ToString());

                var customer = await _customerRepository.Fetch(customerId, connectionString);
                var orderDetails = await _orderDetailRepository.FetchOrderDetails(orderId, connectionString);

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

        public async Task<IEnumerable<OrderModel>> FetchPaginatedCategorizedOrders(int pageNumber,int pageSize, int customerId, Constants.OrderStatus orderStatus, string connectionString)
        {
            List<OrderModel> orders = new List<OrderModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM OrdersTable  WHERE CustomerId = @customerId AND Status = @status ORDER by OrderId OFFSET (@PageNumber - 1) * @PageSize ROWS " +
                "FETCH NEXT @PageSize ROWS ONLY";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@customerId", customerId);
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.AddWithValue("@status", orderStatus.ToString());
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderId = int.Parse(reader["OrderId"].ToString());

                var employeeId = -1;
                EmployeeModel employee = null;
                if (!reader.IsDBNull(2))
                {
                    employeeId = int.Parse(reader["EmployeeId"].ToString());
                    employee = await _employeeRepository.Fetch(employeeId, connectionString);
                }

                var totalCost = double.Parse(reader["TotalCost"].ToString());

                var customer = await _customerRepository.Fetch(customerId, connectionString);
                var orderDetails = await _orderDetailRepository.FetchOrderDetails(orderId, connectionString);

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

        public Task Update(OrderModel data, int id, string connectionString)
        {
            throw new NotImplementedException();
        }

        public async Task<int> FetchTotalNumberOfOrders(Constants.OrderStatus? orderStatus, string connectionString)
        {
            var totalNumberOfProducts = 0;
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "";
            if (orderStatus == null)
                queryString = "SELECT COUNT(OrderId) as total FROM OrdersTable";
            else
                queryString = "SELECT COUNT(OrderId) as total FROM OrdersTable WHERE Status = @orderStatus";

            using SqlCommand command = new SqlCommand(queryString, connection);
            if (orderStatus != null)
                command.Parameters.AddWithValue("@orderStatus", orderStatus.ToString());

            using SqlDataReader reader = await command.ExecuteReaderAsync();
           
            if (await reader.ReadAsync())
            {
                totalNumberOfProducts = int.Parse(reader["total"].ToString());
            }
            return totalNumberOfProducts;
        }
    }
}
