using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.MetaModel
{
    public class DatabaseDescriptor : Descriptor
    {
        public DatabaseDescriptor()
        {
            Tables = new DescriptorCollection<TableDescriptor>();
        }

        public DescriptorCollection<TableDescriptor> Tables
        {
            get;
            private set;
        }
    }
}
