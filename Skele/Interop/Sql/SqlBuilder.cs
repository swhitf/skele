using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.Sql
{
    public class SqlBuilder
    {
        private const string TruncateFormat = "TRUNCATE TABLE {0}";

        public SqlBuilder(ISqlDialect dialect)
        {
            Dialect = dialect;
        }

        protected ISqlDialect Dialect
        {
            get;
            set;
        }

        public SqlInsertBuilder Insert(string table)
        {
            return new SqlInsertBuilder(Dialect, table);
        }

        public string Insert(string table, IDictionary<string, Object> data)
        {
            return Insert(table)
                .Columns(data.Keys.OrderBy(x => x).ToArray())
                .Values(data.Keys.OrderBy(x => x).Select(x => data[x]).ToArray())
                .ToSql();
        }

        public string Truncate(string table)
        {
            return string.Format(TruncateFormat, Dialect.EscapeObjectName(table));
        }

        public SqlQueryBuilder Query(string table)
        {
            return new SqlQueryBuilder(Dialect, table);
        }
    }
}
