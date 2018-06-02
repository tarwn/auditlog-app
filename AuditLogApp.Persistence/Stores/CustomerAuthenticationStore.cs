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
                CustomerId = authMethod.CustomerId.RawValue,
                CredentialType = (int)authMethod.CredentialType,
                authMethod.Secret,
                authMethod.DisplayName,
                CreationTime = DateTime.UtcNow,
                CreatedBy = authMethod.CreatedBy.RawValue
            };
            string sql = @";
                INSERT INTO dbo.CustomerAuthenticationMethods(Id, CustomerId, CredentialType, Secret, DisplayName, CreationTime, CreatedBy, IsRevoked)
                VALUES(@Id, @CustomerId, @CredentialType, @Secret, @DisplayName, @CreationTime, @CreatedBy, 0);

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

        public async Task<CustomerAuthenticationDTO> GetAsync(CustomerId customerId, CustomerAuthenticationId id)
        {
            var sqlParams = new
            {
                CustomerId = customerId.RawValue,
                Id = id.RawValue
            };
            string sql = @";
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
                WHERE CustomerId = @CustomerId
                    AND Id = @Id;
            ";

            return await _db.QuerySingleOrDefault(async (db) =>
            {
                return await db.FetchAsync<CustomerAuthenticationDTO>(sql, sqlParams);
            });
        }

        public async Task<CustomerAuthenticationDTO> GetAsync(CustomerAuthenticationId id)
        {
            var sqlParams = new
            {
                Id = id.RawValue
            };
            string sql = @";
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

            return await _db.QuerySingleOrDefault(async (db) =>
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
                       CustomerId,
                       CredentialType,
                       Secret,
                       DisplayName,
                       CreationTime,
                       CreatedBy,
                       IsRevoked,
                       RevokeTime
                FROM dbo.CustomerAuthenticationMethods 
                WHERE CustomerId = @CustomerId
                    And CredentialType = @CredentialType;
            ";

            return await _db.Query(async (db) =>
            {
                return await db.FetchAsync<CustomerAuthenticationDTO>(sql, sqlParams);
            });
        }

        public async Task<CustomerAuthenticationDTO> UpdateAsync(CustomerAuthenticationDTO authMethod)
        {
            var sqlParams = new
            {
                Id = authMethod.Id.RawValue,
                CustomerId = authMethod.CustomerId.RawValue,
                authMethod.IsRevoked,
                authMethod.RevokeTime
            };
            string sql = @";
                UPDATE dbo.CustomerAuthenticationMethods
                SET IsRevoked = @IsRevoked,
                    RevokeTime = @RevokeTime
                WHERE CustomerId = @CustomerId
                    And Id = @Id;

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
                WHERE CustomerId = @CustomerId
                    And Id = @Id;
            ";

            return await _db.QuerySingle(async (db) =>
            {
                return await db.FetchAsync<CustomerAuthenticationDTO>(sql, sqlParams);
            });
        }
    }
}
