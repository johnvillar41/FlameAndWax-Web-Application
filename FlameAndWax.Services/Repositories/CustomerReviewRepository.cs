using Dapper;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class CustomerReviewRepository : ICustomerReviewRepository
    {
        public async Task<int> AddAsync(CustomerReviewModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var result = await connection.QueryFirstAsync<int>("AddCustomerReview",
                new
                {
                    Data.Product.ProductId,
                    Data.Customer.CustomerId,
                    Data.ReviewScore,
                    Data.ReviewDetail,
                    Date = DateTime.Now
                }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task DeleteAsync(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DeleteCustomerReview",
                new
                {
                    Id = id
                }, commandType: CommandType.StoredProcedure);
        }

        public async Task<CustomerReviewModel> FetchAsync(int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var customerReview = await connection.QueryFirstOrDefaultAsync<CustomerReviewModel>("FetchCustomerReview",
                new
                {
                    Id = id
                }, commandType: CommandType.StoredProcedure);
            return customerReview;
        }

        public async Task<IEnumerable<CustomerReviewModel>> FetchPaginatedResultAsync(int pageNumber, int pageSize, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var customerReviews = await connection.QueryAsync<CustomerReviewModel>("FetchPaginatedResultCustomerReview",
                new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }, commandType: CommandType.StoredProcedure);
            return customerReviews;
        }

        public async Task<IEnumerable<CustomerReviewModel>> FetchPaginatedReviewsOfAProductAsync(int pageNumber, int pageSize, int productId, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var customerReviews = await connection.QueryAsync<CustomerReviewModel>("FetchPaginatedReviewsOfAProduct",
                new
                {
                    ProductId = productId,
                    PageSize = pageSize,
                    pageNumber = pageNumber
                }, commandType: CommandType.StoredProcedure);
            return customerReviews;
        }

        public async Task<IEnumerable<CustomerReviewModel>> FetchTopCommentsAsync(string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var customerReviews = await connection
                .QueryAsync<CustomerReviewModel, ProductModel, CustomerModel, ShippingAddressModel, ProductGalleryModel, CustomerReviewModel>("FetchTopComments",
               (customerReview, product, customer, shippingAddress, productGallery) =>
               {
                   customerReview.Customer = customer;
                   customerReview.Product = product;
                   customerReview.Customer.Addresses = new List<ShippingAddressModel>();
                   ((List<ShippingAddressModel>)customerReview.Customer.Addresses).Add(new ShippingAddressModel()
                   {
                       ShippingAddressId = shippingAddress.ShippingAddressId,
                       CustomerId = customer.CustomerId,
                       Address = shippingAddress.Address,
                       PostalCode = shippingAddress.PostalCode,
                       City = shippingAddress.City,
                       Region = shippingAddress.Region,
                       Country = shippingAddress.Country
                   });
                   customerReview.Product.ProductGallery = new List<ProductGalleryModel>();
                   ((List<ProductGalleryModel>)customerReview.Product.ProductGallery).Add(new ProductGalleryModel()
                   {
                       ProductGalleryId = productGallery.ProductGalleryId,
                       ProductId = productGallery.ProductId,
                       ProductPhotoLink = productGallery.ProductPhotoLink
                   });
                   return customerReview;
               }, commandType: CommandType.StoredProcedure, splitOn: "ProductId, CustomerId, ShippingAddressId, ProductGalleryId");
            return customerReviews;
        }

        public async Task<int> FetchTotalNumberOfReviewsOnAProductAsync(int productId, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var result = await connection.ExecuteScalarAsync<int>("FetchTotalNumberOfReviewsOnAProduct",
                new
                {
                    productId
                }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task UpdateAsync(CustomerReviewModel data, int id, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("UpdateCustomerReview",
                new { id, CustomerReview = data.ReviewDetail }, commandType: CommandType.StoredProcedure);
        }
    }
}
