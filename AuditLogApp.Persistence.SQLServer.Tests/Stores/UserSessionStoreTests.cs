using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Persistence.SQLServer.Tests.Stores
{
    [TestFixture]
    public class UserSessionStoreTests : TestBase
    {
        [Test]
        [Category("Integration:Database")]
        public async Task CreateAsync_ValidUser_GetsNewSession()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.UserSessions.CreateAsync(ValidUserId, DateTime.UtcNow);

            Assert.AreEqual(TestBase.ValidUser.Id, result.UserId);
        }
    }
}
