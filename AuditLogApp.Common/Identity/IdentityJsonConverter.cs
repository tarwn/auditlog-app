using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuditLogApp.Common.Identity
{
    public class IdentityJsonConverter<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((IIdentity<T>)value).RawValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value != null)
            {
                var ctor = objectType.GetConstructor(new Type[] { typeof(T) });
                var obj = ctor.Invoke(new object[] { (T)Convert.ChangeType(reader.Value, typeof(T)) });
                return obj;
            }
            else
            {
                return null;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IIdentity<T>).IsAssignableFrom(objectType);
        }
    }
}
