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
        public async Task GetAsync_TestUserAdCustomer_GetsUserRecord()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.Users.GetAsync(TestBase.ValidCustomerId, TestBase.ValidUserId);

            Assert.AreEqual(TestBase.ValidUser.Id, result.Id);
            Assert.AreEqual(TestBase.ValidUser.CustomerId, result.CustomerId);
            Assert.AreEqual(TestBase.ValidUser.DisplayName, result.DisplayName);
        }

        [Test]
        [Category("Integration:Database")]
        public async Task GetAsync_TestUser_GetsUserRecord()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.Users.GetAsync(TestBase.ValidUserId);

            Assert.AreEqual(TestBase.ValidUser.Id, result.Id);
            Assert.AreEqual(TestBase.ValidUser.CustomerId, result.CustomerId);
            Assert.AreEqual(TestBase.ValidUser.DisplayName, result.DisplayName);
        }

        [Test]
        [Category("Integration:Database")]
        public async Task GetByUsernameAsync_TestUser_GetsUserRecord()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.Users.GetByUsernameAsync(TestBase.ValidUser.Username);

            Assert.AreEqual(TestBase.ValidUser.Id, result.Id);
            Assert.AreEqual(TestBase.ValidUser.CustomerId, result.CustomerId);
            Assert.AreEqual(TestBase.ValidUser.DisplayName, result.DisplayName);
        }

        [Test]
        [Category("Integration:Database")]
        public async Task IsUsernameRegisteredAsync_TestUser_Exists()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.Users.IsUsernameRegisteredAsync(TestBase.ValidUser.Username);

            Assert.IsTrue(result);
        }


        [Test]
        [Category("Integration:Database")]
        public async Task IsUsernameRegisteredAsync_NonExistentUser_DoesNotExist()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.Users.IsUsernameRegisteredAsync(TestBase.ValidUser.Username + Guid.NewGuid().ToString());

            Assert.IsFalse(result);
        }
    }
}
