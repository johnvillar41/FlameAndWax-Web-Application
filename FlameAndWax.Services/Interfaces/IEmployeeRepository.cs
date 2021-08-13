using FlameAndWax.Data.Models;
using System.Threading.Tasks;

namespace FlameAndWax.Data.Interfaces
{
    public interface IEmployeeRepository : IBaseRepository<EmployeeModel>
    {
        Task UpdateProfilePicture(string profileLink);
    }
}
