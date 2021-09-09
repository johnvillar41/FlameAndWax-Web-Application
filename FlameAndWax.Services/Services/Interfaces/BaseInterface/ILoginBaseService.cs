using System;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.BaseInterface.Interface
{
    public interface IAccountBaseService<T> where T : class
    {
        Task<ServiceResult<int>> Login(T loginCredentials,string connectionString);
        Task<ServiceResult<Boolean>> Register(T registeredCredentials, string connectionString);
    }
}
