using FlameAndWax.Data.Models;
using System.Threading.Tasks;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IEmployeeRepository : IBaseRepository<EmployeeModel>
    {
        Task UpdateProfilePicture(string profileLink, string connectionString);
        Task<int> Login(EmployeeModel employee, string connectionString);
        Task ModifyEmployeeStatus(int employeeId, EmployeeAccountStatus accountStatus, string connectionString);
    }
}
