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
        private IComponentContext context;

        public AutofacCommandRegistry(IComponentContext context)
        {
            this.context = context;
        }

        public ICommandHandler<T> ResolveHandler<T>() where T : ICommand
        {
            return context.Resolve<ICommandHandler<T>>();
        }

        public bool Contains<T>() where T : ICommand
        {
            return context.IsRegistered<ICommandHandler<T>>();
        }
    }
}
