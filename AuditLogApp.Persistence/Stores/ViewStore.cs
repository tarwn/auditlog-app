using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;
using AuditLogApp.Common.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Persistence.SQLServer.Stores
{
    public class ViewStore : IViewStore
    {
        private DatabaseUtility _db;

        public ViewStore(DatabaseUtility dbUtility)
        {
            _db = dbUtility;
        }

        public async Task<ViewDTO> CreateNewAsync(ViewDTO view)
        {
            view.AccessKey = "----";

            var rawViewContent = JsonConvert.SerializeObject(view);
            var sqlParams = new
            {
                Id = Guid.NewGuid(),
                CustomerId = view.CustomerId.RawValue,
                Content = rawViewContent
            };
            string sql = @";
                INSERT INTO dbo.Views(Id, CustomerId, Content)
                VALUES(@Id, @CustomerId, @Content);

                SELECT Id, 
                       CustomerId,
                       AccessKey,
                       Content
                FROM dbo.Views V
                WHERE V.Id = @Id
                    AND V.CustomerId = @CustomerId;
            ";

            var rawView = await _db.QuerySingleOrDefault(async (db) =>
            {
                return await db.FetchAsync<RawView>(sql, sqlParams);
            });
            var newView = JsonConvert.DeserializeObject<ViewDTO>(rawView.Content);
            newView.Id = rawView.Id;
            newView.AccessKey = rawView.AccessKey;
            return newView;
        }


        public async Task<ViewDTO> GetForCustomerAsync(CustomerId customerId)
        {
            var sqlParams = new
            {
                CustomerId = customerId.RawValue
            };
            string sql = @";
                SELECT Id, 
                       CustomerId,
                       AccessKey,
                       Content
                FROM dbo.Views V
                WHERE V.CustomerId = @CustomerId;
            ";

            var rawView = await _db.QuerySingleOrDefault(async (db) =>
            {
                return await db.FetchAsync<RawView>(sql, sqlParams);
            });
            var view = JsonConvert.DeserializeObject<ViewDTO>(rawView.Content);
            view.Id = rawView.Id;
            view.AccessKey = rawView.AccessKey;
            return view;
        }

        public async Task<ViewDTO> UpdateAsync(ViewDTO view)
        {
            view.AccessKey = "----";
            var rawViewContent = JsonConvert.SerializeObject(view);
            var sqlParams = new
            {
                Id = view.Id.RawValue,
                CustomerId = view.CustomerId.RawValue,
                Content = rawViewContent
            };
            string sql = @";
                UPDATE dbo.Views
                SET Content = @Content
                WHERE V.Id = @Id
                    AND V.CustomerId = @CustomerId;

                SELECT Id, 
                       CustomerId,
                       AccessKey,
                       Content
                FROM dbo.Views V
                WHERE V.Id = @Id
                    AND V.CustomerId = @CustomerId;
            ";

            var rawView = await _db.QuerySingleOrDefault(async (db) =>
            {
                return await db.FetchAsync<RawView>(sql, sqlParams);
            });
            var newView = JsonConvert.DeserializeObject<ViewDTO>(rawView.Content);
            newView.Id = rawView.Id;
            newView.AccessKey = rawView.AccessKey;
            return newView;
        }

        public async Task<string> ResetKeyAsync(ViewId id, CustomerId customerId, string accessKey)
        {
            var sqlParams = new
            {
                Id = id.RawValue,
                CustomerId = customerId.RawValue,
                AccessKey = accessKey
            };
            string sql = @";
                UPDATE dbo.Views
                SET AccessKey = @AccessKey
                WHERE V.Id = @Id
                    AND V.CustomerId = @CustomerId;

                SELECT AccessKey
                FROM dbo.Views V
                WHERE V.Id = @Id
                    AND V.CustomerId = @CustomerId;
            ";

            return await _db.QuerySingleOrDefault(async (db) =>
            {
                return await db.FetchAsync<string>(sql, sqlParams);
            });
        }

    }

    public class RawView
    {
        public ViewId Id { get; set; }
        public CustomerId CustomerId { get; set; }
        public string AccessKey { get; set; }
        public string Content { get; set; }
    }
}
