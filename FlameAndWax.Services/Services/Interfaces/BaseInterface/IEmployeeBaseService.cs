using FlameAndWax.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.BaseInterface.Interface
{
    public interface IEmployeeBaseService
    {
        Task<ServiceResult<Boolean>> DeactivateCustomerAccount(int customerId,string connectionString);
        Task<ServiceResult<Boolean>> DeleteCustomerAccount(int employeeId,string connectionString);
        Task<ServiceResult<Boolean>> ModifyProduct(ProductModel updatedProduct, int productId,string connectionString);

        Task<ServiceResult<IEnumerable<CustomerModel>>> FetchAllCustomerAccounts(int pageNumber, int pageSize, string connectionString);        
        Task<ServiceResult<IEnumerable<ProductModel>>> FetchAllProducts(int pageNumber, int pageSize, string connectionString);
    }
}
