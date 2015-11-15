using Skele.Interop.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Skele.Interop
{
    public class SqlServerDialect : ISqlDialect
    {
        public string EscapeObjectName(string name)
        {
            if (name == "*")
                return name;

            var escaped = new StringBuilder();
            escaped.Append('[');

            foreach (char c in name)
            {
                if (c == '.' || char.IsWhiteSpace(c))
                {
                    escaped.Append(']');
                }

                escaped.Append(c);

                if (c == '.' || char.IsWhiteSpace(c))
                {
                    escaped.Append('[');
                }
            }

            escaped.Append(']');
            return escaped.ToString();
        }

        public string FormatLiteralValue(Object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is DateTimeOffset)
            {
                value = ((DateTimeOffset)value).ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (value is DateTime)
            {
                value = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (value is string)
            {
                return string.Format("'{0}'", value);
            }

            return value.ToString();
        }
    }
}
