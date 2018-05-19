using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditLogApp.Common.Identity
{
    // interfaces with the generic type are mappable to/from DB by adding cases to IdentityMapper

    public interface IIdentity<T> : IIdentity
    {
        T RawValue { get; }
    }

    public interface IIdentity
    {
    }
}
