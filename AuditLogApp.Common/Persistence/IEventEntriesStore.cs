using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;

namespace AuditLogApp.Common.Persistence
{
    public interface IEventEntriesStore
    {
        Task<EventEntryId> CreateAsync(EventEntryDTO eventEntry);
        Task<EventEntryDTO> GetAsync(CustomerId customerId, EventEntryId entryId);
        Task<List<EventEntryDTO>> SearchAsync(CustomerId customerId, string clientUUID, DateTime? fromDate, DateTime? throughDate);

        Task<List<EventActorDTO>> GetEventActorsByUUIDAsync(CustomerId customerId, string uuid);
        Task<EventActorDTO> UpdateEventActorAsync(CustomerId customerId, string uuid, string name, string email);
        Task<EventActorDTO> ForgetEventActorAsync(CustomerId customerId, string uuid);

        Task<List<EventClientDTO>> GetAllClientsAsync(CustomerId customerId);
    }
}