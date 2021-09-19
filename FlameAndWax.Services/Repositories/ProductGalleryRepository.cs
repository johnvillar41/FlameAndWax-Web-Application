using FlameAndWax.Data.Models;
using FlameAndWax.Services.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class ProductGalleryRepository : IProductGalleryRepository
    {
        public Task<int> Add(ProductGalleryModel Data, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int id, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProductGalleryModel> Fetch(int id, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<ProductGalleryModel>> FetchPaginatedResult(int pageNumber, int pageSize, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<ProductGalleryModel>> FetchAllPicturesForProduct(int productId, string connectionString)
        {
            List<ProductGalleryModel> productGalleries = new List<ProductGalleryModel>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("FetchAllPicturesForProduct", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", productId);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while(await reader.ReadAsync())
            {
                productGalleries.Add(
                        new ProductGalleryModel
                        {
                            ProductGalleryId = int.Parse(reader["ProductGalleryId"].ToString()),
                            ProductId = int.Parse(reader["ProductId"].ToString()),
                            PhotoLink = reader["ProductPhotoLink"].ToString()
                        }
                    );
            }
            return productGalleries;
        }

        public Task Update(ProductGalleryModel data, int id, string connectionString)
        {
            throw new System.NotImplementedException();
        }
    }
}
