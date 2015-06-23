using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop
{
    public abstract class SqlBuilder
    {
        public SqlInsertBuilder Insert(string table)
        {
            return new SqlInsertBuilder(GetDialect(), table);
        }

        public string Insert(string table, IDictionary<string, Object> data)
        {
            return this
                .Insert(table)
                .Columns(data.Keys.OrderBy(x => x).ToArray())
                .Values(data.Keys.OrderBy(x => x).Select(x => data[x]).ToArray())
                .ToSql();
        }

        protected abstract SqlDialect GetDialect();
    }
}
