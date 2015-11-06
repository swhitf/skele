using Jint;
using Skele.Interop;
using Skele.Interop.MetaModel;
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
        private IDatabaseSession session;
        private Engine engine;

        public TableAdapter(IDatabaseSession session, TableDescriptor table)
        {
            this.session = session;
            this.table = table;
        }

        public void Add(IScriptContext context, IDictionary<string, Object> data)
        {
            var builder = session.Build();
            var sql = builder.Insert(table.Name, data);

            session.Execute(sql);
        }

        public void Truncate(IScriptContext context)
        {
            var builder = session.Build();
            var sql = builder.Truncate(table.Name);

            session.Execute(sql);
        }
    }
}
