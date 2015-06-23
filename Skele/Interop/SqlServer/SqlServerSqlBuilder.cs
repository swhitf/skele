using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.SqlServer
{
    class SqlServerSqlBuilder : SqlBuilder
    {
        protected override SqlDialect GetDialect()
        {
            return new SqlServerSqlDialect();
        }
    }
}
