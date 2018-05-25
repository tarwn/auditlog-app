using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.ErrorNotification
{
    public class ExceptionToHandle
    {
        public string ExceptionType { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public ExceptionToHandle InnerException { get; set; }

        public static ExceptionToHandle FromException(Exception exc)
        {
            var eth = new ExceptionToHandle()
            {
                ExceptionType = exc.GetType().Name,
                Message = exc.Message,
                Source = exc.Source,
                StackTrace = exc.StackTrace
            };

            // TODO exception type-specific properties here

            if (exc.InnerException != null)
            {
                eth.InnerException = FromException(exc.InnerException);
            }

            return eth;
        }
    }
}
