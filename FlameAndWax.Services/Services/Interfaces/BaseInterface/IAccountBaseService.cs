using System;
using System.Threading.Tasks;
using FlameAndWax.Services.Services.Models;

namespace FlameAndWax.Services.Services.BaseInterface.Interface
{
    public interface IAccountBaseService<T> where T : class
    {
        Task<ServiceResult<int>> LoginAsync(T loginCredentials,string connectionString);
        Task<ServiceResult<Boolean>> RegisterAsync(T registeredCredentials, string connectionString);        
    }
}
