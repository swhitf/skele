using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop
{
    public class SqlInsertBuilder : SqlGenerator
    {
        private List<string> columns;
        private List<Object> values;
        private string table;
        private SqlDialect dialect;

        public SqlInsertBuilder(SqlDialect dialect, string table)
        {
            this.dialect = dialect;
            this.table = table;

            columns = new List<string>();
            values = new List<Object>();
        }

        public virtual SqlInsertBuilder Columns(params string[] columns)
        {
            this.columns.AddRange(columns);
            return this;
        }

        public virtual SqlInsertBuilder Values(params Object[] values)
        {
            this.values.AddRange(values);
            return this;
        }

        public override string ToSql()
        {
            var tabStr = dialect.EscapeObjectIdentifier(table);

            var colStr = string.Join(
                ", ", columns.Distinct().Select(dialect.EscapeObjectIdentifier));
            
            var valStr = string.Join(
                ", ", values.Distinct().Select(dialect.FormatLiteral));

            return string.Format(
                "INSERT INTO {0} ({1}) VALUES ({2})",
                tabStr,
                colStr,
                valStr);                
        }
    }
}
