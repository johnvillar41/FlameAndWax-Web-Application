using System;
using System.Threading.Tasks;
using FlameAndWax.Services.Services.Models;

namespace FlameAndWax.Services.Services.BaseInterface.Interface
{
    public interface IAccountBaseService<T> where T : class
    {
        Task<ServiceResult<int>> Login(T loginCredentials,string connectionString);
        Task<ServiceResult<Boolean>> Register(T registeredCredentials, string connectionString);        
    }
}
