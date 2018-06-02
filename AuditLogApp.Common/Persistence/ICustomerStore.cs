using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;

namespace AuditLogApp.Common.Persistence
{
    public interface ICustomerStore
    {
        Task<CustomerDTO> GetAsync(CustomerId id);
        Task<CustomerDTO> CreateAsync(CustomerDTO customer);
    }
}
