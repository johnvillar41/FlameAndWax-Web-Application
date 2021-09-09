using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface ICustomerService : ILoginBaseService<CustomerModel>
    {
        Task<ServiceResult<Boolean>> ModifyAccountDetails(CustomerModel modifiedAccount, int customerId, string connectionString);
        Task<ServiceResult<Boolean>> SendMessage(MessageModel newMessage, string connectionString);
        Task<ServiceResult<Boolean>> CheckIfCustomerHasOrderedAProduct(string customerUsername, int productId, string connectionString);
        Task<ServiceResult<Boolean>> AddCustomerReview(CustomerReviewModel customerReview, string connectionString);
        Task<ServiceResult<Boolean>> CheckoutOrder(OrderModel order, string usernameLoggedIn, string connectionString);

        Task<ServiceResult<double>> FetchProductPrice(int productId, string connectionString);
        Task<ServiceResult<int>> FetchTotalNumberOfProductsByCategory(Category? category, string connection);
        Task<ServiceResult<int>> FetchTotalNumberOfOrdersByOrderStatus(OrderStatus? orderStatus, string connection);
        Task<ServiceResult<int>> FetchTotalNumberOfReviews(int productId, string connectionString);

        Task<ServiceResult<CustomerModel>> FetchAccountDetail(int customerId, string connectionString);
        Task<ServiceResult<ProductModel>> FetchProductDetail(int productId, string connectionString);

        Task<ServiceResult<IEnumerable<ProductModel>>> FetchAllProducts(int pageNumber, int pageSize, string connectionString);
        Task<ServiceResult<IEnumerable<CustomerReviewModel>>> FetchCustomerReviewsInAProduct(int pageNumber, int pageSize, int productId, string connectionString);        
        Task<ServiceResult<IEnumerable<ProductModel>>> FetchProductByCategory(int pageNumber, int pageSize, Category category, string connectionString);
        Task<ServiceResult<IEnumerable<OrderModel>>> FetchOrdersByStatus(int pageNumber, int pageSize, int customerId, OrderStatus status, string connectionString);
    }
}


