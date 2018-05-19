using AsyncPoco;
using AuditLogApp.Common.Identity;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AuditLogApp.Persistence.SQLServer
{
    public class IdentityMapper : IMapper
    {
        private StandardMapper standardMapper = new StandardMapper();

        public ColumnInfo GetColumnInfo(PropertyInfo pocoProperty)
        {
            return standardMapper.GetColumnInfo(pocoProperty);
        }

        public Func<object, object> GetFromDbConverter(PropertyInfo targetProperty, Type sourceType)
        {
            var t = targetProperty.PropertyType;
            if (typeof(IIdentity<int>).IsAssignableFrom(t))
            {
                var ctor = t.GetConstructor(new Type[] { typeof(Int32) });
                return (x) => ctor.Invoke(new object[] { (int)x });
            }
            else if (typeof(IIdentity<Guid>).IsAssignableFrom(t))
            {
                var ctor = t.GetConstructor(new Type[] { typeof(Guid) });
                return (x) => ctor.Invoke(new object[] { (Guid)x });
            }
            else
            {
                return standardMapper.GetFromDbConverter(targetProperty, sourceType);
            }
        }

        public TableInfo GetTableInfo(Type pocoType)
        {
            return standardMapper.GetTableInfo(pocoType);
        }

        public Func<object, object> GetToDbConverter(PropertyInfo sourceProperty)
        {
            if (typeof(IIdentity<int>).IsAssignableFrom(sourceProperty.PropertyType))
            {
                return (x) =>
                {
                    if (x == null)
                        return null;
                    else if (typeof(IIdentity<int>).IsAssignableFrom(x.GetType()))
                        return ((IIdentity<int>)x).RawValue;
                    else
                        return x;
                };
            }
            if (typeof(IIdentity<Guid>).IsAssignableFrom(sourceProperty.PropertyType))
            {
                return (x) =>
                {
                    if (x == null)
                        return null;
                    else if (typeof(IIdentity<Guid>).IsAssignableFrom(x.GetType()))
                        return ((IIdentity<Guid>)x).RawValue;
                    else
                        return x;
                };
            }
            else
            {
                return standardMapper.GetToDbConverter(sourceProperty);
            }
        }
    }
}
