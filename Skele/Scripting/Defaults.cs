using Skele.Interop.MetaModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Skele.Scripting
{
     class Defaults : KeyedCollection<Regex, Defaults.DefaultEntry>
    {
        public void Add(Regex pattern, Func<ColumnAdapter, object> callback)
        {
            Add(new DefaultEntry
            {
                Pattern = pattern,
                Callback = callback,
            });
        }

        protected override Regex GetKeyForItem(DefaultEntry item)
        {
            return item.Pattern;
        }

        public class DefaultEntry
        {
            public Func<ColumnAdapter, object> Callback { get; set; }
            public Regex Pattern { get; set; }
        }
    }
}
