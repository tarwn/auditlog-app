using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

// https://stackoverflow.com/questions/10293440/how-to-make-asp-net-mvc-model-binder-treat-incoming-date-as-utc

namespace AuditLogApp.Other
{
    public class DateTimeModelBinder : IModelBinder
    {
        private static readonly string[] DateTimeFormats = { "yyyyMMdd'T'HHmmss.FFFFFFFK", "yyyy-MM-dd'T'HH:mm:ss.FFFFFFFK" };

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var stringValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;

            if (bindingContext.ModelType == typeof(DateTime?) && string.IsNullOrEmpty(stringValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            bindingContext.Result = DateTime.TryParseExact(stringValue, DateTimeFormats,
                CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var result)
                ? ModelBindingResult.Success(result)
                : ModelBindingResult.Failed();

            return Task.CompletedTask;
        }
    }
}
