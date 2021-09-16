using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface ICustomerReviewRepository : IBaseRepository<CustomerReviewModel>
    {
        Task<IEnumerable<CustomerReviewModel>> FetchPaginatedReviewsOfAProduct(int pageSize, int pageNumber, int productId, string connectionString);
        Task<int> FetchTotalNumberOfReviewsOnAProduct(int productId, string connectionString);
        Task<IEnumerable<CustomerReviewModel>> FetchTopComments(string connectionString);
    }
}
