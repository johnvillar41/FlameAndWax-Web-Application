using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.Models;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface ICartService 
    {
        Task<ServiceResult<bool>> CheckoutOrderAsync(OrderModel order, string usernameLoggedIn, string connectionString);
        Task<ServiceResult<double>> FetchProductPriceAsync(int productId, string connectionString);
        Task<ServiceResult<ProductModel>> FetchProductDetailAsync(int productId, string connectionString);
    }
}