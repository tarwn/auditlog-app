using AuditLogApp.Common.DTO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Persistence.SQLServer.Tests.Stores
{
    [TestFixture]
    public class ViewStoreTests : TestBase
    {
        [Test]
        [Category("Integration:Database")]
        public async Task GetAsync_CreateViewAsync_ProducesNewView()
        {
            var database = new PersistenceStore(ConnectionString);
            string originalAccessKey = "abc-123";
            var view = new ViewDTO(null, ValidCustomerId, originalAccessKey, new ViewCustomizationDTO("url","logo", "title", new List<ViewCustomizationHeaderLinkDTO>(), "copyright"), new List<ViewColumnDTO>() { });

            var result = await database.Views.CreateNewAsync(view);

            Assert.AreEqual(originalAccessKey, result.AccessKey);
            Assert.AreNotEqual(originalAccessKey, view.AccessKey);
            Assert.AreEqual(view.CustomerId, result.CustomerId);
            Assert.IsNotNull(result.Id);
        }
    }
}
