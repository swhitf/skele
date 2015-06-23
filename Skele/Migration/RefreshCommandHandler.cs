using Skele.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    class RefreshCommandHandler : CommandHandlerBase<RefreshCommand>
    {
        private ICommandDispatcher dispatcher;

        public RefreshCommandHandler(ICommandDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public override void Execute(RefreshCommand input)
        {
            dispatcher.Dispatch(new InitCommand());


        }
    }
}
