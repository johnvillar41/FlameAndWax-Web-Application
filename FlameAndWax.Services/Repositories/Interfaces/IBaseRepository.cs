using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Data.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<int> Add(T Data, string connectionString);
        Task Delete(int id, string connectionString);
        Task Update(T data, int id, string connectionString);
        Task<T> Fetch(int id, string connectionString);
        Task<IEnumerable<T>> FetchPaginatedResult(int pageNumber, int pageSize, string connectionString);
    }
}
