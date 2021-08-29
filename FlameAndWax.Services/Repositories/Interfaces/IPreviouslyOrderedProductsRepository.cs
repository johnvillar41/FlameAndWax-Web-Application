using FlameAndWax.Data.Models;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IPreviouslyOrderedProductsRepository
    {
        Task<bool> HasCustomerOrderedAProduct(int productId,string customerUsername, string connectionString);
        Task<int> AddPreviouslyOrderedProducts(PreviouslyOrderedProductModel previouslyOrderedProduct, string connectionString);
    }
}
