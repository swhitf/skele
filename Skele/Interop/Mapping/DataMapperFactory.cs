using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.Mapping
{
    static class DataMapperFactory
    {
        public interface ITranslator
        {
            bool Bool(string columnName);

            int Int(string columnName);

            string String(string columnName);
        }

        public static IDataMapper<T> Create<T>(Func<ITranslator, T> mapper)
        {
            return new DataMapperImpl<T>(mapper);
        }

        class DataMapperImpl<T> : IDataMapper<T>
        {
            private Func<ITranslator, T> mapper;

            public DataMapperImpl(Func<ITranslator, T> mapper)
            {
                this.mapper = mapper;
            }

            public T Map(IDataRecord record)
            {
                return mapper(new TranslatorImpl(record));
            }
        }

        class TranslatorImpl : ITranslator
        {
            private Dictionary<string, int> index;
            private IDataRecord record;
            public TranslatorImpl(IDataRecord record)
            {
                this.record = record;
                this.index = new Dictionary<string, int>();

                for (int i = 0; i < record.FieldCount; i++)
                {
                    index[record.GetName(i)] = i;
                }
            }

            public bool Bool(string columnName)
            {
                return Get(columnName, x => record.GetBoolean(x));
            }

            public int Int(string columnName)
            {
                return Get(columnName, x => record.GetInt32(x));
            }

            public string String(string columnName)
            {
                return Get(columnName, x => record.GetString(x));
            }

            private T Get<T>(string columnName, Func<int, T> getter)
            {
                if (record.IsDBNull(index[columnName]))
                    return default(T);

                return getter(index[columnName]);
            }
        }
    }
}
