using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using FlameAndWax.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

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

        public async Task<ServiceResult<bool>> AddCustomerReview(CustomerReviewModel customerReview, string connectionString)
        {
            if (customerReview == null) return ServiceHelper.BuildServiceResult<bool>(false, true, "Customer review has no data");

            try
            {
                var customerReviewResult = await _customerReviewRepository.Add(customerReview, connectionString);
                if (customerReviewResult == -1) return ServiceHelper.BuildServiceResult<bool>(false, true, "Error adding customer review");
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<bool>(false, true, e.Message); }

            return ServiceHelper.BuildServiceResult<bool>(true, false, null);
        }

        public async Task<ServiceResult<bool>> AddOrderTransaction(OrderModel newOrder, string connectionString)
        {
            if (newOrder == null) return ServiceHelper.BuildServiceResult<bool>(false, true, "OrderModel not defined!");

            try
            {
                var orderRepositoryResult = await _orderRepository.Add(newOrder, connectionString);
                if (orderRepositoryResult == -1) return ServiceHelper.BuildServiceResult<bool>(false, true, "Failed to add order item");

                var orderDetails = newOrder.OrderDetails;
                foreach (var orderDetail in orderDetails)
                {
                    var orderDetailRepositoryResult = await _orderDetailRepository.Add(orderDetail, connectionString);
                    if (orderDetailRepositoryResult == -1) return ServiceHelper.BuildServiceResult<bool>(false, true, "Failed to add order details");

                    await _productRepository.ModifyNumberOfUnitsInOrder(orderDetail.Product.ProductId, orderDetail.Quantity, connectionString);
                }
                return ServiceHelper.BuildServiceResult<bool>(true, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<bool>(false, true, e.Message); }
        }

        public async Task<ServiceResult<bool>> CheckIfCustomerHasOrderedAProduct(string customerUsername, int productId, string connectionString)
        {
            try
            {
                var isSuccess = await _previouslyOrderedProductsRepository.HasCustomerOrderedAProduct(productId, customerUsername, connectionString);
                return ServiceHelper.BuildServiceResult<bool>(isSuccess, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<bool>(false, true, e.Message); }
        }

        public async Task<ServiceResult<CustomerModel>> FetchAccountDetail(int customerId, string connectionString)
        {
            try
            {
                var customer = await _customerRepository.Fetch(customerId, connectionString);
                if (customer == null) return ServiceHelper.BuildServiceResult<CustomerModel>(new CustomerModel(), true, "Customer not found!");
                return ServiceHelper.BuildServiceResult<CustomerModel>(customer, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<CustomerModel>(null, false, e.Message); }
        }

        public async Task<ServiceResult<IEnumerable<ProductModel>>> FetchAllProducts(int pageNumber, int pageSize, string connectionString)
        {
            try
            {
                var products = await _productRepository.FetchPaginatedResult(pageNumber, pageSize, connectionString);
                return ServiceHelper.BuildServiceResult<IEnumerable<ProductModel>>(products, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<IEnumerable<ProductModel>>(null, true, e.Message); }
        }

        public async Task<ServiceResult<IEnumerable<CustomerReviewModel>>> FetchCustomerReviewsInAProduct(int productId, string connectionString)
        {
            try
            {
                var customerReviews = await _customerReviewRepository.FetchReviewsOfAProduct(productId, connectionString);
                return ServiceHelper.BuildServiceResult<IEnumerable<CustomerReviewModel>>(customerReviews, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<IEnumerable<CustomerReviewModel>>(null, true, e.Message); }
        }

        public async Task<ServiceResult<IEnumerable<ProductModel>>> FetchNewArrivedProducts(string connectionString)
        {
            try
            {
                var newArrivals = await _productRepository.FetchNewArrivedProducts(connectionString);
                return ServiceHelper.BuildServiceResult<IEnumerable<ProductModel>>(newArrivals, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<IEnumerable<ProductModel>>(null, true, e.Message); }
        }

        public async Task<ServiceResult<IEnumerable<OrderDetailModel>>> FetchOrderDetails(int orderId = 0, string connectionString = "")
        {
            try
            {
                if (orderId == 0)
                    return ServiceHelper.BuildServiceResult<IEnumerable<OrderDetailModel>>(null, true, "Order Id not defined!");
                var orderDetails = await _orderDetailRepository.FetchOrderDetails(orderId, connectionString);
                return ServiceHelper.BuildServiceResult<IEnumerable<OrderDetailModel>>(orderDetails, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<IEnumerable<OrderDetailModel>>(null, true, e.Message); }
        }

        public async Task<ServiceResult<IEnumerable<OrderModel>>> FetchOrders(int pageNumber, int pageSize, int customerId = 0, string connectionString = "")
        {
            try
            {
                if (customerId == 0)
                    return ServiceHelper.BuildServiceResult<IEnumerable<OrderModel>>(null, true, "Customer Id not defined!");
                var ordersFromCustomer = await _orderRepository.FetchPaginatedOrdersFromCustomer(pageNumber, pageSize, customerId, connectionString);
                return ServiceHelper.BuildServiceResult<IEnumerable<OrderModel>>(ordersFromCustomer, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<IEnumerable<OrderModel>>(null, true, e.Message); }
        }

        public async Task<ServiceResult<IEnumerable<OrderModel>>> FetchOrdersByStatus(int customerId, Constants.OrderStatus status, string connectionString)
        {
            try
            {
                var categorizedOrders = await _orderRepository.FetchCategorizedOrders(customerId, status, connectionString);
                return ServiceHelper.BuildServiceResult<IEnumerable<OrderModel>>(categorizedOrders, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<IEnumerable<OrderModel>>(null, true, e.Message); }
        }

        public async Task<ServiceResult<IEnumerable<ProductModel>>> FetchProductByCategory(int pageNumber, int pageSize, Category category, string connectionString)
        {
            try
            {
                var categorizedProducts = await _productRepository.FetchPaginatedCategorizedProducts(pageNumber, pageSize, category, connectionString);
                return ServiceHelper.BuildServiceResult<IEnumerable<ProductModel>>(categorizedProducts, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<IEnumerable<ProductModel>>(null, true, e.Message); }
        }

        public async Task<ServiceResult<ProductModel>> FetchProductDetail(int productId, string connectionString)
        {
            try
            {
                var productDetail = await _productRepository.Fetch(productId, connectionString);
                return ServiceHelper.BuildServiceResult<ProductModel>(productDetail, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<ProductModel>(null, true, e.Message); }
        }

        public async Task<ServiceResult<double>> FetchProductPrice(int productId, string connectionString)
        {
            try
            {
                var productPrice = await _productRepository.Fetch(productId, connectionString);
                return ServiceHelper.BuildServiceResult<double>(productPrice.ProductPrice, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<double>(-0.1, true, e.Message); }
        }

        public async Task<ServiceResult<bool>> CheckoutOrder(OrderModel order, string usernameLoggedIn, string connectionString)
        {
            try
            {
                var primaryKey = await _orderRepository.Add(order, connectionString);
                if (primaryKey == -1) return ServiceHelper.BuildServiceResult<bool>(false, true, "Error Inserting Order");

                foreach (var orderDetail in order.OrderDetails)
                {
                    orderDetail.OrderId = primaryKey;
                    var primaryKeyOrderDetail = await _orderDetailRepository.Add(orderDetail, connectionString);
                    if (primaryKeyOrderDetail == -1)
                        return ServiceHelper.BuildServiceResult<bool>(false, true, "Error Inserting OrderDetail");

                    var previouslyOrderedModel = new PreviouslyOrderedProductModel
                    {
                        ProductId = orderDetail.Product.ProductId,
                        CustomerUsername = usernameLoggedIn
                    };

                    var result = await _previouslyOrderedProductsRepository.AddPreviouslyOrderedProducts(previouslyOrderedModel, connectionString);
                    if (result == -1)
                        return ServiceHelper.BuildServiceResult<bool>(false, true, "Error Adding Previous Orders");

                    await _productRepository.UpdateAddUnitsOnOrder(orderDetail.Product.ProductId, orderDetail.Quantity, connectionString);
                }


                return ServiceHelper.BuildServiceResult<bool>(true, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<bool>(false, true, e.Message); }
        }

        public async Task<ServiceResult<int>> Login(CustomerModel loginCredentials, string connectionString)
        {
            if (loginCredentials == null)
                return ServiceHelper.BuildServiceResult<int>(-1, true, "Login Credentials has no value");
            try
            {
                var isLoggedIn = await _customerRepository.LoginCustomerAccount(loginCredentials, connectionString);
                if (isLoggedIn > -1)
                    return ServiceHelper.BuildServiceResult<int>(isLoggedIn, false, null);

                else
                    return ServiceHelper.BuildServiceResult<int>(-1, true, "Invalid User");
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                if (e.Message.Equals("The parameterized query '(@username nvarchar(4000),@password nvarchar(4000))SELECT Custom' expects the parameter '@username', which was not supplied."))
                    return ServiceHelper.BuildServiceResult<int>(-1, true, "Please enter username and password!");
                if (e.Message.Equals($"The parameterized query '(@username nvarchar(4000),@password nvarchar({(loginCredentials.Password == null ? 0 : loginCredentials.Password.Length)}))SELECT CustomerI' expects the parameter '@username', which was not supplied."))
                    return ServiceHelper.BuildServiceResult<int>(-1, true, "Please enter a valid username!");
                if (e.Message.Equals($"The parameterized query '(@username nvarchar({(loginCredentials.Username == null ? 0 : loginCredentials.Username.Length)}),@password nvarchar(4000))SELECT CustomerI' expects the parameter '@password', which was not supplied."))
                    return ServiceHelper.BuildServiceResult<int>(-1, true, "Please enter a valid password!");
                return ServiceHelper.BuildServiceResult<int>(-1, true, e.Message);
            }
        }

        public async Task<ServiceResult<bool>> ModifyAccountDetails(CustomerModel modifiedAccount, int customerId, string connectionString)
        {
            if (modifiedAccount == null)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Modified Account details has no value!");
            try
            {
                await _customerRepository.Update(modifiedAccount, customerId, connectionString);
                return ServiceHelper.BuildServiceResult<bool>(true, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<bool>(false, true, e.Message); }
        }

        public async Task<ServiceResult<bool>> Register(CustomerModel registeredCredentials, string connectionString)
        {
            if (registeredCredentials == null)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Registered Data has no value");
            try
            {
                var customerRespositoryResult = await _customerRepository.Add(registeredCredentials, connectionString);
                if (customerRespositoryResult == -1) return ServiceHelper.BuildServiceResult<bool>(false, true, "Error Adding Customer");

                return ServiceHelper.BuildServiceResult<bool>(true, false, null);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                if (e.Message.Equals($"Cannot insert duplicate key row in object 'dbo.CustomerTable' with unique index 'IX_CustomerTable'. The duplicate key value is ({registeredCredentials.Username}).\r\nThe statement has been terminated."))
                    return ServiceHelper.BuildServiceResult<bool>(false, true, "Duplicate Username! Please try a different username");

                return ServiceHelper.BuildServiceResult<bool>(false, true, "Please Fill all the missing fields!");
            }
        }

        public async Task<ServiceResult<bool>> SendMessage(MessageModel newMessage, string connectionString)
        {
            if (newMessage == null)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Empty Message!");
            try
            {
                var messageRepositoryResult = await _messageRepository.Add(newMessage, connectionString);
                if (messageRepositoryResult == -1) return ServiceHelper.BuildServiceResult<bool>(false, true, "Error Adding Message");

                return ServiceHelper.BuildServiceResult<bool>(true, false, null);
            }
            catch (System.Data.SqlClient.SqlException) { return ServiceHelper.BuildServiceResult<bool>(false, true, "Please fill up all missing fields!"); }
        }

        public async Task<ServiceResult<int>> FetchTotalNumberOfProductsByCategory(Category? category, string connection)
        {
            try
            {
                var totalNumberOfProducts = await _productRepository.FetchTotalNumberOfProducts(category, connection);
                return ServiceHelper.BuildServiceResult<int>(totalNumberOfProducts, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<int>(-1, true, e.Message); }
        }

        public async Task<ServiceResult<int>> FetchTotalNumberOfOrdersByOrderStatus(OrderStatus? orderStatus, string connection)
        {
            try
            {
                var totalNumberOfProducts = await _orderRepository.FetchTotalNumberOfOrders(orderStatus, connection);
                return ServiceHelper.BuildServiceResult<int>(totalNumberOfProducts, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<int>(-1, true, e.Message); }
        }
    }
}
