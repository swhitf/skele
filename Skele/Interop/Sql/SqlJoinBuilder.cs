using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.Sql
{
    public class SqlJoinBuilder<T>
    {
        private Func<string, T> _callback;

        public SqlJoinBuilder(Func<string, T> callback)
        {
            _callback = callback;
        }

        public T On(Action<SqlClauseBuilder> clause)
        {
            var scb = new SqlClauseBuilder();
            clause(scb);

            return _callback(scb.ToSql());
        }
    }
}
