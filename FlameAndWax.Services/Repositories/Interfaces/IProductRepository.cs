using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Data.Repositories.Interfaces
{
    public interface IProductRepository : IBaseRepository<ProductModel>
    {
        Task ModifyNumberOfStocks(int productId, int numberOfStocksToBeSubtracted);
        Task ModifyNumberOfUnitsInOrder(int productId, int numberOfUnitsToBeAdded);
        Task UpdateAddUnitsOnOrder(int productId, int quantity);
        Task<IEnumerable<ProductModel>> FetchNewArrivedProducts();
        Task<IEnumerable<ProductModel>> FetchCategorizedProducts(Constants.Constants.Category category);
    }
}
