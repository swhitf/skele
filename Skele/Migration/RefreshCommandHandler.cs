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
        public RefreshCommandHandler(ICommandContext context)
            : base(context)
        {

        }

        public override int Execute(RefreshCommand input)
        {
            var target = GetTarget();
            var manager = GetDatabaseManager();
            
            if (manager.Exists(target.Database))
            {
                Log.WriteLine("Dropping {0}...", target.Database);
                manager.Delete(target.Database);
            }

            if (Dispatcher.Dispatch(new InitCommand()) != 0)
            {
                return FailureResult();
            }

            if (Dispatcher.Dispatch<MigrateCommand>(input) != 0)
            {
                return FailureResult();
            }

            return SuccessResult();
        }
    }
}
