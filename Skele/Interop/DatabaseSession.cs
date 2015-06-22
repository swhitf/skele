using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop
{
    public abstract class DatabaseSession
    {
        public abstract void CreateTable(TableDescriptor table);

        public abstract DatabaseDescriptor Describe();

        public abstract T QuerySingle<T>(string sql)
            where T : new();

        public abstract dynamic QuerySingle(string sql);

        public abstract void Execute(string sql);

        public abstract void ExecuteBatch(string batchScript);

        public abstract void ExecuteBatch(IEnumerable<string> batch);
    }
}