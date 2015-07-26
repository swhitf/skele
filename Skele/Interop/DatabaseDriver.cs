using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop
{
    /// <summary>
    /// Represents a database driver.
    /// </summary>
    public abstract class DatabaseDriver
    {
        public abstract SqlBuilder CreateSqlBuilder();

        public abstract DatabaseSession Create(string databaseName);

        public abstract bool Exists(string databaseName);

        public abstract IEnumerable<string> List();

        public abstract DatabaseSession Open(string databaseName);
    }
}
