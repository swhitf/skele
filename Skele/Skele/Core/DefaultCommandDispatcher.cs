using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    class DefaultCommandDispatcher : ICommandDispatcher
    {
        private Dictionary<Type, Object> handlers;

        public DefaultCommandDispatcher()
        {
            handlers = new Dictionary<Type, Object>();
        }

        public void Register<T>(ICommandHandler<T> handler)
            where T : ICommand
        {
            handlers.Add(typeof(T), handlers);
        }

        public void Dispatch<T>(T command) where T : ICommand
        {
            var type = command.GetType();

            if (handlers.ContainsKey(type))
            {
                var handler = handlers[type] as ICommandHandler<T>;

                if (handler == null)
                {
                    throw new ApplicationException(string.Format("Invalid handler registered for command: {0}", type.Name));
                }

                handler.Execute(command);
            }

            throw new InvalidOperationException(string.Format("Unrecognized command: {0}", type.Name));
        }
    }
}
