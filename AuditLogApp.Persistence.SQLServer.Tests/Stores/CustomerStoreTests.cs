using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Persistence.SQLServer.Tests.Stores
{
    [TestFixture]
    public class CustomerStoreTests : TestBase
    {
        [Test]
        [Category("Integration:Database")]
        public async Task GetAsync_TestCustomer_GetsCustomerRecord()
        {
            var database = new PersistenceStore(ConnectionString);

            var result = await database.Customers.GetAsync(TestBase.ValidCustomerId);

            Assert.AreEqual(TestBase.ValidCustomer.Id, result.Id);
            Assert.AreEqual(TestBase.ValidCustomer.DisplayName, result.DisplayName);
        }
    }
}
