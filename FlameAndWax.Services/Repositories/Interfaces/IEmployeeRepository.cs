using FlameAndWax.Data.Models;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IEmployeeRepository : IBaseRepository<EmployeeModel>
    {
        Task<int> LoginEmployeeAccountAsync(EmployeeModel employeeModel, string connectionString);
        Task<int> FetchTotalEmployeesCountAsync(string connectionString);
    }
}
