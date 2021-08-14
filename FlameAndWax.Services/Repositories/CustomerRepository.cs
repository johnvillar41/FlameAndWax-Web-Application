using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public async Task Add(CustomerModel Data)
        {
            using SqlConnection connection = new SqlConnection(Constants.Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "INSERT INTO CustomerTable(CustomerName,ContactNumber,Username,Password,ProfilePictureLink)" +
                "VALUES(@name,@number,@username,@password,@link)";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@name", Data.CustomerName);
            command.Parameters.AddWithValue("@number", Data.ContactNumber);
            command.Parameters.AddWithValue("@username", Data.Username);
            command.Parameters.AddWithValue("@password", Data.Password);
            command.Parameters.AddWithValue("@link", Data.ProfilePictureLink);
            await command.ExecuteNonQueryAsync();
        }

        public async Task Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "DELETE FROM CustomerTable WHERE CustomerId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<CustomerModel> Fetch(int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM CustomerTable WHERE CustomerId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new CustomerModel
                {
                    CustomerId = int.Parse(reader["CustomerId"].ToString()),
                    CustomerName = reader["CustomerName"].ToString(),
                    ContactNumber = reader["ContactNumber"].ToString(),
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                    ProfilePictureLink = reader["ProfilePictureLink"].ToString()
                };
            }
            return null;
        }

        public async Task<IEnumerable<CustomerModel>> FetchAll()
        {
            List<CustomerModel> customers = new List<CustomerModel>();

            using SqlConnection connection = new SqlConnection(Constants.Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM CustomerTable";
            using SqlCommand command = new SqlCommand(queryString, connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                customers.Add(
                    new CustomerModel
                    {
                        CustomerId = int.Parse(reader["CustomerId"].ToString()),
                        CustomerName = reader["CustomerName"].ToString(),
                        ContactNumber = reader["ContactNumber"].ToString(),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        ProfilePictureLink = reader["ProfilePictureLink"].ToString()
                    }
                );
            }
            return customers;
        }

        public async Task<bool> LoginCustomerAccount(CustomerModel loginCustomer)
        {
            using SqlConnection connection = new SqlConnection(Constants.Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM CustomerTable WHERE Username = @username AND Password = @password";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddRange(new string[] { loginCustomer.Username, loginCustomer.Password });
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if(await reader.ReadAsync())
                return true;
            return false;
        }

        public async Task Update(CustomerModel data, int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "UPDATE CustomerTable SET CustomerName = @name, ContactNumber = @number, Username = @username, " +
                "Password = @password, ProfilePictureLink = @link WHERE CustomerId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", data.CustomerName);
            command.Parameters.AddWithValue("@number", data.ContactNumber);
            command.Parameters.AddWithValue("@username", data.Username);
            command.Parameters.AddWithValue("@password", data.Password);
            command.Parameters.AddWithValue("@link", data.ProfilePictureLink);
            await command.ExecuteNonQueryAsync();
        }
    }
}
