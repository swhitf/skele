using Skele.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Util
{
    class HelloCommandHandler : ICommandHandler<HelloCommand>
    {
        private TextWriter log;

        public HelloCommandHandler(ICommandContext context)
        {
            log = context.Log;
        }

        public int Execute(HelloCommand command)
        {
            log.WriteLine("Hello, this is Skele speaking.");
            log.WriteLine();
            log.WriteLine("Created by Stephen Whitfield.");
            log.WriteLine("Version {0}", Assembly.GetExecutingAssembly().GetName().Version);
            log.WriteLine();

            return 0;
        }
    }
}
