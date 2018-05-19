using AuditLogApp.Common.DTO;
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
        public static UserDTO ValidUser => new UserDTO(ValidUserId, ValidCustomerId, "Sample User");
    }
}
