using FlameAndWax.Data.Models;
using FlameAndWax.Services.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class ProductGalleryRepository : IProductGalleryRepository
    {
        public Task Add(ProductGalleryModel Data)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProductGalleryModel> Fetch(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<ProductGalleryModel>> FetchAll()
        {
            throw new System.NotImplementedException();
        }

        public Task Update(ProductGalleryModel data, int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
