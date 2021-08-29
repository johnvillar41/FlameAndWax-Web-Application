using FlameAndWax.Data.Models;
using System.Threading.Tasks;

namespace FlameAndWax.Data.Repositories.Interfaces
{
    public interface IEmployeeRepository : IBaseRepository<EmployeeModel>
    {
        Task UpdateProfilePicture(string profileLink, string connectionString);
        Task<int> Login(EmployeeModel employee, string connectionString);
        Task ModifyEmployeeStatus(int employeeId, Constants.Constants.EmployeeAccountStatus accountStatus, string connectionString);
    }
}
