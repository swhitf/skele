using Skele.Interop.MetaModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SmoConnection = Microsoft.SqlServer.Management.Common.ServerConnection;
using SmoServer = Microsoft.SqlServer.Management.Smo.Server;
using SmoDatabase = Microsoft.SqlServer.Management.Smo.Database;
using SmoTable = Microsoft.SqlServer.Management.Smo.Table;
using SmoColumn = Microsoft.SqlServer.Management.Smo.Column;
using SmoDataType = Microsoft.SqlServer.Management.Smo.DataType;
using Skele.Interop.Mapping;
using Skele.Interop.Sql;

namespace Skele.Interop.SqlServer
{
    class SqlServerSession : IDatabaseSession
    {
        private string databaseName;
        private string connString;

        public SqlServerSession(string databaseName, string connString)
        {
            this.databaseName = databaseName;
            this.connString = connString;
        }

        public SqlBuilder Build()
        {
            return new SqlBuilder(new SqlServerDialect());
        }

        public void CreateTable(TableDescriptor tableSpec)
        {
            WithSmo(database =>
            {
                var table = new SmoTable(database, tableSpec.Name);

                foreach (var columnSpec in tableSpec.Columns)
                {
                    var type = columnSpec.Length.HasValue
                        ? new SmoDataType(SmoTypes.GetSmoType(columnSpec.DataType), columnSpec.Length.Value)
                        : new SmoDataType(SmoTypes.GetSmoType(columnSpec.DataType));

                    var column = new SmoColumn(table, columnSpec.Name, type);
                    table.Columns.Add(column);
                }

                table.Create();
            });
        }

        public DatabaseDescriptor Describe()
        {
            return MetadataFactory.Create(this);
        }

        public void Execute(string sql)
        {
            using (var conn = CreateConnection(open: true))
            {
                var command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
            }
        }

        public void ExecuteBatch(string batchScript)
        {
            WithSmo(database =>
            {
                database.ExecuteNonQuery(batchScript);
            });
        }

        public void ExecuteBatch(IEnumerable<string> batch)
        {
            foreach (var sql in batch)
            {
                ExecuteBatch(sql);
            }
        }

        public IEnumerable<Dictionary<string, Object>> Query(string sql)
        {
            return QueryInternal(sql, new DictionaryDataMapper());
        }

        public IEnumerable<T> Query<T>(string sql)
            where T : new()
        {
            return QueryInternal(sql, new AutoDataMapper<T>());
        }

        public IEnumerable<T> Query<T>(string sql, IDataMapper<T> mapper)
        {
            return QueryInternal(sql, mapper);
        }

        public T QuerySingle<T>(string sql)
            where T : new()
        {
            return QueryInternal(sql, new AutoDataMapper<T>(), 1)
                .FirstOrDefault();
        }

        public Dictionary<string, Object> QuerySingle(string sql)
        {
            return QueryInternal(sql, new DictionaryDataMapper(), 1)
                .FirstOrDefault();
        }

        public T QuerySingle<T>(string sql, string column)
            where T : IConvertible
        {
            var result = QueryInternal(sql, new DictionaryDataMapper(), 1)
                .FirstOrDefault();

            if (result == null)
            {
                return default(T);
            }

            if (!result.ContainsKey(column))
            {
                throw new DatabaseException("Column not present in result set: " + column);
            }

            return (T)Convert.ChangeType(result[column], typeof(T));
        }

        public T QuerySingle<T>(string sql, IDataMapper<T> mapper)
        {
            return QueryInternal(sql, mapper, 1)
                .FirstOrDefault();
        }

        private SqlConnection CreateConnection(bool open = true)
        {
            var conn = new SqlConnection(connString);

            if (open)
            {
                conn.Open();
            }

            return conn;
        }

        private List<T> QueryInternal<T>(string sql, IDataMapper<T> mapper, int limit = 0)
        {
            using (var conn = CreateConnection())
            {
                var command = new SqlCommand(sql, conn);
                var reader = command.ExecuteReader();
                var results = new List<T>();

                while (reader.Read())
                {
                    results.Add(mapper.Map(reader));

                    if (limit > 0 && limit >= results.Count)
                    {
                        break;
                    }
                }

                return results;
            }
        }

        private void WithSmo(Action<SmoDatabase> action)
        {
            using (var conn = CreateConnection(open: false))
            {
                var server = new SmoServer(new SmoConnection(conn));
                var database = server.Databases[databaseName];

                action(database);
            }
        }
    }
}