using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface ICustomerService : ILoginBaseService<CustomerModel>
    {
        Task<ServiceResult<Boolean>> AddOrderTransaction(OrderModel newOrder,string connectionString);
        Task<ServiceResult<Boolean>> ModifyAccountDetails(CustomerModel modifiedAccount, int customerId,string connectionString);
        Task<ServiceResult<Boolean>> SendMessage(MessageModel newMessage, string connectionString);
        Task<ServiceResult<Boolean>> CheckIfCustomerHasOrderedAProduct(string customerUsername,int productId, string connectionString);
        Task<ServiceResult<Boolean>> AddCustomerReview(CustomerReviewModel customerReview, string connectionString);       
        Task<ServiceResult<Boolean>> CheckoutOrder(OrderModel order,string usernameLoggedIn, string connectionString);
   
        Task<ServiceResult<double>> FetchProductPrice(int productId, string connectionString);        

        Task<ServiceResult<CustomerModel>> FetchAccountDetail(int customerId, string connectionString);       
        Task<ServiceResult<ProductModel>> FetchProductDetail(int productId, string connectionString);

        Task<ServiceResult<IEnumerable<OrderDetailModel>>> FetchOrderDetails(int orderId, string connectionString);
        Task<ServiceResult<IEnumerable<OrderModel>>> FetchOrders(int customerId, string connectionString);
        Task<ServiceResult<IEnumerable<ProductModel>>> FetchAllProducts(int pageNumber, int pageSize, string connectionString);
        Task<ServiceResult<IEnumerable<CustomerReviewModel>>> FetchCustomerReviewsInAProduct(int productId, string connectionString);
        Task<ServiceResult<IEnumerable<ProductModel>>> FetchNewArrivedProducts(string connectionString);
        Task<ServiceResult<IEnumerable<ProductModel>>> FetchProductByCategory(Constants.Category category, string connectionString);
    }
}
