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
        public CustomerService(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            ICustomerRepository customerRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _customerRepository = customerRepository;
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
                //await _productRepository.ModifyNumberOfStocks(orderDetail.Product.ProductId, orderDetail.Quantity);
            }
        }

        public async Task<CustomerModel> FetchAccountDetail(int customerId = 0)
        {
            if (customerId == 0) return new CustomerModel();

            var customer = await _customerRepository.Fetch(customerId);
            return customer;
        }

        public Task<IEnumerable<ProductModel>> FetchAllProducts()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<CustomerReviewModel>> FetchCustomerReviewsInAProduct(int productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<OrderDetailModel>> FetchOrderDetails(int orderId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<OrderModel>> FetchOrders(int customerId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProductModel> FetchProductDetail(int productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Login(CustomerModel loginCredentials)
        {
            throw new System.NotImplementedException();
        }

        public Task ModifyAccountDetails(CustomerModel modifiedAccount, int customerId)
        {
            throw new System.NotImplementedException();
        }

        public Task Register(CustomerModel registeredCredentials)
        {
            throw new System.NotImplementedException();
        }
    }
}
