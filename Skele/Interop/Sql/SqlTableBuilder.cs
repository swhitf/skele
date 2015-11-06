using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.Sql
{
    public class SqlTableBuilder : SqlGenerator
    {
        private string table;
        private List<ColumnDef> columns;

        public SqlTableBuilder(string table)
        {
            this.table = table;

            columns = new List<ColumnDef>();
        }

        public SqlTableBuilder Column(string name, string type, bool nullable = true, object features = null)
        {
            columns.Add(new ColumnDef
            {
                Name = name,
                Type = type,
                Nullable = nullable,
                Features = features,
            });

            return this;
        }

        public override string ToSql()
        {
            var components = columns.Select(Transform);

            var sql = new StringBuilder();
            sql.AppendLine("CREATE TABLE {0} (");
            sql.Append(string.Join("," + Environment.NewLine, components));
            sql.AppendLine(")");
            return sql.ToString();
        }

        private string Transform(ColumnDef cd)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("{0} ", cd.Name);
            sql.AppendFormat("{0} ", cd.Type);
            sql.AppendFormat("{0}NULL ", cd.Nullable ? "" : "NOT ");
            return sql.ToString();
        }

        private class ColumnDef
        {
            public string Name;
            public string Type;
            public bool Nullable;
            public object Features;
        }
    }
}
