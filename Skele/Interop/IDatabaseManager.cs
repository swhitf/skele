using Skele.Interop.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop
{
    public interface IDatabaseManager : IDisposable
    {
        SqlBuilder BuildSql();

        IDatabaseSession Create(string databaseName);

        void Delete(string databaseName);

        bool Exists(string databaseName);

        IEnumerable<string> List();

        IDatabaseSession Open(string databaseName);
    }
}
