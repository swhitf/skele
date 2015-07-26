using Skele.Interop;
using Skele.Migration;
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
        public CommandHandlerBase(ICommandContext context)
        {
            Context = context;
        }

        protected ICommandContext Context
        {
            get;
            private set;
        }

        protected TextWriter Log
        {
            get { return Context.Log; }
        }

        protected TextWriter Output
        {
            get { return Context.Output; }
        }

        public abstract int Execute(T input);

        protected virtual int CancelledResult(string message = null)
        {
            if (message != null)
            {
                Console.WriteLine(message);
            }

            return 1;
        }

        protected virtual int FailureResult(string message = null)
        {
            if (message != null)
            {
                Console.WriteLine(message);
            }

            return 2;
        }

        protected virtual int SuccessResult(string message = null)
        {
            if (message != null)
            {
                Console.WriteLine(message);
            }

            return 0;
        }
    }
}
