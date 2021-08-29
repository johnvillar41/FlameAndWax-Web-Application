using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using System;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface IAdministratorService : ILoginBaseService<EmployeeModel>, IEmployeeBaseService
    {
        Task<ServiceResult<Boolean>> RemoveEmployee(int employeeId, string connectionString);
        Task<ServiceResult<Boolean>> MarkEmployeeAsTerminated(int employeeId, string connectionString);
    }
}
