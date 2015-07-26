using Skele.Core;
using Skele.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.IO
{
    class DumpCommandHandler : CommandHandlerBase<DumpCommand>
    {

        public DumpCommandHandler(ICommandContext context)
            : base(context)
        {
        }

        public override int Execute(DumpCommand input)
        {
            var outProvider = CreateOutputProvider(input.OutputPath);
            var session = GetDatabaseSession();

            foreach (var table in input.TableList)
            {
                var sql = session.Build() 
                    .Query(table)
                    .SelectAll()
                    .ToSql();

                var results = session.Query(sql);
            }

            return SuccessResult();
        }

        private IEnumerable<DumpStrategy> CreatePlan(DumpCommand input)
        {
            foreach (var table in input.TableList)
            {
                yield return new DumpStrategy
                {
                    Table = table,
                };
            }
        }

        private OutputProvider CreateOutputProvider(string outputPath)
        {
            if (Directory.Exists(outputPath))
            {

            }

            throw new NotImplementedException();
        }

        private IDataFormatter GetFormatter(string type)
        {
            type = type.Trim().ToLower();

            switch (type)
            {
                case "csv":
                    return null;

                case "sql":
                    return null;

                default:
                    throw new NotSupportedException(string.Format("Format type \"{0}\" is not supported."));
            }
        }

        private delegate Stream OutputProvider(string table);

        private class DumpStrategy
        {
            public string Table;
            public Func<Stream> OutProvider;
        }
    }
}
