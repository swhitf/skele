using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmoDatabase = Microsoft.SqlServer.Management.Smo.Database;
using SmoTable = Microsoft.SqlServer.Management.Smo.Table;
using SmoColumn = Microsoft.SqlServer.Management.Smo.Column;
using Skele.Interop.MetaModel;
using Skele.Interop.Mapping;

namespace Skele.Interop.SqlServer
{
    static class MetadataFactory
    {
        public static DatabaseDescriptor Create(IDatabaseSession session)
        {
            var sql = session.Build()
                .Query("INFORMATION_SCHEMA.SCHEMATA")
                .Distinct()
                .Select("CATALOG_NAME")
                .ToSql();

            var name = session.QuerySingle<string>(sql, "CATALOG_NAME");
            var tables = FetchTables(session);
            ApplyColumnData(session, tables);
            ApplyPrimaryKeyData(session, tables);

            var dbObj = new DatabaseDescriptor();
            dbObj.Name = name;
            dbObj.Tables.AddRange(tables);

            return dbObj;
        }

        private static void ApplyColumnData(IDatabaseSession session, List<TableDescriptor> tables)
        {
            var tableLookup = tables
                .ToDictionary(x => x.Name);

            var sql = session.Build()
                .Query("INFORMATION_SCHEMA.COLUMNS")
                .Select("TABLE_NAME", "COLUMN_NAME", "COLUMN_DEFAULT", "IS_NULLABLE", "DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH")
                .ToSql();

            var mapper = DataMapperFactory.Create(x =>
            {
                var tn = x.String("TABLE_NAME");
                var cd = new ColumnDescriptor
                {
                    Name = x.String("COLUMN_NAME"),
                    DefaultValue = x.String("COLUMN_DEFAULT"),
                    Nullable = ConvertFromAnsiBool(x.String("IS_NULLABLE")),
                    DataType = ConvertFromAnsiDataType(x.String("DATA_TYPE")),
                    Length = x.Int("CHARACTER_MAXIMUM_LENGTH"),
                };

                tableLookup[tn].Columns.Add(cd);
                return cd;
            });

            session.Query(sql, mapper).ToList();
        }

        private static void ApplyPrimaryKeyData(IDatabaseSession session, List<TableDescriptor> tables)
        {
            var sql = session.Build()
                .Query("INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE u")
                .InnerJoin("INFORMATION_SCHEMA.TABLE_CONSTRAINTS c", x => x
                    .On("c.TABLE_CATALOG = u.TABLE_CATALOG")
                    .And("c.TABLE_SCHEMA = u.TABLE_SCHEMA")
                    .And("c.TABLE_NAME = u.TABLE_NAME")
                )
                .Select("u.TABLE_CATALOG", "u.TABLE_SCHEMA", "u.TABLE_NAME", "u.COLUMN_NAME")
                .Filter(x =>
                    x.Where("CONSTRAINT_TYPE = 'PRIMARY KEY'")
                )
                .ToSql();

            var mapper = DataMapperFactory.Create(x => new
            {
                Catalog = x.String("TABLE_CATALOG"),
                Schema = x.String("TABLE_SCHEMA"),
                Table = x.String("TABLE_NAME"),
                Column = x.String("COLUMN_NAME"),
            });

            var keys = session
                .Query(sql, mapper)
                .GroupBy(x => new
                {
                    x.Catalog,
                    x.Schema,
                    x.Table,
                });

            foreach (var group in keys)
            {
                var table = tables.FirstOrDefault(x => x.Name == group.Key.Table);
                table.PrimaryKey = new PrimaryKeyDescriptor();
                table.PrimaryKey.Columns.AddRange(
                    group.Select(x => table.Columns[x.Column])
                );
            }
        }

        private static bool ConvertFromAnsiBool(string value)
        {
            return value == "YES";
        }

        private static Type ConvertFromAnsiDataType(string type)
        {
            switch (type.ToUpper())
            {
                case "BIGINT":
                    return typeof(long);
                case "BINARY":
                    return typeof(byte[]);
                case "BIT":
                case "BOOLEAN":
                    return typeof(bool);
                case "DATE":
                case "DATETIME":
                case "TIME":
                case "TIMESTAMP":
                    return typeof(DateTime);
                case "DATETIMEOFFSET":
                    return typeof(DateTimeOffset);
                case "DECIMAL":
                case "MONEY":
                case "NUMERIC":
                    return typeof(decimal);
                case "FLOAT":
                    return typeof(float);
                case "INT":
                case "INTEGER":
                case "SMALLINT":
                    return typeof(int);
                case "HIERARCHYID":
                case "NVARCHAR":
                case "VARBINARY":
                case "VARCHAR":
                    return typeof(string);

                default:
                    throw new InvalidOperationException("Unsupported data type: " + type);
            }
        }

        private static List<TableDescriptor> FetchTables(IDatabaseSession session)
        {
            var sql = session.Build()
                .Query("INFORMATION_SCHEMA.TABLES")
                .Select("TABLE_CATALOG", "TABLE_SCHEMA", "TABLE_NAME", "TABLE_TYPE")
                .ToSql();

            var mapper = DataMapperFactory.Create(x => new TableDescriptor
            {
                Name = x.String("TABLE_NAME"),
                Schema = x.String("TABLE_SCHEMA"),
            });

            return session.Query(sql, mapper).ToList();
        }
    }

}
