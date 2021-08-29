using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IProductGalleryRepository : IBaseRepository<ProductGalleryModel>
    {
        Task<IEnumerable<ProductGalleryModel>> FetchAllPicturesForProduct(int productId, string connectionString);
    }
}
