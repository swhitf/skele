using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    class DefaultCommandDispatcher : ICommandDispatcher
    {
        private Dictionary<Type, Func<Object>> factories;

        public DefaultCommandDispatcher()
        {
            factories = new Dictionary<Type, Func<Object>>();
        }

        public void Register<T>(Func<ICommandHandler<T>> factory)
            where T : ICommand
        {
            factories.Add(typeof(T), factory);
        }

        public int Dispatch<T>(T command) where T : ICommand
        {
            var type = typeof(T);

            if (factories.ContainsKey(type))
            {
                var factory = factories[type];
                var handler = (ICommandHandler<T>)factory();

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
