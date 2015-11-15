using Skele.Interop.MetaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    class ColumnAdapter
    {
        private ColumnDescriptor _column;

        public ColumnAdapter(ColumnDescriptor column)
        {
            _column = column;
        }

        public string dataType
        {
            get { return _column.DataType.Name; }
        }

        public Nullable<int> length
        {
            get { return _column.Length; }
        }

        public bool nullable
        {
            get { return _column.Nullable; }
        }

        public string defaultValue
        {
            get { return _column.DefaultValue; }
        }
    }
}
