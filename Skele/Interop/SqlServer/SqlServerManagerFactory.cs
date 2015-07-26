using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.SqlServer
{
    class SqlServerManagerFactory : IDatabaseManagerFactory
    {
        public IDatabaseManager Create(string connectionString)
        {
            return new SqlServerManager(connectionString);
        }
    }
}
