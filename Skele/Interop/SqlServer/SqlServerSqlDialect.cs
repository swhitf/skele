using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.SqlServer
{
    class SqlServerSqlDialect : SqlDialect
    {
        public override string EscapeObjectIdentifier(string name)
        {
            return string.Format("[{0}]", string.Join("].[", name.Split('.')));
        }

        public override string FormatLiteral(Object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is string)
            {
                return string.Format("'{0}'", value);
            }

            return value.ToString();
        }
    }
}
