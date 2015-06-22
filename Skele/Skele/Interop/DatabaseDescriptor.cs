using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop
{
    public class DatabaseDescriptor : ObjectDescriptor
    {
        public DatabaseDescriptor()
        {
            Tables = new ObjectDescriptorCollection<TableDescriptor>();
        }

        public ObjectDescriptorCollection<TableDescriptor> Tables
        {
            get;
            private set;
        }
    }
}
