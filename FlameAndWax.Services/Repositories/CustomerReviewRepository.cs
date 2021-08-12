using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Interfaces;
using FlameAndWax.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class CustomerReviewRepository : ICustomerReviewRepository
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;        
        public CustomerReviewRepository(ICustomerRepository customerRepository,IProductRepository productRepository)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }
        public async Task Add(CustomerReviewModel Data)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "INSERT INTO CustomerReviewTable(ProductId,CustomerId,ReviewScore,ReviewDetail)" +
                "VALUES(@ProductId,@CustomerId,@ReviewScore,@ReviewDetail)";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@ProductId", Data.Product.ProductId);
            command.Parameters.AddWithValue("@CustomerId", Data.Customer.CustomerId);
            command.Parameters.AddWithValue("@ReviewScore", Data.ReviewScore);
            command.Parameters.AddWithValue("@ReviewDetail", Data.ReviewDetail);
            await command.ExecuteNonQueryAsync();
        }

        public async Task Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "DELETE FROM CustomerReviewTable WHERE ReviewId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<CustomerReviewModel> Fetch(int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM CustomerReviewTable WHERE ReviewId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if(await reader.ReadAsync())
            {
                var productId = int.Parse(reader["ProductId"].ToString());
                var customerId = int.Parse(reader["CustomerId"].ToString());
                var reviewScore = int.Parse(reader["ReviewScore"].ToString());
                return new CustomerReviewModel
                {
                    Product = await _productRepository.Fetch(productId),
                    Customer = await _customerRepository.Fetch(customerId),
                    ReviewId = id,
                    ReviewScore = Helpers.ServiceHelper.BuildReviewScore(reviewScore),
                    ReviewDetail = reader["ReviewDetail"].ToString()
                };
            }
            return null;
        }

        public Task<IEnumerable<CustomerReviewModel>> FetchAll(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(CustomerReviewModel data, int id)
        {
            throw new NotImplementedException();
        }
    }
}
