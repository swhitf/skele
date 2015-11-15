using Jint;
using Jint.Native;
using Skele.Interop;
using Skele.Interop.MetaModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    class TableAdapter
    {
        private Defaults _defaults;
        private IDatabaseSession _session;
        private TableDescriptor _table;

        public TableAdapter(IDatabaseSession session, TableDescriptor table, Defaults defaults)
        {
            _session = session;
            _table = table;
            _defaults = defaults;
        }

        public ExpandoObject add(ExpandoObject data)
        {
            ApplyDefaults(data);

            var builder = _session.Build();
            var sql = builder.Insert(_table.Name, data);

            _session.Execute(sql);
            return data;
        }

        public ExpandoObject[] addMany(ExpandoObject[] objects)
        {
            foreach (var data in objects)
            {
                add(data);
            }

            return objects;
        }

        public ExpandoObject[] generate(int count, Func<int, ExpandoObject> generator)
        {
            var objects = new List<ExpandoObject>();

            for (int i = 0; i < count; i++)
            {
                objects.Add(generator(i));
            }

            return addMany(objects.ToArray());
        }

        public void truncate()
        {
            var builder = _session.Build();
            var sql = builder.Truncate(_table.Name);

            _session.Execute(sql);
        }

        private void ApplyDefaults(ExpandoObject data)
        {
            var dataDict = (IDictionary<string, object>)data;

            foreach (var column in _table.Columns)
            {
                if (dataDict.ContainsKey(column.Name))
                    continue;

                var key = $"{_table.Name}.{column.Name}";

                foreach (var entry in _defaults)
                {
                    if (entry.Pattern.IsMatch(key))
                    {
                        dataDict[column.Name] = entry.Callback(new ColumnAdapter(column));
                    }
                }
            }
        }
    }
}
