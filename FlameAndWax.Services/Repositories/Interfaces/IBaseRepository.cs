using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IAddBaseInterface<T>
    {
        Task<int> Add(T Data, string connectionString);
    }
    public interface IDeleteBaseInterface
    {
        Task Delete(int id, string connectionString);
    }
    public interface IUpdateBaseInterface<T>
    {
        Task Update(T data, int id, string connectionString);
    }
    public interface IFetchBaseInterface<T>
    {
        Task<T> Fetch(int id, string connectionString);
    }
    public interface IBaseRepository<T> : IAddBaseInterface<T>, IDeleteBaseInterface, IUpdateBaseInterface<T>,IFetchBaseInterface<T> where T : class
    {
        Task<IEnumerable<T>> FetchPaginatedResult(int pageNumber, int pageSize, string connectionString);
    }
}
