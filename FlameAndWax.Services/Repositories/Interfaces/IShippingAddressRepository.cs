using System.Collections.Generic;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IShippingAddressRepository : IBaseRepository<ShippingAddressModel>
    {
        Task<IEnumerable<ShippingAddressModel>> FetchAll(int customerId, string connectionString);
    }
}