using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    class AutofacNamedTypeFactory<T> : INamedTypeFactory<T>
    {
        private IContainer container;

        public AutofacNamedTypeFactory(IContainer container)
        {
            this.container = container;
        }

        public T Create(string name)
        {
            return container.ResolveNamed<T>(name);
        }
    }
}
