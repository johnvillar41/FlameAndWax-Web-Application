using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface ICustomerService : ILoginBaseService<CustomerModel>
    {
        Task AddOrderTransaction(OrderModel newOrder);
        Task ModifyAccountDetails(CustomerModel modifiedAccount, int customerId);

        Task<CustomerModel> FetchAccountDetail(int customerId);       
        Task<ProductModel> FetchProductDetail(int productId);

        Task<IEnumerable<OrderDetailModel>> FetchOrderDetails(int orderId);
        Task<IEnumerable<OrderModel>> FetchOrders(int customerId);
        Task<IEnumerable<ProductModel>> FetchAllProducts();
        Task<IEnumerable<CustomerReviewModel>> FetchCustomerReviewsInAProduct(int productId);
    }
}
