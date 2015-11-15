using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.Sql
{
    public class SqlOnBuilder
    {
        public SqlClauseBuilder On(string condition)
        {
            return new SqlClauseBuilder(" ON " + condition);
        }
    }

    public class SqlWhereBuilder
    {
        public SqlClauseBuilder Where(string condition)
        {
            return new SqlClauseBuilder(" WHERE " + condition);
        }
    }

    public class SqlClauseBuilder : SqlGenerator
    {
        private string _sql;

        public SqlClauseBuilder(string sql = "")
        {
            _sql = sql;
        }

        public SqlClauseBuilder Not()
        {
            _sql += " NOT";
            return this;
        }

        public SqlClauseBuilder And(string condition)
        {
            _sql += " AND " + condition;
            return this;
        }

        public SqlClauseBuilder Or(string condition)
        {
            _sql += " OR " + condition;
            return this;
        }

        public override string ToSql()
        {
            return _sql;
        }
    }
}
