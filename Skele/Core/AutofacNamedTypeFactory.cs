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
        private IComponentContext context;

        public AutofacNamedTypeFactory(IComponentContext context)
        {
            this.context = context;
        }

        public T Create(string name)
        {
            return context.ResolveNamed<T>(name);
        }
    }
}
