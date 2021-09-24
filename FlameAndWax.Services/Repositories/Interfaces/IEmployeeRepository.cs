using FlameAndWax.Data.Models;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IEmployeeRepository : IFetchBaseInterface<EmployeeModel>
    {
        Task<int> LoginEmployeeAccount(EmployeeModel employeeModel, string connectionString);
    }
}
