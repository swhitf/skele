using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    abstract class CommandHandlerBase<T> : ICommandHandler<T>
        where T : ICommand
    {
        public CommandHandlerBase(TextWriter log = null, TextWriter output = null)
        {
            Log = log ?? TextWriter.Null;
            Output = output ?? TextWriter.Null;
        }

        protected TextWriter Log { get; private set; }

        protected TextWriter Output { get; private set; }

        public abstract void Execute(T command);
    }
}
