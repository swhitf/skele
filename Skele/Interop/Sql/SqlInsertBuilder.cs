using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Skele.Interop.Sql
{
    public class SqlInsertBuilder : SqlGenerator
    {
        private const string Template = "INSERT INTO {0} ({1}) VALUES ({2})";

        public SqlInsertBuilder(ISqlDialect dialect, string table)
        {
            Dialect = dialect;
            Table = table;
            ColumnList = new List<string>();
            ValueList = new List<Object>();
        }

        protected SqlInsertBuilder()
        {

        }

        protected string Table
        {
            get;
            set;
        }

        protected List<string> ColumnList
        {
            get;
            set;
        }

        protected List<Object> ValueList
        {
            get;
            set;
        }

        protected ISqlDialect Dialect
        {
            get;
            set;
        }

        public virtual SqlInsertBuilder Columns(params string[] columns)
        {
            return Columns((IEnumerable<string>)columns);
        }

        public virtual SqlInsertBuilder Columns(IEnumerable<string> columns)
        {
            ColumnList.AddRange(columns);
            return this;
        }

        public virtual SqlInsertBuilder Values(params Object[] values)
        {
            return Values((IEnumerable<Object>)values);
        }

        public virtual SqlInsertBuilder Values(IEnumerable<Object> values)
        {
            ValueList.AddRange(values);
            return this;
        }

        public override string ToSql()
        {
            return string.Format(
                Template,
                Table,
                string.Join(", ", ColumnList.Select(Dialect.EscapeObjectName)),
                string.Join(", ", ValueList.Select(Dialect.FormatLiteralValue))
            );
        }

        public virtual SqlInsertBuilder Data(Object data)
        {
            var columns = new List<string>();
            var values = new List<Object>();

            var properties = TypeDescriptor.GetProperties(data);
            foreach (PropertyDescriptor p in properties)
            {
                columns.Add(p.Name);
                values.Add(p.GetValue(data));
            }

            return Columns(columns).Values(values);
        }
    }
}
