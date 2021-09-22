using FlameAndWax.Data.Models;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<EmployeeModel> Fetch(int employeeId, string connectionString);        
    }
}
