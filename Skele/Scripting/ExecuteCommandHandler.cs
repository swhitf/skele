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
        private DatabaseDriver driver;

        public ExecuteCommandHandler(DatabaseDriver driver)
        {
            this.driver = driver;
        }

        public override void Execute(ExecuteCommand input)
        {
            var session = driver.Open("Katana");
            var metadata = session.Describe();

            var engine = new Engine();
            AddTableAdapters(engine, metadata.Tables);

            var source = File.ReadAllText(input.FilePath);
            

            //Jint.Runtime.Interop.DefaultTypeConverter
            //var engine = new Engine();
            //var result = engine
            //    .
            //    .Execute("1 + 1")
            //    .GetCompletionValue()
            //    .AsNumber();
        }

        private void AddTableAdapters(Engine engine, IEnumerable<TableDescriptor> tables)
        {
            foreach (var t in tables)
            {
                engine.SetValue(t.Name, new TableAdapter(t));
            }
        }
    }
}
