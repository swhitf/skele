using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.MetaModel
{
    public class DescriptorCollection<T> : KeyedCollection<string, T>
        where T : Descriptor
    {
        protected override string GetKeyForItem(T item)
        {
            return item.Name;
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var i in items)
            {
                Add(i);
            }
        }
    }
}
