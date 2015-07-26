using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.Sql
{
    public abstract class SqlGenerator
    {
        public abstract string ToSql();

        public override string ToString()
        {
            return ToSql();
        }
    }
}
