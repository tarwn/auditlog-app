using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Enums;
using AuditLogApp.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Application.Models
{
    public class APIKeyModel
    {
        public APIKeyModel() { }

        public APIKeyModel(CustomerAuthenticationDTO method)
        {
            Id = method.Id;
            Secret = method.Secret;
            DisplayName = method.DisplayName;
            CreationTime = method.CreationTime;
            IsRevoked = method.IsRevoked;
            RevokeTime = method.RevokeTime;
            CredentialType = method.CredentialType;
            CreatedBy = method.CreatedBy;
        }

        public CustomerAuthenticationId Id { get; set; }
        public string Secret { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokeTime { get; set; }

        //??
        public CredentialType CredentialType { get; set; }
        public UserId CreatedBy { get; set; }
    }
}
