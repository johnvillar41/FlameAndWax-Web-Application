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
        
        Task<ServiceResult<Boolean>> CheckIfCustomerHasOrderedAProduct(string customerUsername, int productId, string connectionString);
        Task<ServiceResult<Boolean>> AddCustomerReview(CustomerReviewModel customerReview, string connectionString);            
        Task<ServiceResult<int>> FetchTotalNumberOfReviews(int productId, string connectionString);       
        Task<ServiceResult<ProductModel>> FetchProductDetail(int productId, string connectionString);
        Task<PagedServiceResult<IEnumerable<ProductModel>>> FetchAllProducts(int pageNumber, int pageSize, string connectionString);
        Task<ServiceResult<IEnumerable<CustomerReviewModel>>> FetchCustomerReviewsInAProduct(int pageNumber, int pageSize, int productId, string connectionString);        
        Task<PagedServiceResult<IEnumerable<ProductModel>>> FetchProductByCategory(int pageNumber, int pageSize, Category category, string connectionString);
        
    }
}


