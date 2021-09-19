﻿using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public async Task<int> Add(EmployeeModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("AddNewEmployee", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FirstName", Data.FirstName);
            command.Parameters.AddWithValue("@LastName", Data.LastName);
            command.Parameters.AddWithValue("@Email", Data.Email);
            command.Parameters.AddWithValue("@PhotoLink", Data.PhotoLink);
            command.Parameters.AddWithValue("@Bday", Data.BirthDate);
            command.Parameters.AddWithValue("@HireDate", Data.HireDate);
            command.Parameters.AddWithValue("@City", Data.City);
            command.Parameters.AddWithValue("@Username", Data.Username);
            command.Parameters.AddWithValue("@Password", Data.Password);
            command.Parameters.AddWithValue("@Status", Data.Status);

            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var primaryKey = int.Parse(reader["pk"].ToString());
                return primaryKey;
            }
            return -1;
        }

        public async Task Delete(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("DeleteEmployee", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<EmployeeModel> Fetch(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchEmployee", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new EmployeeModel
                {
                    EmployeeId = int.Parse(reader["EmployeeId"].ToString()),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Email = reader["Email"].ToString(),
                    PhotoLink = reader["PhotoLink"].ToString(),
                    BirthDate = DateTime.Parse(reader["BirthDaye"].ToString()),
                    HireDate = DateTime.Parse(reader["HireDate"].ToString()),
                    City = reader["City"].ToString(),
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                    Status = ServiceHelper.ConvertStringToEmployeeAccountStatus(reader["Status"].ToString())
                };
            }
            return null;
        }

        public async Task<IEnumerable<EmployeeModel>> FetchPaginatedResult(int pageNumber, int pageSize, string connectionString)
        {
            List<EmployeeModel> employees = new List<EmployeeModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchPaginatedResultEmployee", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                employees.Add(
                        new EmployeeModel
                        {
                            EmployeeId = int.Parse(reader["EmployeeId"].ToString()),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            PhotoLink = reader["PhotoLink"].ToString(),
                            BirthDate = DateTime.Parse(reader["BirthDaye"].ToString()),
                            HireDate = DateTime.Parse(reader["HireDate"].ToString()),
                            City = reader["City"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                            Status = ServiceHelper.ConvertStringToEmployeeAccountStatus(reader["Status"].ToString())
                        }
                    );
            }
            return employees;
        }

        public async Task<int> Login(EmployeeModel employee, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("LoginEmployee", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@username", employee.Username);
            command.Parameters.AddWithValue("@username", employee.Password);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return int.Parse(reader["EmployeeId"].ToString());
            }
            return -1;
        }

        public async Task UpdateEmployeeStatus(int employeeId, Constants.EmployeeAccountStatus accountStatus, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("UpdateEmployeeStatus", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@status", accountStatus.ToString());
            command.Parameters.AddWithValue("@id", employeeId);
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Updates the employee credentials excluding the PhotoLink property
        /// </summary>       
        public async Task Update(EmployeeModel data, int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("UpdateEmployee", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@firstName", data.FirstName);
            command.Parameters.AddWithValue("@lastName", data.LastName);
            command.Parameters.AddWithValue("@email", data.Email);
            command.Parameters.AddWithValue("@bday", data.BirthDate);
            command.Parameters.AddWithValue("@hireDate", data.HireDate);
            command.Parameters.AddWithValue("@city", data.City);
            command.Parameters.AddWithValue("@username", data.Username);
            command.Parameters.AddWithValue("@password", data.Password);
            command.Parameters.AddWithValue("@id", data.EmployeeId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateProfilePicture(string profileLink, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("UpdateEmployeeProfilePicture", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@firstName", profileLink);
            await command.ExecuteNonQueryAsync();
        }        
    }
}
