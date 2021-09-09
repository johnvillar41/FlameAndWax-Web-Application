using System;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using FlameAndWax.Services.Services.Interfaces;
using FlameAndWax.Services.Services.Models;

namespace FlameAndWax.Services.Services
{
    public class CartService : ICartService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IPreviouslyOrderedProductsRepository _previouslyOrderedProductsRepository;
        private readonly IProductRepository _productRepository;
        public CartService(
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IPreviouslyOrderedProductsRepository previouslyOrderedProductsRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _previouslyOrderedProductsRepository = previouslyOrderedProductsRepository;
            _productRepository = productRepository;
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
        public async Task<ServiceResult<double>> FetchProductPrice(int productId, string connectionString)
        {
            try
            {
                var productPrice = await _productRepository.Fetch(productId, connectionString);
                return ServiceHelper.BuildServiceResult<double>(productPrice.ProductPrice, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<double>(-0.1, true, e.Message); }
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
    }

}