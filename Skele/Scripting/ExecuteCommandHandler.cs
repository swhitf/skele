using Jint;
using Skele.Core;
using Skele.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    class ExecuteCommandHandler : CommandHandlerBase<ExecuteCommand>
    {

        public ExecuteCommandHandler(ICommandContext context)
            : base(context)
        {
        }

        public override int Execute(ExecuteCommand input)
        {
            var session = GetDatabaseSession();
            var metadata = session.Describe();
            var engine = new Engine();

            foreach (var table in metadata.Tables)
            {
                engine.SetValue(table.Name, new TableAdapter(engine, session, table));
            }

            var source = File.ReadAllText(input.FilePath);
            engine.Execute(source);

            //Jint.Runtime.Interop.DefaultTypeConverter
            //var engine = new Engine();
            //var result = engine
            //    .
            //    .Execute("1 + 1")
            //    .GetCompletionValue()
            //    .AsNumber();

            return SuccessResult();
        }
    }
}
