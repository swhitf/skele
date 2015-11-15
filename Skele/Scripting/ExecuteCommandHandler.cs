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
            var session = Context.GetDatabaseSession();
            var metadata = session.Describe();
            var engine = new JavaScriptEngine();

            var defaults = new Defaults();
            engine.Set("Default", new DefaultsAdapter(defaults));
            engine.Set("Session", new SessionAdapter(session));

            foreach (var table in metadata.Tables)
            {
                engine.Set(table.Name, new TableAdapter(session, table, defaults));
            }

            var source = File.ReadAllText(input.FilePath);
            engine.Execute(source);

            return SuccessResult();
        }
    }
}
