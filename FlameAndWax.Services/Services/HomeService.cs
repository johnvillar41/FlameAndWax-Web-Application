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
        public HomeService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
    }
}