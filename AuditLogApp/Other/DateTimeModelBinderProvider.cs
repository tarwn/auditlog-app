using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// https://stackoverflow.com/questions/10293440/how-to-make-asp-net-mvc-model-binder-treat-incoming-date-as-utc

namespace AuditLogApp.Other
{
    public class DateTimeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType != typeof(DateTime) &&
                context.Metadata.ModelType != typeof(DateTime?))
                return null;

            return new BinderTypeModelBinder(typeof(DateTimeModelBinder));
        }
    }
}
