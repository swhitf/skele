using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skele.Interop.Sql
{
    public class SqlDeleteBuilder : SqlGenerator
    {
        public SqlDeleteBuilder()
        {
        }

        public SqlDeleteBuilder(ISqlDialect dialect, string table)
        {
            Dialect = dialect;
            Table = table;
        }

        protected ISqlDialect Dialect
        {
            get;
            set;
        }

        protected string FilterClause
        {
            get;
            set;
        }

        protected string Table
        {
            get;
            set;
        }

        public SqlDeleteBuilder Filter(Func<SqlWhereBuilder, SqlClauseBuilder> whereCallback)
        {
            FilterClause = whereCallback(new SqlWhereBuilder()).ToSql();
            return this;
        }

        public override string ToSql()
        {
            var sql = new StringBuilder();
            sql.AppendFormat("DELETE FROM {0}", Dialect.EscapeObjectName(Table));

            if (!string.IsNullOrEmpty(FilterClause))
            {
                sql.Append(FilterClause);
            }

            return sql.ToString();
        }
    }
}
