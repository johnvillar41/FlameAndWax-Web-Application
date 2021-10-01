using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using FlameAndWax.Services.Services.Interfaces;
using FlameAndWax.Services.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Services.Services
{
    public class ProductService : IProductsService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerReviewRepository _customerReviewRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IPreviouslyOrderedProductsRepository _previouslyOrderedProductsRepository;
        public ProductService(
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

        public async Task<ServiceResult<bool>> AddCustomerReviewAsync(CustomerReviewModel customerReview, string connectionString)
        {
            if (customerReview == null) return ServiceHelper.BuildServiceResult<bool>(false, true, "Customer review has no data");

            try
            {
                var customerReviewResult = await _customerReviewRepository.AddAsync(customerReview, connectionString);
                if (customerReviewResult == -1) return ServiceHelper.BuildServiceResult<bool>(false, true, "Error adding customer review");
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<bool>(false, true, e.Message); }

            return ServiceHelper.BuildServiceResult<bool>(true, false, null);
        }

        public async Task<ServiceResult<bool>> CheckIfCustomerHasOrderedAProductAsync(string customerUsername, int productId, string connectionString)
        {
            try
            {
                var isSuccess = await _previouslyOrderedProductsRepository.HasCustomerOrderedAProductAsync(productId, customerUsername, connectionString);
                return ServiceHelper.BuildServiceResult<bool>(isSuccess, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<bool>(false, true, e.Message); }
        }

        public async Task<PagedServiceResult<IEnumerable<ProductModel>>> FetchAllProductsAsync(int pageNumber, int pageSize, string connectionString)
        {
            try
            {
                var products = await _productRepository.FetchPaginatedResultAsync(pageNumber, pageSize, connectionString);
                return ServiceHelper.BuildPagedResult<IEnumerable<ProductModel>>(
                    new ServiceResult<IEnumerable<ProductModel>>
                    {
                        Result = products,
                        HasError = false,
                        ErrorContent = null
                    },
                    pageNumber,
                    await _productRepository.FetchTotalNumberOfProductsAsync(null, connectionString)
                );
            }
            catch (Exception e)
            {
                return ServiceHelper.BuildPagedResult<IEnumerable<ProductModel>>(
                  new ServiceResult<IEnumerable<ProductModel>>
                  {
                      Result = null,
                      HasError = true,
                      ErrorContent = e.Message
                  },
                  pageNumber,
                  -1
              );
            }
        }

        public async Task<PagedServiceResult<IEnumerable<CustomerReviewModel>>> FetchCustomerReviewsInAProductAsync(int pageNumber, int pageSize, int productId, string connectionString)
        {
            try
            {
                var customerReviews = await _customerReviewRepository.FetchPaginatedReviewsOfAProductAsync(pageNumber, pageSize, productId, connectionString);
                return ServiceHelper.BuildPagedResult<IEnumerable<CustomerReviewModel>>(
                    new ServiceResult<IEnumerable<CustomerReviewModel>>
                    {
                        Result = customerReviews,
                        HasError = false,
                        ErrorContent = null
                    },
                    pageNumber,
                    await _customerReviewRepository.FetchTotalNumberOfReviewsOnAProductAsync(productId, connectionString)
                );
            }
            catch (Exception e)
            {
                return ServiceHelper.BuildPagedResult<IEnumerable<CustomerReviewModel>>(
                    new ServiceResult<IEnumerable<CustomerReviewModel>>
                    {
                        Result = null,
                        HasError = true,
                        ErrorContent = e.Message
                    },
                    pageNumber,
                    -1
                );
            }
        }

        public async Task<PagedServiceResult<IEnumerable<ProductModel>>> FetchProductByCategoryAsync(int pageNumber, int pageSize, Category category, string connectionString)
        {
            try
            {
                var categorizedProducts = await _productRepository.FetchPaginatedCategorizedProductsAsync(pageNumber, pageSize, category, connectionString);
                return ServiceHelper.BuildPagedResult<IEnumerable<ProductModel>>(
                    new ServiceResult<IEnumerable<ProductModel>>
                    {
                        Result = categorizedProducts,
                        HasError = false,
                        ErrorContent = null
                    },
                    pageNumber,
                    await _productRepository.FetchTotalNumberOfProductsAsync(category, connectionString)
                );
            }
            catch (Exception e)
            {
                return ServiceHelper.BuildPagedResult<IEnumerable<ProductModel>>(
                    new ServiceResult<IEnumerable<ProductModel>>
                    {
                        Result = null,
                        HasError = true,
                        ErrorContent = e.Message
                    },
                    pageNumber,
                    -1
                );
            }
        }
        
        public async Task<ServiceResult<ProductModel>> FetchProductDetailAsync(int productId, string connectionString)
        {
            try
            {
                var productDetails = await _productRepository.FetchAsync(productId, connectionString);
                return ServiceHelper.BuildServiceResult<ProductModel>(productDetails, false, null);
            }
            catch (Exception e) { return ServiceHelper.BuildServiceResult<ProductModel>(null, true, e.Message); }
        }
    }
}
