using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop
{
    public class TableDescriptor : ObjectDescriptor
    {
        public TableDescriptor()
        {
            Columns = new ObjectDescriptorCollection<ColumnDescriptor>();
        }

        public ObjectDescriptorCollection<ColumnDescriptor> Columns
        {
            get;
            private set;
        }
    }
}
