using FlameAndWax.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.BaseInterface.Interface
{
    public interface IEmployeeBaseService
    {
        Task<ServiceResult<Boolean?>> DeactivateCustomerAccount(int employeeId);
        Task<ServiceResult<Boolean?>> DeleteCustomerAccount(int employeeId);
        Task<ServiceResult<Boolean?>> ModifyProduct(ProductModel updatedProduct, int productId);

        Task<ServiceResult<IEnumerable<CustomerModel>>> FetchAllCustomerAccounts();        
        Task<ServiceResult<IEnumerable<ProductModel>>> FetchAllProducts();
    }
}
