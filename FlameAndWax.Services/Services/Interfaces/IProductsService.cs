using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using FlameAndWax.Services.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface IProductsService
    {
        
        Task<ServiceResult<Boolean>> CheckIfCustomerHasOrderedAProductAsync(string customerUsername, int productId, string connectionString);
        Task<ServiceResult<Boolean>> AddCustomerReviewAsync(CustomerReviewModel customerReview, string connectionString);           
        Task<ServiceResult<ProductModel>> FetchProductDetailAsync(int productId, string connectionString);
        Task<PagedServiceResult<IEnumerable<ProductModel>>> FetchAllProductsAsync(int pageNumber, int pageSize, string connectionString);
        Task<PagedServiceResult<IEnumerable<CustomerReviewModel>>> FetchCustomerReviewsInAProductAsync(int pageNumber, int pageSize, int productId, string connectionString);        
        Task<PagedServiceResult<IEnumerable<ProductModel>>> FetchProductByCategoryAsync(int pageNumber, int pageSize, Category category, string connectionString);
    }
}


