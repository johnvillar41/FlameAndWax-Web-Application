using FlameAndWax.Data.Models;
using System.Threading.Tasks;

namespace FlameAndWax.Data.Repositories.Interfaces
{
    public interface IProductRepository : IBaseRepository<ProductModel>
    {
        Task ModifyNumberOfStocks(int productId, int numberOfStocksToBeSubtracted);
    }
}
