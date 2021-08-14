using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.BaseInterface.Interface;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface IEmployeeService : ILoginBaseService<ServiceResult<EmployeeModel>>, IEmployeeBaseService
    {
    }
}
