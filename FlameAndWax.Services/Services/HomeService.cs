using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using FlameAndWax.Services.Services.Interfaces;
using FlameAndWax.Services.Services.Models;

namespace FlameAndWax.Services.Services
{
    public class HomeService : IHomeService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICustomerReviewRepository _customerReviewRepository;
        public HomeService(IProductRepository productRepository, ICustomerReviewRepository customerReviewRepository)
        {
            _productRepository = productRepository;
            _customerReviewRepository = customerReviewRepository;
        }        
        public async Task<ServiceResult<Tuple<IEnumerable<ProductModel>, IEnumerable<CustomerReviewModel>>>> FetchNewArrivedProductsAndTopCustomerReviewsAsync(string connectionString)
        {
            try
            {
                var newArrivals = await _productRepository.FetchNewArrivedProductsAsync(connectionString);
                var topCustomerComments = await _customerReviewRepository.FetchTopCommentsAsync(connectionString);
                var tuple = Tuple.Create<IEnumerable<ProductModel>, IEnumerable<CustomerReviewModel>>(newArrivals, topCustomerComments);

                return ServiceHelper.BuildServiceResult<Tuple<IEnumerable<ProductModel>, IEnumerable<CustomerReviewModel>>>(tuple, false, null);
            }
            catch (Exception e)
            {
                return ServiceHelper.BuildServiceResult<Tuple<IEnumerable<ProductModel>, IEnumerable<CustomerReviewModel>>>(null, true, e.Message);
            }
        }
    }
}