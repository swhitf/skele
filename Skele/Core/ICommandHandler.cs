using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    public interface ICommandHandler<TCommand> 
        where TCommand : ICommand
    {
        int Execute(TCommand command);
    }
}
