using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Persistence.SQLServer.Stores
{
    public class EventEntriesStore : IEventEntriesStore
    {
        private DatabaseUtility _db;

        public EventEntriesStore(DatabaseUtility dbUtility)
        {
            _db = dbUtility;
        }

        public async Task<EventEntryId> CreateAsync(EventEntryDTO eventEntry)
        {
            var sqlParams = new
            {
                CustomerId = eventEntry.CustomerId.RawValue,
                eventEntry.ReceptionTime,
                eventEntry.UUID,
                eventEntry.Client_Id,
                eventEntry.Client_Name,
                eventEntry.EventTime,
                eventEntry.Action,
                eventEntry.Description,
                eventEntry.URL,
                eventEntry.Actor_UUID,
                eventEntry.Actor_Name,
                eventEntry.Actor_Email,
                eventEntry.Context_Client_IP,
                eventEntry.Context_Client_BrowserAgent,
                eventEntry.Context_Server_ServerId,
                eventEntry.Context_Server_Version,
                eventEntry.Target_Type,
                eventEntry.Target_UUID,
                eventEntry.Target_Label,
                eventEntry.Target_URL
            };

            // double @ is for variables delcared in the statement so PteaPoco won't try to provide them

            string sql = @";
                DECLARE @@ActorId uniqueidentifier;
                SELECT @@ActorId = Id 
                FROM dbo.EventActors
                WHERE CustomerId = @CustomerId
                    AND UUID = @Actor_UUID;

                IF @@ActorId IS NULL
                BEGIN
                    SET @@ActorId = NewID();
                    INSERT INTO dbo.EventActors(Id, CustomerId, UUID, Name, Email)
                    VALUES(@@ActorId, @CustomerId, @Actor_UUID, @Actor_Name, @Actor_Email);
                END
                ELSE
                BEGIN
                    UPDATE dbo.EventActors
                    SET Name = @Actor_Name,
                        Email = @Actor_Email
                    WHERE Id = @@ActorId;
                END

                DECLARE @@Id uniqueidentifier = NewId();
                INSERT INTO dbo.EventEntries(Id, CustomerId, ReceptionTime, UUID, Client_Id, Client_Name, EventTime, Action, Description, URL, EventActorId, Context_Client_IP, Context_Client_BrowserAgent, Context_Server_ServerId, Context_Server_Version, Target_Type, Target_UUID, Target_Label, Target_URL)
                VALUES(                    @@Id,@CustomerId,@ReceptionTime,@UUID,@Client_Id,@Client_Name,@EventTime,@Action,@Description,@URL,    @@ActorId,@Context_Client_IP,@Context_Client_BrowserAgent,@Context_Server_ServerId,@Context_Server_Version,@Target_Type,@Target_UUID,@Target_Label,@Target_URL);

                SELECT @@Id as Id;
            ";

            var rawId = await _db.QuerySingle(async (db) =>
            {
                return await db.FetchAsync<Guid>(sql, sqlParams);
            });
            return new EventEntryId(rawId);
        }
    }
}
