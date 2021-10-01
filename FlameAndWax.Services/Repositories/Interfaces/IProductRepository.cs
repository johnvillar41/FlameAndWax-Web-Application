using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IProductRepository : IBaseRepository<ProductModel>
    {
        Task UpdateNumberOfStocksAsync(int productId, int numberOfStocksToBeSubtracted, string connectionString);
        Task UpdateNumberOfUnitsInOrderAsync(int productId, int numberOfUnitsToBeAdded, string connectionString);
        Task UpdateAddUnitsOnOrderAsync(int productId, int quantity, string connectionString);
        Task<IEnumerable<ProductModel>> FetchNewArrivedProductsAsync(string connectionString);
        Task<IEnumerable<ProductModel>> FetchPaginatedCategorizedProductsAsync(int pageNumber, int pageSize, Category category, string connectionString);
        Task<int> FetchTotalNumberOfProductsAsync(Category? category, string connectionString);
    }
}
