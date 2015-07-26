using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.Sql
{
    public interface ISqlDialect
    {
        string EscapeObjectName(string name);

        string FormatLiteralValue(Object value);
    }
}
