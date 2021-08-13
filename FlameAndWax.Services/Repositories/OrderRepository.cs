﻿using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using FlameAndWax.Services.Helpers;
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
        public OrderRepository(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IOrderDetailRepository orderDetailRepository)
        {
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _orderDetailRepository = orderDetailRepository;
        }
        public async Task Add(OrderModel Data)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "INSERT INTO OrdersTable(CustomerId,EmployeeId,OrderDetailsId,DateNeeded,ModeOfPayment,Courier)" +
                "VALUES(@customerId,@employeeId,@orderDetailsId,@dateNeeded,@modeOfPayment,@courier)";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@customerId", Data.Customer.CustomerId);
            command.Parameters.AddWithValue("@employeeId", Data.Employee.EmployeeId);
            command.Parameters.AddWithValue("@orderDetailsId", Data.OrderDetails.OrderDetailId);
            command.Parameters.AddWithValue("@dateNeeded", Data.DateNeeded);
            command.Parameters.AddWithValue("@modeOfPayment", nameof(Data.ModeOfPayment));
            command.Parameters.AddWithValue("@courier", nameof(Data.Courier));
            await command.ExecuteNonQueryAsync();
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
                var orderDetailId = int.Parse(reader["OrderDetailsId"].ToString());

                var customer = await _customerRepository.Fetch(customerId);
                var employee = await _employeeRepository.Fetch(employeeId);
                var orderDetail = await _orderDetailRepository.Fetch(orderDetailId);

                var modeOfPayment = ServiceHelper.BuildModeOfPayment(reader["ModeOfPayment"].ToString());
                var courier = ServiceHelper.BuildCourier(reader["Courier"].ToString());
                return new OrderModel
                {
                    OrderId = orderId,
                    Customer = customer,
                    Employee = employee,
                    OrderDetails = orderDetail,
                    DateNeeded = DateTime.Parse(reader["DateNeeded"].ToString()),
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
                var orderDetailId = int.Parse(reader["OrderDetailsId"].ToString());

                var customer = await _customerRepository.Fetch(customerId);
                var employee = await _employeeRepository.Fetch(employeeId);
                var orderDetail = await _orderDetailRepository.Fetch(orderDetailId);

                var modeOfPayment = ServiceHelper.BuildModeOfPayment(reader["ModeOfPayment"].ToString());
                var courier = ServiceHelper.BuildCourier(reader["Courier"].ToString());

                orders.Add(
                        new OrderModel
                        {
                            OrderId = orderId,
                            Customer = customer,
                            Employee = employee,
                            OrderDetails = orderDetail,
                            DateNeeded = DateTime.Parse(reader["DateNeeded"].ToString()),
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
