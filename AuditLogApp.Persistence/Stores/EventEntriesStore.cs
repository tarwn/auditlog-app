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

        #region Event Entry

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
                    INSERT INTO dbo.EventActors(Id, CustomerId, UUID, Name, Email, IsForgotten)
                    VALUES(@@ActorId, @CustomerId, @Actor_UUID, @Actor_Name, @Actor_Email, 0);
                END
                ELSE
                BEGIN
                    UPDATE dbo.EventActors
                    SET Name = @Actor_Name,
                        Email = @Actor_Email
                    WHERE Id = @@ActorId
                        AND IsForgotten = 0;
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
        
        public async Task<EventEntryDTO> GetAsync(CustomerId customerId, EventEntryId entryId)
        {
            var sqlParams = new {
                CustomerId = customerId.RawValue,
                Id = entryId.RawValue
            };

            string sql = @"
                SELECT EE.Id, 
                       EE.CustomerId, 
                       EE.ReceptionTime, 
                       EE.UUID, 
                       EE.Client_Id, 
                       EE.Client_Name, 
                       EE.EventTime, 
                       EE.Action, 
                       EE.Description, 
                       EE.URL, 
                       EE.EventActorId AS Actor_Id, 
                       EA.UUID AS Actor_UUID,
                       EA.Name AS Actor_Name,
                       EA.Email AS Actor_Email,
                       EE.Context_Client_IP, 
                       EE.Context_Client_BrowserAgent, 
                       EE.Context_Server_ServerId, 
                       EE.Context_Server_Version, 
                       EE.Target_Type, 
                       EE.Target_UUID, 
                       EE.Target_Label, 
                       EE.Target_URL
                FROM dbo.EventEntries EE   
                    INNER JOIN dbo.EventActors EA ON EA.Id = EE.EventActorId
                WHERE EE.CustomerId = @CustomerId
                    AND EE.Id = @Id;
            ";

            return await _db.QuerySingleOrDefault(async (db) =>
            {
                return await db.FetchAsync<EventEntryDTO>(sql, sqlParams);
            });
        }

        public async Task<List<EventEntryDTO>> SearchAsync(CustomerId customerId, string clientID, DateTime? fromDate, DateTime? throughDate)
        {
            var whereClauses = new List<string>();
            if (!String.IsNullOrEmpty(clientID))
            {
                whereClauses.Add("AND EE.Client_ID = @ClientID");
            }
            if (fromDate.HasValue)
            {
                whereClauses.Add("AND EE.EventTime >= @FromDate");
            }
            if (throughDate.HasValue)
            {
                if (throughDate.Value.Hour == 0 && throughDate.Value.Minute == 0)
                {
                    whereClauses.Add("AND EE.EventTime < DateAdd(day, 1, @ThroughDate)");
                }
                else
                {
                    whereClauses.Add("AND EE.EventTime <= @ThroughDate");
                }
            }

            var sqlParams = new
            {
                CustomerId = customerId.RawValue,
                ClientID = clientID,
                FromDate = fromDate,
                ThroughDate = throughDate
            };
            string sql = @"
                SELECT EE.Id, 
                       EE.CustomerId, 
                       EE.ReceptionTime, 
                       EE.UUID, 
                       EE.Client_Id, 
                       EE.Client_Name, 
                       EE.EventTime, 
                       EE.Action, 
                       EE.Description, 
                       EE.URL, 
                       EE.EventActorId AS Actor_Id, 
                       EA.UUID AS Actor_UUID,
                       EA.Name AS Actor_Name,
                       EA.Email AS Actor_Email,
                       EE.Context_Client_IP, 
                       EE.Context_Client_BrowserAgent, 
                       EE.Context_Server_ServerId, 
                       EE.Context_Server_Version, 
                       EE.Target_Type, 
                       EE.Target_UUID, 
                       EE.Target_Label, 
                       EE.Target_URL
                FROM dbo.EventEntries EE   
                    INNER JOIN dbo.EventActors EA ON EA.Id = EE.EventActorId
                WHERE EE.CustomerId = @CustomerId
                    " + String.Join("\n", whereClauses) + @";
            ";

            return await _db.Query(async (db) =>
            {
                return await db.FetchAsync<EventEntryDTO>(sql, sqlParams);
            });
        }

        #endregion

        #region Actors

        public async Task<List<EventActorDTO>> GetEventActorsByUUIDAsync(CustomerId customerId, string uuid)
        {
            var sqlParams = new
            {
                CustomerId = customerId.RawValue,
                UUID = uuid
            };
            string sql = @";
                SELECT A.Id,
                       A.CustomerId,
                       A.UUID,
                       A.Name,
                       A.Email,
                       A.IsForgotten
                FROM dbo.EventActors A
                WHERE A.CustomerId = @CustomerId
                    AND A.UUId = @UUID;
            ";

            return await _db.Query(async (db) =>
            {
                return await db.FetchAsync<EventActorDTO>(sql, sqlParams);
            });
        }


        public async Task<EventActorDTO> ForgetEventActorAsync(CustomerId customerId, string uuid)
        {
            var sqlParams = new
            {
                CustomerId = customerId.RawValue,
                UUID = uuid
            };
            string sql = @";
                UPDATE dbo.EventActors 
                SET IsForgotten = 1,
                    Name = '[forgotten]',
                    Email = '[forgotten]'
                WHERE CustomerId = @CustomerId
                    AND UUId = @UUID;

                SELECT TOP 1 A.Id,
                       A.CustomerId,
                       A.UUID,
                       A.Name,
                       A.Email,
                       A.IsForgotten
                FROM dbo.EventActors A
                WHERE A.CustomerId = @CustomerId
                    AND A.UUId = @UUID;
            ";

            return await _db.QuerySingle(async (db) =>
            {
                return await db.FetchAsync<EventActorDTO>(sql, sqlParams);
            });
        }

        public async Task<EventActorDTO> UpdateEventActorAsync(CustomerId customerId, string uuid, string name, string email)
        {
            var sqlParams = new
            {
                CustomerId = customerId.RawValue,
                UUID = uuid,
                Name = name,
                Email = email
            };
            string sql = @";
                UPDATE dbo.EventActors 
                SET Name = @Name,
                    Email = @Email
                WHERE CustomerId = @CustomerId
                    AND UUId = @UUID
                    AND IsForgotten = 0;

                SELECT TOP 1 A.Id,
                       A.CustomerId,
                       A.UUID,
                       A.Name,
                       A.Email,
                       A.IsForgotten
                FROM dbo.EventActors A
                WHERE A.CustomerId = @CustomerId
                    AND A.UUId = @UUID;
            ";

            return await _db.QuerySingle(async (db) =>
            {
                return await db.FetchAsync<EventActorDTO>(sql, sqlParams);
            });
        }


        #endregion

    }
}
