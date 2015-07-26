using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;

namespace Skele.Interop.SqlServer
{
    public class SqlServerDriver : DatabaseDriver
    {
        private Server server;

        public SqlServerDriver()
        {
            server = new Server("STEPHEN-PC\\SQLEXPRESS");
        }

        public override DatabaseSession Create(string databaseName)
        {
            var database = new Database(server, databaseName);
            database.Create();

            return new SqlServerSession(database);
        }

        public override DatabaseSession Open(string databaseName)
        {
            if (!Exists(databaseName))
            {
                throw new DatabaseException(databaseName + " does not exist.");
            }

            return new SqlServerSession(server.Databases[databaseName]);
        }

        public override IEnumerable<string> List()
        {
            foreach (Database db in server.Databases)
            {
                yield return db.Name;
            }
        }

        public override bool Exists(string databaseName)
        {
            return server.Databases.Contains(databaseName);
        }

        public override SqlBuilder CreateSqlBuilder()
        {
            return new SqlServerSqlBuilder();
        }
    }
}
