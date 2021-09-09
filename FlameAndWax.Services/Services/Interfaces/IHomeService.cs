using System.Collections.Generic;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface IHomeService
    {
        Task<ServiceResult<IEnumerable<ProductModel>>> FetchNewArrivedProducts(string connectionString);
    }
}