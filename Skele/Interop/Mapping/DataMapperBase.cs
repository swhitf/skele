using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.Mapping
{
    public abstract class DataMapperBase<T> : IDataMapper<T>
    {
        public T Map(IDataRecord record)
        {
            var obj = CreateInstance();

            for (int i = 0; i < record.FieldCount; i++)
            {

                var name = record.GetName(i);
                var val = record.GetValue(i);

                MapProperty(obj, name, val);
            }

            return obj;
        }

        protected abstract T CreateInstance();

        protected abstract void MapProperty(T item, string name, Object value);
    }
}
