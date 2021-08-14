using System;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.BaseInterface.Interface
{
    public interface ILoginBaseService<T> where T : class
    {
        Task<ServiceResult<Boolean?>> Login(T loginCredentials);
        Task<ServiceResult<Boolean?>> Register(T registeredCredentials);
    }
}
