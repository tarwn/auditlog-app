using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Enums;
using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Persistence.SQLServer.Stores
{
    public class CustomerAuthenticationStore : ICustomerAuthenticationStore
    {
        private DatabaseUtility _db;

        public CustomerAuthenticationStore(DatabaseUtility dbUtility)
        {
            _db = dbUtility;
        }

        public async Task<CustomerAuthenticationDTO> CreateAsync(CustomerAuthenticationDTO authMethod)
        {
            var sqlParams = new
            {
                Id = Guid.NewGuid(),
                UserId = authMethod.CustomerId.RawValue,
                CredentialType = (int)authMethod.CredentialType,
                authMethod.Secret,
                authMethod.DisplayName,
                CreationTime = DateTime.UtcNow,
                CreatedBy = authMethod.CreatedBy.RawValue
            };
            string sql = @";
                INSERT INTO dbo.CustomerAuthenticationMethods(Id, CustomerId, CredentialType, Secret, DisplayName, CreationTime, CreatedBy, IsRevoked)
                VALUES(@Id, @UserId, @CredentialType, @Secret, @DisplayName, @CreationTime, 0);

                SELECT Id,
                       CustomerId,
                       CredentialType,
                       Secret,
                       DisplayName,
                       CreationTime,
                       CreatedBy,
                       IsRevoked,
                       RevokeTime
                FROM dbo.CustomerAuthenticationMethods 
                WHERE Id = @Id;
            ";

            return await _db.QuerySingle(async (db) =>
            {
                return await db.FetchAsync<CustomerAuthenticationDTO>(sql, sqlParams);
            });
        }

        public async Task<List<CustomerAuthenticationDTO>> GetByCredentialTypeAsync(CustomerId customerId, CredentialType credentialType)
        {
            var sqlParams = new
            {
                CustomerId = customerId.RawValue,
                CredentialType = (int)credentialType,
            };
            string sql = @";
                SELECT Id,
                       UserId,
                       CredentialType,
                       Secret,
                       DisplayName,
                       CreationTime,
                       CreatedBy,
                       IsRevoked,
                       RevokeTime
                FROM dbo.UserAuthenticationMethods 
                WHERE CustomerId = @CustomerId
                    And CredentialType = @CredentialType;
            ";

            return await _db.Query(async (db) =>
            {
                return await db.FetchAsync<CustomerAuthenticationDTO>(sql, sqlParams);
            });
        }
    }
}
