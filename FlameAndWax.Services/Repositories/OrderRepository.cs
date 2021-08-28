using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using FlameAndWax.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        public OrderRepository(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IOrderDetailRepository orderDetailRepository)
        {
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _orderDetailRepository = orderDetailRepository;
        }
        public async Task<int> Add(OrderModel Data)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "INSERT INTO OrdersTable(CustomerId,DateOrdered,TotalCost,ModeOfPayment,Courier)" +
                "VALUES(@customerId,@dateOrdered,@totalCost,@modeOfPayment,@courier);" +
                "SELECT SCOPE_IDENTITY() as fk;";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@customerId", Data.Customer.CustomerId);           
            command.Parameters.AddWithValue("@dateOrdered", DateTime.UtcNow);
            command.Parameters.AddWithValue("@totalCost", Data.TotalCost);
            command.Parameters.AddWithValue("@modeOfPayment", Data.ModeOfPayment.ToString());
            command.Parameters.AddWithValue("@courier", Data.Courier.ToString());
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if(await reader.ReadAsync())
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
            var queryString = "DELETE FROM OrdersTable WHERE OrderId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<OrderModel> Fetch(int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
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

                var customer = await _customerRepository.Fetch(customerId);
                var employee = await _employeeRepository.Fetch(employeeId);
                var orderDetails = await _orderDetailRepository.FetchOrderDetails(orderId);

                var modeOfPayment = ServiceHelper.BuildModeOfPayment(reader["ModeOfPayment"].ToString());
                var courier = ServiceHelper.BuildCourier(reader["Courier"].ToString());
                return new OrderModel
                {
                    OrderId = orderId,
                    Customer = customer,
                    Employee = employee,
                    OrderDetails = orderDetails,
                    DateOrdered = DateTime.Parse(reader["DateOrdered"].ToString()),
                    TotalCost = totalCost,
                    ModeOfPayment = modeOfPayment,
                    Courier = courier
                };
            }
            return null;
        }

        public async Task<IEnumerable<OrderModel>> FetchAll()
        {
            List<OrderModel> orders = new List<OrderModel>();

            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM OrdersTable";
            using SqlCommand command = new SqlCommand(queryString, connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderId = int.Parse(reader["OrderId"].ToString());
                var customerId = int.Parse(reader["CustomerId"].ToString());
                var employeeId = int.Parse(reader["EmployeeId"].ToString());                
                var totalCost = double.Parse(reader["TotalCost"].ToString());

                var customer = await _customerRepository.Fetch(customerId);
                var employee = await _employeeRepository.Fetch(employeeId);
                var orderDetails = await _orderDetailRepository.FetchOrderDetails(orderId);

                var modeOfPayment = ServiceHelper.BuildModeOfPayment(reader["ModeOfPayment"].ToString());
                var courier = ServiceHelper.BuildCourier(reader["Courier"].ToString());

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
                            Courier = courier
                        }
                    );
            }
            return orders;
        }

        public async Task<IEnumerable<OrderModel>> FetchOrdersFromCustomer(int customerId)
        {
            List<OrderModel> orders = new List<OrderModel>();
            
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM OrdersTable WHERE CustomerId = @customerId ORDER BY DateOrdered DESC";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@customerId", customerId);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderId = int.Parse(reader["OrderId"].ToString());   
                
                var employeeId = -1;
                EmployeeModel employee = null;
                if (!reader.IsDBNull(2))
                {
                    employeeId = int.Parse(reader["EmployeeId"].ToString());
                    employee = await _employeeRepository.Fetch(employeeId);
                }

                var totalCost = double.Parse(reader["TotalCost"].ToString());

                var customer = await _customerRepository.Fetch(customerId);                
                var orderDetails = await _orderDetailRepository.FetchOrderDetails(orderId);

                var modeOfPayment = ServiceHelper.BuildModeOfPayment(reader["ModeOfPayment"].ToString());
                var courier = ServiceHelper.BuildCourier(reader["Courier"].ToString());

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
                            Courier = courier
                        }
                    );
            }
            return orders;
        }

        public async Task Update(OrderModel data, int id)
        {
            throw new NotImplementedException();
        }

       
    }
}
