using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
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
        private readonly IMessageRepository _messageRepository;
        private readonly IPreviouslyOrderedProductsRepository _previouslyOrderedProductsRepository;
        public CustomerService(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            ICustomerRepository customerRepository,
            ICustomerReviewRepository customerReviewRepository,
            IMessageRepository messageRepository,
            IPreviouslyOrderedProductsRepository previouslyOrderedProductsRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _customerRepository = customerRepository;
            _customerReviewRepository = customerReviewRepository;
            _messageRepository = messageRepository;
            _previouslyOrderedProductsRepository = previouslyOrderedProductsRepository;
        }

        public async Task<ServiceResult<bool>> AddCustomerReview(CustomerReviewModel customerReview)
        {
            await _customerReviewRepository.Add(customerReview);
            return ServiceHelper.BuildServiceResult<Boolean>(true, false, null);
        }

        public async Task<ServiceResult<bool>> AddOrderTransaction(OrderModel newOrder)
        {
            if (newOrder == null)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "OrderModel not defined!");

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
            return ServiceHelper.BuildServiceResult<bool>(true, false, null);
        }

        public async Task<ServiceResult<bool>> CheckIfCustomerHasOrderedAProduct(string customerUsername, int productId)
        {
            var isSuccess = await _previouslyOrderedProductsRepository.HasCustomerOrderedAProduct(productId, customerUsername);
            return ServiceHelper.BuildServiceResult<bool>(isSuccess, false, null);
        }

        public async Task<ServiceResult<CustomerModel>> FetchAccountDetail(int customerId = 0)
        {
            if (customerId == 0)
                return ServiceHelper.BuildServiceResult<CustomerModel>(null, true, "Customer Id not defined!");

            var customer = await _customerRepository.Fetch(customerId);
            return ServiceHelper.BuildServiceResult<CustomerModel>(customer, false, null);
        }

        public async Task<ServiceResult<IEnumerable<ProductModel>>> FetchAllProducts()
        {
            var products = await _productRepository.FetchAll();
            return ServiceHelper.BuildServiceResult<IEnumerable<ProductModel>>(products, false, null);
        }

        public async Task<ServiceResult<IEnumerable<CustomerReviewModel>>> FetchCustomerReviewsInAProduct(int productId = 0)
        {
            if (productId == 0)
                return ServiceHelper.BuildServiceResult<IEnumerable<CustomerReviewModel>>(null, true, "ProductId not defined!");


            var customerReviews = await _customerReviewRepository.FetchReviewsOfAProduct(productId);
            return ServiceHelper.BuildServiceResult<IEnumerable<CustomerReviewModel>>(customerReviews, false, null);
        }

        public async Task<ServiceResult<IEnumerable<ProductModel>>> FetchNewArrivedProducts()
        {
            var newArrivals = await _productRepository.FetchNewArrivedProducts();
            return new ServiceResult<IEnumerable<ProductModel>>
            {
                Result = newArrivals,
                HasError = false,
                ErrorContent = null
            };
        }

        public async Task<ServiceResult<IEnumerable<OrderDetailModel>>> FetchOrderDetails(int orderId = 0)
        {
            if (orderId == 0)
                return ServiceHelper.BuildServiceResult<IEnumerable<OrderDetailModel>>(null, true, "Order Id not defined!");


            var orderDetails = await _orderDetailRepository.FetchOrderDetails(orderId);
            return ServiceHelper.BuildServiceResult<IEnumerable<OrderDetailModel>>(orderDetails, false, null);
        }

        public async Task<ServiceResult<IEnumerable<OrderModel>>> FetchOrders(int customerId = 0)
        {
            if (customerId == 0)
                return ServiceHelper.BuildServiceResult<IEnumerable<OrderModel>>(null, true, "Customer Id not defined!");


            var ordersFromCustomer = await _orderRepository.FetchOrdersFromCustomer(customerId);
            return ServiceHelper.BuildServiceResult<IEnumerable<OrderModel>>(ordersFromCustomer, false, null);
        }

        public async Task<ServiceResult<IEnumerable<ProductModel>>> FetchProductByCategory(Constants.Category category)
        {
            var categorizedProducts = await _productRepository.FetchCategorizedProducts(category);
            return ServiceHelper.BuildServiceResult<IEnumerable<ProductModel>>(categorizedProducts, false, null);
        }

        public async Task<ServiceResult<ProductModel>> FetchProductDetail(int productId = 0)
        {
            if (productId == 0)
                return ServiceHelper.BuildServiceResult<ProductModel>(null, true, "Product Id not defined!");


            var productDetail = await _productRepository.Fetch(productId);
            return ServiceHelper.BuildServiceResult<ProductModel>(productDetail, false, null);
        }

        public async Task<ServiceResult<int>> InsertNewOrder(OrderModel order)
        {
            var primaryKey = await _orderRepository.Add(order);
            if (primaryKey != 1)
                foreach (var orderDetail in order.OrderDetails)
                {
                    orderDetail.Order = new OrderModel { OrderId = primaryKey };
                    var primaryKeyOrderDetail = await _orderDetailRepository.Add(orderDetail);
                    if (primaryKeyOrderDetail == -1)
                        return ServiceHelper.BuildServiceResult<int>(primaryKeyOrderDetail, true, "Error Inserting OrderDetail");
                }
            else
                return ServiceHelper.BuildServiceResult<int>(primaryKey, true, "Error Inserting Order");
            return ServiceHelper.BuildServiceResult<int>(primaryKey, false, null);
        }

        public async Task<ServiceResult<int>> Login(CustomerModel loginCredentials)
        {
            if (loginCredentials.Username == null && loginCredentials.Password == null)
                return ServiceHelper.BuildServiceResult<int>(-1, true, "Login Credentials has no value");

            var isLoggedIn = await _customerRepository.LoginCustomerAccount(loginCredentials);
            if (isLoggedIn > -1)
                return ServiceHelper.BuildServiceResult<int>(isLoggedIn, false, null);

            else
                return ServiceHelper.BuildServiceResult<int>(-1, true, "Invalid User");
        }

        public async Task<ServiceResult<bool>> ModifyAccountDetails(CustomerModel modifiedAccount, int customerId = 0)
        {
            if (modifiedAccount == null)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Modified Account details has no value!");
            if (customerId == 0)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Customer Id is not defined!");


            await _customerRepository.Update(modifiedAccount, customerId);
            return ServiceHelper.BuildServiceResult<bool>(true, false, null);
        }

        public async Task<ServiceResult<bool>> Register(CustomerModel registeredCredentials)
        {
            if (registeredCredentials == null)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Registered Data has no value");

            await _customerRepository.Add(registeredCredentials);
            return ServiceHelper.BuildServiceResult<bool>(true, false, null);
        }

        public async Task<ServiceResult<bool>> SendMessage(MessageModel newMessage)
        {
            if (newMessage == null)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Empty Message!");
            await _messageRepository.Add(newMessage);
            return ServiceHelper.BuildServiceResult<bool>(true, false, null);
        }
    }
}
