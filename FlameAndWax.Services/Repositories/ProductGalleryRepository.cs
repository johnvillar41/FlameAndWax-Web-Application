using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class ProductGalleryRepository : IProductGalleryRepository
    {
        public Task<int> Add(ProductGalleryModel Data)
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

        public Task<IEnumerable<ProductGalleryModel>> FetchPaginatedResult(int pageNumber, int pageSize)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<ProductGalleryModel>> FetchAllPicturesForProduct(int productId)
        {
            List<ProductGalleryModel> productGalleries = new List<ProductGalleryModel>();

            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM ProductGalleryTable WHERE ProductId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
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

        public Task Update(ProductGalleryModel data, int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
