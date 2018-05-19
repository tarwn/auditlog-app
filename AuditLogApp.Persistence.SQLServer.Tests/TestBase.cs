using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Enums;
using AuditLogApp.Common.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace AuditLogApp.Persistence.SQLServer.Tests
{
    public class TestBase
    {

        public static string ConnectionString => "Server=.;Database=AuditLogDev;Trusted_Connection=True;";

        public static CustomerId ValidCustomerId => new CustomerId(1);
        public static CustomerDTO ValidCustomer => new CustomerDTO(ValidCustomerId, "Sample Customer");

        public static UserId ValidUserId => new UserId(1);
        public static UserDTO ValidUser => new UserDTO(ValidUserId, ValidCustomerId, "Sample User", "Sample User", "Sample User");

        public static List<UserAuthenticationDTO> ValidUserAuthentications => new List<UserAuthenticationDTO>
        {
            new UserAuthenticationDTO(Guid.Parse("62A4C043-09E8-44FA-BA60-5702E5E32047"), ValidUserId, CredentialType.PasswordHash, "secret", "display name", DateTime.Now),
            new UserAuthenticationDTO(Guid.Parse("9E7512C5-CE29-4802-9D95-84CC643C805D"), ValidUserId, CredentialType.PasswordHash, "secret 2", "display name 2", DateTime.Now)
        };
    }
}
