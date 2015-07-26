using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    class DefaultCommandDispatcher : ICommandDispatcher
    {
        private ICommandRegistry register;

        public DefaultCommandDispatcher(ICommandRegistry register)
        {
            this.register = register;
        }

        public int Dispatch<T>(T command) where T : ICommand
        {
            var type = typeof(T);

            if (register.Contains<T>())
            {
                var handler = register.ResolveHandler<T>();

                if (handler == null)
                {
                    throw new ApplicationException(string.Format("Invalid handler registered for command: {0}", type.Name));
                }

                return handler.Execute(command);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Unrecognized command: {0}", type.Name));
            }
        }
    }
}
