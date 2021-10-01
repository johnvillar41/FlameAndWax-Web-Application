using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface ICustomerReviewRepository : IBaseRepository<CustomerReviewModel>
    {
        Task<IEnumerable<CustomerReviewModel>> FetchPaginatedReviewsOfAProductAsync(int pageSize, int pageNumber, int productId, string connectionString);
        Task<int> FetchTotalNumberOfReviewsOnAProductAsync(int productId, string connectionString);
        Task<IEnumerable<CustomerReviewModel>> FetchTopCommentsAsync(string connectionString);
    }
}
