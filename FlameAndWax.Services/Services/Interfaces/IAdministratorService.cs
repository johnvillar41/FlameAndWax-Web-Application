using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface IAdministratorService : ILoginBaseService<EmployeeModel>, IEmployeeBaseService
    {
        Task RemoveEmployee(int employeeId);
        Task MarkEmployeeAsTerminated(int employeeId);
    }
}
