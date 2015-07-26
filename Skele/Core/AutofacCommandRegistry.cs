using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    class AutofacCommandRegistry : ICommandRegistry
    {
        private IContainer container;
        public AutofacCommandRegistry(IContainer container)
        {
            this.container = container;
        }

        public ICommandHandler<T> ResolveHandler<T>() where T : ICommand
        {
            return container.Resolve<ICommandHandler<T>>();
        }

        public bool Contains<T>() where T : ICommand
        {
            return container.IsRegistered<ICommandHandler<T>>();
        }
    }
}
