using System.Threading.Tasks;

namespace FlameAndWax.Data.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task Add(T Data);
        Task Delete(int id);
        Task Update(T data);
        Task Fetch(int id);
        Task<T> FetchAll(int id);
    }
}
