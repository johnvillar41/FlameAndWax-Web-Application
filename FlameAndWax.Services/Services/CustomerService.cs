using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using FlameAndWax.Services.Services.Interfaces;
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
        public async Task AddOrderTransaction(OrderModel newOrder)
        {
            if (newOrder == null) return;
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
        }

        public async Task<CustomerModel> FetchAccountDetail(int customerId = 0)
        {
            if (customerId == 0) return new CustomerModel();

            var customer = await _customerRepository.Fetch(customerId);
            return customer;
        }

        public async Task<IEnumerable<ProductModel>> FetchAllProducts()
        {
            return await _productRepository.FetchAll();
        }

        public async Task<IEnumerable<CustomerReviewModel>> FetchCustomerReviewsInAProduct(int productId = 0)
        {
            if (productId == 0) return new List<CustomerReviewModel>();
            return await _customerReviewRepository.FetchReviewsOfAProduct(productId);             
        }

        public async Task<IEnumerable<OrderDetailModel>> FetchOrderDetails(int orderId = 0)
        {
            if (orderId == 0) return new List<OrderDetailModel>();
            return await _orderDetailRepository.FetchOrderDetails(orderId);
        }

        public async Task<IEnumerable<OrderModel>> FetchOrders(int customerId = 0)
        {
            if (customerId == 0) return new List<OrderModel>();
            return await _orderRepository.FetchOrdersFromCustomer(customerId);
        }

        public async Task<ProductModel> FetchProductDetail(int productId)
        {
            return await _productRepository.Fetch(productId);
        }

        public async Task<bool> Login(CustomerModel loginCredentials)
        {
            if (loginCredentials == null) return false;
            return await _customerRepository.LoginCustomerAccount(loginCredentials);
        }

        public async Task ModifyAccountDetails(CustomerModel modifiedAccount, int customerId = 0)
        {
            if (modifiedAccount == null) return;
            if (customerId == 0) return;
            await _customerRepository.Update(modifiedAccount, customerId);
        }

        public async Task Register(CustomerModel registeredCredentials)
        {
            if (registeredCredentials == null) return;
            await _customerRepository.Add(registeredCredentials);
        }
    }
}
