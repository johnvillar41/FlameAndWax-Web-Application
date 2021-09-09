using System.Threading.Tasks;
using FlameAndWax.Data.Models;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface ICartService 
    {
        Task<ServiceResult<bool>> CheckoutOrder(OrderModel order, string usernameLoggedIn, string connectionString);
        Task<ServiceResult<double>> FetchProductPrice(int productId, string connectionString);
        Task<ServiceResult<ProductModel>> FetchProductDetail(int productId, string connectionString);
    }
}