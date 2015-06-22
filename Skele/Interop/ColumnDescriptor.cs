using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop
{
    public class ColumnDescriptor : ObjectDescriptor
    {
        public ColumnDescriptor()
        {

        }

        public Type DataType
        {
            get;
            set;
        }

        public Nullable<int> Length
        {
            get;
            set;
        }

        public bool Nullable
        {
            get;
            set;
        }
    }
}
