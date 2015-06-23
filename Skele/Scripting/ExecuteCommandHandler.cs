using Jint;
using Skele.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    class ExecuteCommandHandler : CommandHandlerBase<ExecuteCommand>
    {
        public override void Execute(ExecuteCommand input)
        {
            //Jint.Runtime.Interop.DefaultTypeConverter
            //var engine = new Engine();
            //var result = engine
            //    .
            //    .Execute("1 + 1")
            //    .GetCompletionValue()
            //    .AsNumber();
        }
    }
}
