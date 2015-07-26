using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    public interface ICommandRegistry
    {
        ICommandHandler<T> ResolveHandler<T>() where T : ICommand;

        bool Contains<T>() where T : ICommand;
    }
}
