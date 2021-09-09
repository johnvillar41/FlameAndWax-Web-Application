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

        public async Task<ServiceResult<IEnumerable<CustomerReviewModel>>> FetchCustomerReviewsInAProduct(int pageNumber, int pageSize, int productId, string connectionString)
        {
            try
            {
                var customerReviews = await _customerReviewRepository.FetchPaginatedReviewsOfAProduct(pageNumber,pageSize, productId, connectionString);
                return ServiceHelper.BuildServiceResult<IEnumerable<CustomerReviewModel>>(customerReviews, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<IEnumerable<CustomerReviewModel>>(null, true, e.Message); }
        }             

        public async Task<ServiceResult<IEnumerable<OrderModel>>> FetchOrdersByStatus(int pageNumber,int pageSize,int customerId, Constants.OrderStatus status, string connectionString)
        {
            try
            {
                var categorizedOrders = await _orderRepository.FetchPaginatedCategorizedOrders(pageNumber,pageSize,customerId, status, connectionString);
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

        public async Task<ServiceResult<int>> FetchTotalNumberOfReviews(int productId, string connectionString)
        {
            try
            {
                var totalNumberOfReviews = await _customerReviewRepository.FetchTotalNumberOfReviewsOnAProduct(productId, connectionString);
                return ServiceHelper.BuildServiceResult<int>(totalNumberOfReviews, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<int>(-1, true, e.Message); }
        }

        public Task<ServiceResult<ProductModel>> FetchProductDetail(int productId, string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}
