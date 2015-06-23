using Skele.Core;
using Skele.Interop;
using Skele.Interop.SqlServer;
using Skele.Migration;
using Skele.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        private DatabaseDriver driver;
        private InputMapper input;

        public Program()
        {
            var packages = new PackageFactory();
            var pkg = packages.Create("..\\..\\");

            driver = new SqlServerDriver();

            var dispatcher = new DefaultCommandDispatcher();
            dispatcher.Register(new InitCommandHandler(driver));
            dispatcher.Register(new ExecuteCommandHandler(driver));
            dispatcher.Register(new ExportCommandHandler());
            dispatcher.Register(new MigrateCommandHandler(driver));

            input = new InputMapper(dispatcher);
            input.Map<InitCommand>("init");
            input.Map<ExecuteCommand>("execute");
            input.Map<ExportCommand>("export")
                .Init(x => x.Package = pkg)
                .Arg("tv", (x, v) => x.TargetVersion = Version.Parse(v));
            input.Map<MigrateCommand>("migrate")
                .Init(x => x.Package = pkg)
                .Arg("tv", (x, v) => x.TargetVersion = Version.Parse(v));
        }

        public void Run(string[] args)
        {
#if DEBUG
            if (args.Length == 0)
            {
                args = new[] { "execute" };
            }
#endif
            input.Process(args);
        }
    }
}
