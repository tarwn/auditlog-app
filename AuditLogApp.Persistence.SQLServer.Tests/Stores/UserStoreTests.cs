using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Persistence.SQLServer.Tests.Stores
{
    [TestFixture]
    public class UserStoreTests : TestBase
    {
        [Test]
        [Category("Integration:Database")]
        public async Task GetAsync_TestUser_GetsUserRecord()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.Users.GetAsync(TestBase.ValidCustomerId, TestBase.ValidUserId);

            Assert.AreEqual(TestBase.ValidUser.Id, result.Id);
            Assert.AreEqual(TestBase.ValidUser.CustomerId, result.CustomerId);
            Assert.AreEqual(TestBase.ValidUser.DisplayName, result.DisplayName);
        }
    }
}
