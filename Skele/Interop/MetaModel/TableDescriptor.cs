using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.MetaModel
{
    public class TableDescriptor : Descriptor
    {
        public TableDescriptor()
        {
            Columns = new DescriptorCollection<ColumnDescriptor>();
        }

        public DescriptorCollection<ColumnDescriptor> Columns
        {
            get;
            private set;
        }
    }
}
