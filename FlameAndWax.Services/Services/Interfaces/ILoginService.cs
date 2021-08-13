using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface ILoginService<T> where T : class
    {
        Task<bool> Login(T loginCredentials);
        Task Register(T registeredCredentials);
    }
}
