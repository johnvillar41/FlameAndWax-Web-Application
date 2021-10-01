using FlameAndWax.Data.Models;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IPreviouslyOrderedProductsRepository
    {
        Task<bool> HasCustomerOrderedAProductAsync(int productId,string customerUsername, string connectionString);
        Task<int> AddPreviouslyOrderedProductsAsync(PreviouslyOrderedProductModel previouslyOrderedProduct, string connectionString);
    }
}
