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

        public RefreshCommandHandler(ICommandContext context, ICommandDispatcher dispatcher)
            : base(context)
        {
            this.dispatcher = dispatcher;
        }

        public override int Execute(RefreshCommand input)
        {
            var target = Context.ActiveTarget;
            var manager = Context.GetDatabaseManager();

            if (manager.Exists(target.Database))
            {
                Log.WriteLine("Dropping {0}...", target.Database);
                manager.Delete(target.Database);
            }

            if (dispatcher.Dispatch(new InitCommand()) != 0)
            {
                return FailureResult();
            }

            if (dispatcher.Dispatch<MigrateCommand>(input) != 0)
            {
                return FailureResult();
            }

            return SuccessResult();
        }
    }
}
