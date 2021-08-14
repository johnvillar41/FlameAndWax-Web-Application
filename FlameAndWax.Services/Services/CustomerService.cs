using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using FlameAndWax.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerReviewRepository _customerReviewRepository;
        public CustomerService(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            ICustomerRepository customerRepository,
            ICustomerReviewRepository customerReviewRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _customerRepository = customerRepository;
            _customerReviewRepository = customerReviewRepository;
        }
        public async Task<ServiceResult<Boolean?>> AddOrderTransaction(OrderModel newOrder)
        {
            if (newOrder == null)            
                return new ServiceResult<Boolean?>
                {
                    Data = false,
                    HasError = true,
                    ErrorContent = "OrderModel not defined!"
                };            

            await _orderRepository.Add(newOrder);
            /**
             * Get the Orders inside the order object then
             * loop through the orderDetail list property then adding 
             * orderDetail and subtracting the Quantity of products
             * inside the database
            **/
            var orderDetails = newOrder.OrderDetails;
            foreach (var orderDetail in orderDetails)
            {
                await _orderDetailRepository.Add(orderDetail);
                await _productRepository.ModifyNumberOfUnitsInOrder(orderDetail.Product.ProductId, orderDetail.Quantity);
            }

            return new ServiceResult<bool?>
            {
                Data = true,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<CustomerModel>> FetchAccountDetail(int customerId = 0)
        {
            if (customerId == 0)            
                return new ServiceResult<CustomerModel>
                {
                    Data = null,
                    HasError = true,
                    ErrorContent = "Customer Id not defined!"
                };            

            var customer = await _customerRepository.Fetch(customerId);
            return new ServiceResult<CustomerModel>
            {
                Data = customer,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<IEnumerable<ProductModel>>> FetchAllProducts()
        {
            var products = await _productRepository.FetchAll();
            return new ServiceResult<IEnumerable<ProductModel>>
            {
                Data = products,
                HasError = false,
                ErrorContent = null
            };             
        }

        public async Task<ServiceResult<IEnumerable<CustomerReviewModel>>> FetchCustomerReviewsInAProduct(int productId = 0)
        {
            if (productId == 0)            
                return new ServiceResult<IEnumerable<CustomerReviewModel>>
                {
                    Data = null,
                    HasError = true,
                    ErrorContent = "ProductId not defined!"
                };
            

            var customerReviews = await _customerReviewRepository.FetchReviewsOfAProduct(productId);
            return new ServiceResult<IEnumerable<CustomerReviewModel>>
            {
                Data = customerReviews,
                HasError = false,
                ErrorContent = null
            };   
        }

        public async Task<ServiceResult<IEnumerable<OrderDetailModel>>> FetchOrderDetails(int orderId = 0)
        {
            if (orderId == 0)            
                return new ServiceResult<IEnumerable<OrderDetailModel>>
                {
                    Data = null,
                    HasError = true,
                    ErrorContent = "Order Id not defined!"
                };
            

            var orderDetails = await _orderDetailRepository.FetchOrderDetails(orderId);
            return new ServiceResult<IEnumerable<OrderDetailModel>>
            {
                Data = orderDetails,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<IEnumerable<OrderModel>>> FetchOrders(int customerId = 0)
        {
            if (customerId == 0)            
                return new ServiceResult<IEnumerable<OrderModel>>
                {
                    Data = null,
                    HasError = true,
                    ErrorContent = "Customer Id not defined!"
                };
            

            var ordersFromCustomer = await _orderRepository.FetchOrdersFromCustomer(customerId);
            return new ServiceResult<IEnumerable<OrderModel>>
            {
                Data = ordersFromCustomer,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<ProductModel>> FetchProductDetail(int productId = 0)
        {
            if(productId == 0)            
                return new ServiceResult<ProductModel>
                {
                    Data = null,
                    HasError = true,
                    ErrorContent = "Product Id not defined!"
                };
            

            var productDetail = await _productRepository.Fetch(productId);
            return new ServiceResult<ProductModel>
            {
                Data = productDetail,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<Boolean?>> Login(CustomerModel loginCredentials)
        {
            if (loginCredentials.Username == null && loginCredentials.Password == null)            
                return new ServiceResult<bool?>
                {
                    Data = null,
                    HasError = true,
                    ErrorContent = "Login Credentials has no value"
                };
            

            return new ServiceResult<bool?>
            {
                Data = true,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<Boolean?>> ModifyAccountDetails(CustomerModel modifiedAccount, int customerId = 0)
        {
            if (modifiedAccount == null)            
                return new ServiceResult<bool?>
                {
                    Data = null,
                    HasError = true,
                    ErrorContent = "Modified Account details has no value!"
                };            
            if (customerId == 0)            
                return new ServiceResult<bool?>
                {
                    Data = null,
                    HasError = true,
                    ErrorContent = "Customer Id is not defined!"
                };
            
       
            await _customerRepository.Update(modifiedAccount, customerId);
            return new ServiceResult<bool?>
            {
                Data = true,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<Boolean?>> Register(CustomerModel registeredCredentials)
        {
            if (registeredCredentials == null)
                return new ServiceResult<bool?>
                {
                    Data = null,
                    HasError = true,
                    ErrorContent = "Registered Data has no value"
                };

            await _customerRepository.Add(registeredCredentials);
            return new ServiceResult<bool?>
            {
                Data = true,
                HasError = false,
                ErrorContent = null
            };
        }
    }
}
