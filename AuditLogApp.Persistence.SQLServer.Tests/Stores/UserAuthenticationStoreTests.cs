using AuditLogApp.Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Persistence.SQLServer.Tests.Stores
{
    [TestFixture]
    public class UserAuthenticationStoreTests : TestBase
    {

        [Test]
        [Category("Integration:Database")]
        public async Task GetByUserAsync_ValidUserCredentialPair_GetsRecords()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.UserAuthentications.GetByUserAsync(ValidUserAuthentications.First().CredentialType, ValidUserId);

            Assert.AreEqual(ValidUserAuthentications.Count(), result.Count);
        }

        [Test]
        [Category("Integration:Database")]
        public async Task GetByUserAsync_ValidUserEmptyCredential_GetsEmptyList()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.UserAuthentications.GetByUserAsync(CredentialType.Twitter, ValidUserId);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        [Category("Integration:Database")]
        public async Task GetAsync_ValidId_GetsRecord()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.UserAuthentications.GetAsync(ValidUserAuthentications.First().Id);

            Assert.IsNotNull(result);
        }

        [Test]
        [Category("Integration:Database")]
        public async Task GetByUserAsync_nonexistentId_NoRecord()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.UserAuthentications.GetAsync(Guid.NewGuid());

            Assert.IsNull(result);
        }


        [Test]
        [Category("Integration:Database")]
        public async Task GetBySecretAsync_ValidCredentialAndSecret_GetsRecord()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.UserAuthentications.GetBySecretAsync(ValidUserAuthentications.First().CredentialType, ValidUserAuthentications.First().Secret);

            Assert.IsNotNull(result);
        }

        [Test]
        [Category("Integration:Database")]
        public async Task GetBySecretAsync_NonexistentCredentialAndSecret_NoRecord()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.UserAuthentications.GetBySecretAsync(ValidUserAuthentications.First().CredentialType, ValidUserAuthentications.First().Secret + " xyz");

            Assert.IsNull(result);
        }

        [Test]
        [Category("Integration:Database")]
        public async Task IsIdentityRegisteredAsync_ValidCredentialAndSecret_GetsRecord()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.UserAuthentications.IsIdentityRegisteredAsync(ValidUserAuthentications.First().CredentialType, ValidUserAuthentications.First().Secret);

            Assert.IsTrue(result);
        }

        [Test]
        [Category("Integration:Database")]
        public async Task IsIdentityRegisteredAsync_NonexistentCredentialAndSecret_NoRecord()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.UserAuthentications.IsIdentityRegisteredAsync(ValidUserAuthentications.First().CredentialType, ValidUserAuthentications.First().Secret + " xyz");

            Assert.IsFalse(result);
        }
    }
}
