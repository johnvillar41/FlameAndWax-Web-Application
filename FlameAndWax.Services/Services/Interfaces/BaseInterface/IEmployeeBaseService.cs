using FlameAndWax.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.BaseInterface.Interface
{
    public interface IEmployeeBaseService
    {
        Task DeactivateCustomerAccount(int employeeId);
        Task DeleteCustomerAccount(int employeeId);
        Task ModifyProduct(ProductModel updatedProduct, int productId);        

        Task<IEnumerable<CustomerModel>> FetchAllCustomerAccounts();
        Task<IEnumerable<ProductModel>> FetchAllProducts();
    }
}
