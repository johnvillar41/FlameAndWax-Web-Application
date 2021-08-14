using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Data.Repositories.Interfaces
{
    public interface ICustomerReviewRepository : IBaseRepository<CustomerReviewModel>
    {
        Task<IEnumerable<CustomerReviewModel>> FetchReviewsOfAProduct(int productId);
    }
}
