using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using FlameAndWax.Services.Services.Models;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface IEmployeeService : IAccountBaseService<EmployeeModel>
    {
        Task<PagedServiceResult<IEnumerable<EmployeeModel>>> FetchAllEmployees(int pageNumber, int pageSize, string connectionString);
    }
}