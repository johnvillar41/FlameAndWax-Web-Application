using System.Collections.Generic;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Repositories.Interfaces;

namespace FlameAndWax.Services.Repositories
{
    public class ShippingAddressRepository : IShippingAddressRepository
    {
        public Task<int> Add(ShippingAddressModel Data, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int id, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public Task<ShippingAddressModel> Fetch(int id, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<ShippingAddressModel>> FetchPaginatedResult(int pageNumber, int pageSize, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(ShippingAddressModel data, int id, string connectionString)
        {
            throw new System.NotImplementedException();
        }
    }
}