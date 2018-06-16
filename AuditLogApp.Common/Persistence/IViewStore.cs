using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;

namespace AuditLogApp.Common.Persistence
{
    public interface IViewStore
    {
        Task<ViewDTO> GetForCustomerAsync(CustomerId customerId);
        Task<ViewDTO> CreateNewAsync(ViewDTO view);
        Task<ViewDTO> UpdateAsync(ViewDTO view);
        Task<string> ResetKeyAsync(ViewId id, CustomerId customerId, string accessKey);
    }
}
