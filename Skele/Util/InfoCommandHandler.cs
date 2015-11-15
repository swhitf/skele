using Skele.Core;
using Skele.Interop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Util
{
    class InfoCommandHandler : CommandHandlerBase<InfoCommand>
    {
        public InfoCommandHandler(ICommandContext context)
            : base(context)
        {
        }

        public override int Execute(InfoCommand input)
        {
            var mapper = DataMapperFactory.Create(x => new
            {
                V = x.String("V"),
                S = x.String("S"),
            });

            var session = Context.GetDatabaseSession();
            var info = session.QuerySingle("SELECT @@VERSION AS V, @@SERVERNAME AS S", mapper);

            Log.WriteLine("Version: {0}", info.V);
            Log.WriteLine("Server: {0}", info.S);

            return SuccessResult();
        }
    }
}
