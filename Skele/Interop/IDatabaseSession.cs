using Skele.Interop.Mapping;
using Skele.Interop.MetaModel;
using Skele.Interop.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop
{
    public interface IDatabaseSession
    {
        ISqlDialect Dialect { get; }

        SqlBuilder Build();

        void CreateTable(TableDescriptor table);

        DatabaseDescriptor Describe();

        void Execute(string sql);

        void ExecuteBatch(string batchScript);

        void ExecuteBatch(IEnumerable<string> batch);

        IEnumerable<Dictionary<string, Object>> Query(string sql);

        IEnumerable<T> Query<T>(string sql)
            where T : new();

        IEnumerable<T> Query<T>(string sql, IDataMapper<T> mapper);

        T QuerySingle<T>(string sql)
            where T : new();

        T QuerySingle<T>(string sql, IDataMapper<T> mapper);

        Dictionary<string, Object> QuerySingle(string sql);

        T QuerySingle<T>(string sql, string column)
            where T : IConvertible;
    }
}
