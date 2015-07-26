using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.Mapping
{
    public static class AutoDataMapper
    {
        public static AutoDataMapper<T> FromPrototype<T>(T example)
            where T : new()
        {
            return new AutoDataMapper<T>();
        }
    }

    public class AutoDataMapper<T> : DataMapperBase<T>
        where T : new()
    {
        private Dictionary<string, PropertyDescriptor> properties;

        protected override T CreateInstance()
        {
            return new T();
        }

        protected override void MapProperty(T item, string name, Object value)
        {
            if (properties == null)
            {
                properties = TypeDescriptor.GetProperties(typeof(T))
                    .Cast<PropertyDescriptor>()
                    .ToDictionary(x => x.Name);
            }

            if (properties.ContainsKey(name))
            {
                properties[name].SetValue(item, value);
            }
            else
            {
                throw new ApplicationException(string.Format(
                    "Cannot map property {0} to object of type {1}.",
                    name,
                    typeof(T).Name));
            }
        }
    }
}
