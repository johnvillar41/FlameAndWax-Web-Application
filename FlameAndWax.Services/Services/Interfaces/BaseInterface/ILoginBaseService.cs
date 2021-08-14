using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.BaseInterface.Interface
{
    public interface ILoginBaseService<T> where T : class
    {
        Task<bool> Login(T loginCredentials);
        Task Register(T registeredCredentials);
    }
}
