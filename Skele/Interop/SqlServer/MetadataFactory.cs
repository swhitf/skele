using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmoDatabase = Microsoft.SqlServer.Management.Smo.Database;
using SmoTable = Microsoft.SqlServer.Management.Smo.Table;
using SmoColumn = Microsoft.SqlServer.Management.Smo.Column;

namespace Skele.Interop.SqlServer
{
    static class MetadataFactory
    {
        public static DatabaseDescriptor Create(SmoDatabase source)
        {
            var database = new DatabaseDescriptor();
            database.Name = source.Name;

            foreach (SmoTable t in source.Tables)
            {
                var table = new TableDescriptor();
                table.Name = t.Name;

                foreach (SmoColumn c in t.Columns)
                {
                    var column = new ColumnDescriptor();
                    column.Name = c.Name;
                    column.Nullable = c.Nullable;
                    column.DataType = TypeMappings.GetClrType(c.DataType.SqlDataType);
                    var dt = c.DataType;

                    table.Columns.Add(column);
                }

                database.Tables.Add(table);
            }

            return database;
        }
    }
}
