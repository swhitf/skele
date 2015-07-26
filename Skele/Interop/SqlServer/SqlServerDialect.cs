using Skele.Interop.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop
{
    public class SqlServerDialect : ISqlDialect
    {
        public string EscapeObjectName(string name)
        {
            return string
                .Format("[{0}]", string.Join("].[", name.Split('.')))
                .Replace("[]", "");
        }

        public string FormatLiteralValue(Object value)
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
