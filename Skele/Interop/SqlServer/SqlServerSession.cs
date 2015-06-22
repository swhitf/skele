using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;
using IDataReader = System.Data.IDataReader;
using DataTable = System.Data.DataTable;
using DataRow = System.Data.DataRow;
using DataColumn = System.Data.DataColumn;
using System.ComponentModel;


namespace Skele.Interop.SqlServer
{
    class SqlServerSession : DatabaseSession
    {
        private Database database;

        public SqlServerSession(Database database)
        {
            this.database = database;
        }

        public override void CreateTable(TableDescriptor tableSpec)
        {
            var table = new Table(database, tableSpec.Name);

            foreach (var columnSpec in tableSpec.Columns)
            {
                var type = columnSpec.Length.HasValue
                    ? new DataType(TypeMappings.GetSmoType(columnSpec.DataType), columnSpec.Length.Value)
                    : new DataType(TypeMappings.GetSmoType(columnSpec.DataType));

                var column = new Column(table, columnSpec.Name, type);
                table.Columns.Add(column);
            }

            table.Create();
        }

        public override DatabaseDescriptor Describe()
        {
            return MetadataFactory.Create(database);
        }

        public override T QuerySingle<T>(string sql)
        {
            var data = QueryTable(sql);

            if (data.Rows.Count > 0)
            {
                return MapRowToObject<T>(data, data.Rows[0]);
            }
            
            return default(T);
        }

        public override dynamic QuerySingle(string sql)
        {
            throw new NotImplementedException();
        }

        private DataTable QueryTable(string sql)
        {
            var results = database.ExecuteWithResults(sql);
            return results.Tables[0];
        }

        private T MapRowToObject<T>(DataTable table, DataRow row)
            where T : new()
        {
            var props = TypeDescriptor.GetProperties(typeof(T))
                .Cast<PropertyDescriptor>()
                .ToDictionary(x => x.Name);

            return MapRowToObject<T>(table, row, props);
        }

        private T MapRowToObject<T>(DataTable table, DataRow row, Dictionary<string, PropertyDescriptor> props)
            where T : new()
        {
            var obj = new T();

            foreach (DataColumn col in table.Columns)
            {
                if (!props.ContainsKey(col.ColumnName))
                    continue;

                props[col.ColumnName].SetValue(obj, row[col.ColumnName]);
            }

            return obj;
        }

        public override void Execute(string sql)
        {
            database.ExecuteNonQuery(sql);
        }

        public override void ExecuteBatch(string batchScript)
        {
            database.ExecuteNonQuery(batchScript);
        }

        public override void ExecuteBatch(IEnumerable<string> batch)
        {
            foreach (var sql in batch)
            {
                ExecuteBatch(sql);
            }
        }
    }
}
