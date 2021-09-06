using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IProductRepository : IBaseRepository<ProductModel>
    {
        Task ModifyNumberOfStocks(int productId, int numberOfStocksToBeSubtracted, string connectionString);
        Task ModifyNumberOfUnitsInOrder(int productId, int numberOfUnitsToBeAdded, string connectionString);
        Task UpdateAddUnitsOnOrder(int productId, int quantity, string connectionString);
        Task<IEnumerable<ProductModel>> FetchNewArrivedProducts(string connectionString);
        Task<IEnumerable<ProductModel>> FetchCategorizedProducts(Category category, string connectionString);
        Task<int> FetchTotalNumberOfProducts(Category? category, string connectionString);
    }
}
