﻿using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface ICustomerService : ILoginBaseService<CustomerModel>
    {
        Task<ServiceResult<Boolean>> AddOrderTransaction(OrderModel newOrder);
        Task<ServiceResult<Boolean>> ModifyAccountDetails(CustomerModel modifiedAccount, int customerId);
        Task<ServiceResult<Boolean>> SendMessage(MessageModel newMessage);

        Task<ServiceResult<CustomerModel>> FetchAccountDetail(int customerId);       
        Task<ServiceResult<ProductModel>> FetchProductDetail(int productId);

        Task<ServiceResult<IEnumerable<OrderDetailModel>>> FetchOrderDetails(int orderId);
        Task<ServiceResult<IEnumerable<OrderModel>>> FetchOrders(int customerId);
        Task<ServiceResult<IEnumerable<ProductModel>>> FetchAllProducts();
        Task<ServiceResult<IEnumerable<CustomerReviewModel>>> FetchCustomerReviewsInAProduct(int productId);
        Task<ServiceResult<IEnumerable<ProductModel>>> FetchNewArrivedProducts();
    }
}
