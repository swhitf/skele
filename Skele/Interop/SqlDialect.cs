using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop
{
    public abstract class SqlDialect
    {
        public abstract string EscapeObjectIdentifier(string name);

        public abstract string FormatLiteral(Object value);
    }
}
