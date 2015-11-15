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
using Skele.Interop.Sql;
using Skele.Interop.Mapping;

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
            var builder = _session.Build();
            var sql = builder.Insert($"{_table.Schema}.{_table.Name}", data);
            ApplyDefaults(data);

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
            var sql = builder.Truncate($"{_table.Schema}.{_table.Name}");

            _session.Execute(sql);
        }

        public ExpandoObject[] query(ExpandoObject where)
        {
            var sql = _session.Build()
                .Query($"{_table.Schema}.{_table.Name}")
                .SelectAll()
                .Filter(x => GenerateFilter(where, x))
                .ToSql();

            return _session.Query(sql, new ExpandoDataMapper()).ToArray();
        }

        private SqlClauseBuilder GenerateFilter(ExpandoObject where, SqlWhereBuilder wb)
        {
            var cb = wb.Where("1=1");
            var dataDict = (IDictionary<string, object>)where;

            foreach (var entry in dataDict)
            {
                cb.And(string.Format("{0} = {1}",
                    _session.Dialect.EscapeObjectName(entry.Key),
                    _session.Dialect.FormatLiteralValue(entry.Value))
                );
            }

            return cb;
        }

        //public void delete(ExpandoObject where)
        //{
        //    var dataDict = (IDictionary<string, object>)where;
        //    var sql = _session.Build()
        //        .Delete($"{_table.Schema}.{_table.Name}")
        //        .Filter(w =>
        //        {
        //            var c = w.Where("1=1");

        //            foreach (var entry in dataDict)
        //            {
        //                c.And(string.Format("{0} = {1}",
        //                    _session.Dialect.EscapeObjectName(entry.Key),
        //                    _session.Dialect.FormatLiteralValue(entry.Value))
        //                );
        //            }

        //            return c;
        //        })
        //        .ToSql();

        //    Console.WriteLine(sql);
        //}

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
