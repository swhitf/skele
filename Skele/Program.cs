using Skele.Core;
using Skele.Input;
using Skele.Interop;
using Skele.Interop.SqlServer;
using Skele.Migrations;
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
            
        }

        private DatabaseDriver driver;
        private DefaultCommandDispatcher dispatcher;
        private InputMapper input;

        public Program()
        {
            var packages = new PackageFactory();
            var pkg = packages.Create("..\\..\\");

            driver = new SqlServerDriver();

            dispatcher = new DefaultCommandDispatcher();
            dispatcher.Register(new ExportCommandHandler());
            dispatcher.Register(new MigrateCommandHandler(driver));
            dispatcher.Register(new InitCommandHandler(driver));

            input = new InputMapper();
            input.Map<ExportCommand>("export")
                .Arg("tv", (x, v) => x.TargetVersion = Version.Parse(v))
                .Init(x => x.Package = pkg);
        }

        public void Run(string[] args)
        {
            
        }
    }
}
