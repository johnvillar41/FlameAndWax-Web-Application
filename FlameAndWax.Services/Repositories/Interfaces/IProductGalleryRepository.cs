using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IProductGalleryRepository
    {
        Task<IEnumerable<ProductGalleryModel>> FetchAllPicturesForProduct(int productId, string connectionString);
    }
}
