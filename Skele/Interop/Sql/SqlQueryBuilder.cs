using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skele.Interop.Sql
{
    public class SqlQueryBuilder : SqlGenerator
    {
        public SqlQueryBuilder()
        {
        }

        public SqlQueryBuilder(ISqlDialect dialect, string table)
        {
            Dialect = dialect;
            Table = table;
            ColumnList = new List<string>();
            OrderList = new List<string>();

            JoinList = new List<JoinEntry>();
        }

        protected List<string> ColumnList
        {
            get;
            set;
        }

        protected ISqlDialect Dialect
        {
            get;
            set;
        }

        protected bool DistinctOnly
        {
            get;
            set;
        }

        protected string FilterClause
        {
            get;
            set;
        }

        protected List<JoinEntry> JoinList
        {
            get;
            set;
        }

        protected int LimitAmount
        {
            get;
            set;
        }

        protected List<string> OrderList
        {
            get;
            set;
        }

        protected string Table
        {
            get;
            set;
        }

        public SqlQueryBuilder Distinct()
        {
            DistinctOnly = true;
            return this;
        }

        public SqlQueryBuilder Filter(Func<SqlWhereBuilder, SqlClauseBuilder> whereCallback)
        {
            FilterClause = whereCallback(new SqlWhereBuilder()).ToSql();
            return this;
        }

        public SqlQueryBuilder InnerJoin(string table, Func<SqlOnBuilder, SqlClauseBuilder> onCallback)
        {
            return Join("INNER", table, onCallback);
        }

        public SqlQueryBuilder LeftJoin(string table, Func<SqlOnBuilder, SqlClauseBuilder> onCallback)
        {
            return Join("LEFT", table, onCallback);
        }

        public SqlQueryBuilder RightJoin(string table, Func<SqlOnBuilder, SqlClauseBuilder> onCallback)
        {
            return Join("RIGHT", table, onCallback);
        }

        protected SqlQueryBuilder Join(string type, string table, Func<SqlOnBuilder, SqlClauseBuilder> onCallback)
        {
            JoinList.Add(new JoinEntry
            {
                Condition = onCallback(new SqlOnBuilder()).ToSql(),
                Table = table,
                Type = type,
            });

            return this;
        }

        public SqlQueryBuilder Limit(int value)
        {
            LimitAmount = value;
            return this;
        }

        public SqlQueryBuilder OrderBy(params string[] columns)
        {
            return OrderBy((IEnumerable<string>)columns);
        }

        public SqlQueryBuilder OrderBy(IEnumerable<string> columns)
        {
            OrderList.AddRange(columns);
            return this;
        }

        public SqlQueryBuilder Select(params string[] columns)
        {
            ColumnList.AddRange(columns);
            return this;
        }

        public SqlQueryBuilder SelectAll()
        {
            ColumnList.Add("*");
            return this;
        }

        public override string ToSql()
        {
            var sql = new StringBuilder();
            sql.Append("SELECT");
            sql.Append(DistinctOnly ? "" : " DISTINCT");
            sql.Append(LimitAmount < 1 ? "" : " TOP " + LimitAmount);
            sql.AppendFormat(" {0}", string.Join(", ", SanitizedColumnList(ColumnList, " AS ").Distinct()));
            sql.AppendFormat(" FROM {0}", Dialect.EscapeObjectName(Table));

            foreach (var join in JoinList)
            {
                sql.AppendFormat(" {0} JOIN {1} {2}",
                    join.Type,
                    Dialect.EscapeObjectName(join.Table),
                    join.Condition);
            }

            if (!string.IsNullOrEmpty(FilterClause))
            {
                sql.Append(FilterClause);
            }

            if (OrderList.Any())
            {
                sql.AppendFormat(" ORDER BY {0}", string.Join(", ", SanitizedColumnList(OrderList, " ", false).Distinct()));
            }

            return sql.ToString();
        }

        private IEnumerable<string> SanitizedColumnList(IEnumerable<string> list, string joinValue, bool escapeModifier = true)
        {
            return SanitizedColumnList(
                list,
                (v, m) =>
                    Dialect.EscapeObjectName(v) +
                    joinValue +
                    (escapeModifier ? Dialect.EscapeObjectName(m) : m));
        }

        private IEnumerable<string> SanitizedColumnList(IEnumerable<string> list, Func<string, string, string> resolver)
        {
            foreach (var c in list)
            {
                if (c.Contains('|'))
                {
                    var nameModifier = c.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (nameModifier.Length > 1)
                    {
                        yield return resolver(nameModifier[0], nameModifier[1]);
                        continue;
                    }
                }

                yield return Dialect.EscapeObjectName(c);
            }
        }

        protected class JoinEntry
        {
            public string Condition;
            public string Table;
            public string Type;
        }
    }
}
