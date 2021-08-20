using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IPreviouslyOrderedProducts
    {
        Task<bool> HasCustomerOrderedAProduct(int productId,int customerId);
    }
}
