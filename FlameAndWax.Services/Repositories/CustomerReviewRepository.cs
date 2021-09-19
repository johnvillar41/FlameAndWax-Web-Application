using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
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
        public CustomerReviewRepository(
            ICustomerRepository customerRepository,
            IProductRepository productRepository)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }
        public async Task<int> Add(CustomerReviewModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("AddCustomerReview", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ProductId", Data.Product.ProductId);
            command.Parameters.AddWithValue("@CustomerId", Data.Customer.CustomerId);
            command.Parameters.AddWithValue("@ReviewScore", Data.ReviewScore);
            command.Parameters.AddWithValue("@ReviewDetail", Data.ReviewDetail);
            command.Parameters.AddWithValue("@Date", DateTime.Now);

            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return int.Parse(reader["pk"].ToString());
            }
            return -1;
        }

        public async Task Delete(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("DeleteCustomerReview", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<CustomerReviewModel> Fetch(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchCustomerReview", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var productId = int.Parse(reader["ProductId"].ToString());
                var customerId = int.Parse(reader["CustomerId"].ToString());
                var reviewScore = int.Parse(reader["ReviewScore"].ToString());

                return new CustomerReviewModel
                {
                    Product = await _productRepository.Fetch(productId, connectionString),
                    Customer = await _customerRepository.Fetch(customerId, connectionString),
                    ReviewId = id,
                    ReviewScore = Helpers.ServiceHelper.BuildReviewScore(reviewScore),
                    ReviewDetail = reader["ReviewDetail"].ToString(),
                    Date = DateTime.Parse(reader["Date"].ToString())
                };
            }
            return null;
        }

        public async Task<IEnumerable<CustomerReviewModel>> FetchPaginatedResult(int pageNumber, int pageSize, string connectionString)
        {
            List<CustomerReviewModel> customerReviews = new List<CustomerReviewModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchPaginatedResultCustomerReview", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var productId = int.Parse(reader["ProductId"].ToString());
                var customerId = int.Parse(reader["CustomerId"].ToString());
                var reviewScore = int.Parse(reader["ReviewScore"].ToString());

                customerReviews.Add(
                        new CustomerReviewModel
                        {
                            ReviewId = int.Parse(reader["ReviewId"].ToString()),
                            Product = await _productRepository.Fetch(productId, connectionString),
                            Customer = await _customerRepository.Fetch(customerId, connectionString),
                            ReviewScore = Helpers.ServiceHelper.BuildReviewScore(reviewScore),
                            ReviewDetail = reader["ReviewDetail"].ToString(),
                            Date = DateTime.Parse(reader["Date"].ToString())
                        }
                    );
            }
            return customerReviews;
        }

        public async Task<IEnumerable<CustomerReviewModel>> FetchPaginatedReviewsOfAProduct(int pageNumber, int pageSize, int productId, string connectionString)
        {
            List<CustomerReviewModel> customerReviews = new List<CustomerReviewModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchPaginatedReviewsOfAProduct", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@productId", productId);
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var customerId = int.Parse(reader["CustomerId"].ToString());
                var reviewScore = int.Parse(reader["ReviewScore"].ToString());

                customerReviews.Add(
                        new CustomerReviewModel
                        {
                            ReviewId = int.Parse(reader["ReviewId"].ToString()),
                            Product = await _productRepository.Fetch(productId, connectionString),
                            Customer = await _customerRepository.Fetch(customerId, connectionString),
                            ReviewScore = Helpers.ServiceHelper.BuildReviewScore(reviewScore),
                            ReviewDetail = reader["ReviewDetail"].ToString(),
                            Date = DateTime.Parse(reader["Date"].ToString())
                        }
                    );
            }
            return customerReviews;
        }

        public async Task<IEnumerable<CustomerReviewModel>> FetchTopComments(string connectionString)
        {
            List<CustomerReviewModel> topCustomerReviews = new List<CustomerReviewModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchTopComments", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                topCustomerReviews.Add(
                    new CustomerReviewModel
                    {
                        ReviewId = int.Parse(reader["ReviewId"].ToString()),
                        Product = await _productRepository.Fetch(int.Parse(reader["ProductId"].ToString()), connectionString),
                        Customer = await _customerRepository.Fetch(int.Parse(reader["CustomerId"].ToString()), connectionString),
                        ReviewScore = ServiceHelper.BuildReviewScore(int.Parse(reader["ReviewScore"].ToString())),
                        ReviewDetail = reader["ReviewDetail"].ToString(),
                        Date = DateTime.Parse(reader["Date"].ToString())
                    }
                );
            }
            return topCustomerReviews;
        }

        public async Task<int> FetchTotalNumberOfReviewsOnAProduct(int productId, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchTotalNumberOfReviewsOnAProduct", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@productId", productId);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return int.Parse(reader["total"].ToString());
            }
            return -1;
        }

        public async Task Update(CustomerReviewModel data, int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("UpdateCustomerReview", connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }
    }
}
