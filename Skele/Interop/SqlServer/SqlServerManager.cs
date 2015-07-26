using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SmoConnection = Microsoft.SqlServer.Management.Common.ServerConnection;
using SmoServer = Microsoft.SqlServer.Management.Smo.Server;
using SmoDatabase = Microsoft.SqlServer.Management.Smo.Database;
using Skele.Interop.Mapping;
using Skele.Interop.Sql;

namespace Skele.Interop.SqlServer
{
    class SqlServerManager : IDatabaseManager
    {
        private SqlConnectionStringBuilder connString;

        public SqlServerManager(string connectionString)
        {
            connString = new SqlConnectionStringBuilder(connectionString);
        }

        public IDatabaseSession Create(string databaseName)
        {
            if (Exists(databaseName))
            {
                throw new DatabaseException("Cannot create database because the name it already exists: " + databaseName);
            }

            var server = new SmoServer(connString.DataSource);
            var database = new SmoDatabase(server, databaseName);
            database.Create();

            return OpenInternal(databaseName);
        }

        public SqlBuilder BuildSql()
        {
            return new SqlBuilder(new SqlServerDialect());
        }

        public bool Exists(string databaseName)
        {
            return List().Contains(databaseName);
        }

        public IEnumerable<string> List()
        {
            var session = OpenInternal("master");
            var sql = BuildSql()
                .Query("sys.databases")
                .Select("name")
                .ToSql();

            var mapper = DataMapperFactory.Create<string>(
                x => x.String("name"));

            return session.Query(sql, mapper).ToList();
        }

        public IDatabaseSession Open(string databaseName)
        {
            if (!Exists(databaseName))
            {
                throw new DatabaseException(databaseName + " does not exist.");
            }

            return OpenInternal(databaseName);
        }

        private string CreateConnectionString(string databaseName)
        {
            var cs = new SqlConnectionStringBuilder(connString.ConnectionString);
            cs.InitialCatalog = databaseName;

            return cs.ConnectionString;
        }

        private IDatabaseSession OpenInternal(string databaseName)
        {
            return new SqlServerSession(databaseName, CreateConnectionString(databaseName));
        }

        public void Delete(string databaseName)
        {
            if (Exists(databaseName))
            {
                var session = OpenInternal(databaseName);
                var sql = new StringBuilder();
                sql.AppendFormat("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", databaseName);
                sql.AppendLine();
                sql.AppendLine("USE master");
                sql.AppendFormat("DROP DATABASE [{0}]", databaseName);

                session.Execute(sql.ToString());
            }
        }
    }
}
