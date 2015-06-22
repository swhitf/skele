using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop
{
    public class ObjectDescriptorCollection<T> : KeyedCollection<string, T>
        where T : ObjectDescriptor
    {
        protected override string GetKeyForItem(T item)
        {
            return item.Name;
        }
    }
}
