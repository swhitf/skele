using Skele.Interop;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    class TableAdapter
    {
        private TableDescriptor table;

        public TableAdapter(TableDescriptor table)
        {
            this.table = table;
        }

        public void add(IDictionary<string, Object> data)
        {
            throw new NotImplementedException();
        }
    }
}
