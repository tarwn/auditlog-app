using System.Threading.Tasks;
using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;

namespace AuditLogApp.Common.Persistence
{
    public interface IEventEntriesStore
    {
        Task<EventEntryId> CreateAsync(EventEntryDTO eventEntry);
    }
}