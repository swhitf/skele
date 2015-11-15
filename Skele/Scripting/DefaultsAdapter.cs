using Skele.Interop.MetaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    class DefaultsAdapter
    {
        private Defaults _defaults;

        public DefaultsAdapter(Defaults defaults)
        {
            _defaults = defaults;
        }

        public void For(string pattern, Func<ColumnAdapter, object> callback)
        {
            _defaults.Add(new Regex(pattern, RegexOptions.IgnoreCase), callback);
        }
    }
}