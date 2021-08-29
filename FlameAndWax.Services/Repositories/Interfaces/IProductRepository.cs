using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Data.Repositories.Interfaces
{
    public interface IProductRepository : IBaseRepository<ProductModel>
    {
        Task ModifyNumberOfStocks(int productId, int numberOfStocksToBeSubtracted,string connectionString);
        Task ModifyNumberOfUnitsInOrder(int productId, int numberOfUnitsToBeAdded, string connectionString);
        Task UpdateAddUnitsOnOrder(int productId, int quantity, string connectionString);
        Task<IEnumerable<ProductModel>> FetchNewArrivedProducts(string connectionString);
        Task<IEnumerable<ProductModel>> FetchCategorizedProducts(Constants.Constants.Category category, string connectionString);
    }
}
