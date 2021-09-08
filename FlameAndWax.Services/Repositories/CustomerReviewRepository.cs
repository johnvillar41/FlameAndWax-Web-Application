using FlameAndWax.Data.Models;
using FlameAndWax.Services.Repositories.Interfaces;
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
            var queryString = "INSERT INTO CustomerReviewTable(ProductId,CustomerId,ReviewScore,ReviewDetail)" +
                "VALUES(@ProductId,@CustomerId,@ReviewScore,@ReviewDetail)";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@ProductId", Data.Product.ProductId);
            command.Parameters.AddWithValue("@CustomerId", Data.Customer.CustomerId);
            command.Parameters.AddWithValue("@ReviewScore", Data.ReviewScore);
            command.Parameters.AddWithValue("@ReviewDetail", Data.ReviewDetail);
            await command.ExecuteNonQueryAsync();
            return Data.ReviewId;
        }

        public async Task Delete(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "DELETE FROM CustomerReviewTable WHERE ReviewId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<CustomerReviewModel> Fetch(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM CustomerReviewTable WHERE ReviewId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
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
                    ReviewDetail = reader["ReviewDetail"].ToString()
                };
            }
            return null;
        }

        public async Task<IEnumerable<CustomerReviewModel>> FetchPaginatedResult(int pageNumber, int pageSize, string connectionString)
        {
            List<CustomerReviewModel> customerReviews = new List<CustomerReviewModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM CustomerReviewTable ORDER BY ReviewID OFFSET (@PageNumber - 1) * @PageSize ROWS " +
                "FETCH NEXT @PageSize ROWS ONLY";
            using SqlCommand command = new SqlCommand(queryString, connection);
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
                            ReviewDetail = reader["ReviewDetail"].ToString()
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
            var queryString = "SELECT * FROM CustomerReviewTable WHERE ProductId = @productId ORDER by ReviewId DESC OFFSET (@PageNumber - 1) * @PageSize ROWS " +
            "FETCH NEXT @PageSize ROWS ONLY";
            using SqlCommand command = new SqlCommand(queryString, connection);
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
                            ReviewDetail = reader["ReviewDetail"].ToString()
                        }
                    );
            }
            return customerReviews;
        }

        public async Task Update(CustomerReviewModel data, int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var queryString = "UPDATE CustomerReviewTable SET ReviewScore = @reviewScore, ReviewDetail = @detail" +
                "WHERE ReviewId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }
    }
}
