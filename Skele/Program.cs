using Skele.Core;
using Skele.Interop;
using Skele.Interop.SqlServer;
using Skele.Migration;
using Skele.Scripting;
using Skele.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele
{
    class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Interactive mode:");
                string input = Console.ReadLine();
                while (input != "exit")
                {
                    Main(Win32.CommandLineToArgs(input));
                    input = Console.ReadLine();
                }

                return 0;
            }
            else
            {
                var dispatcher = new DefaultCommandDispatcher();

                var noPrompt = false;
                var context = new DefaultCommandContext
                {
                    DatabaseManagerFactory = new SqlServerManagerFactory(),
                    Dispatcher = dispatcher,
                    Log = Console.Out,
                    Output = Console.Out,
                    Packages = new PackageFactory(),
                    Presenter = new ConsolePresenter(),
                    Project = Project.TryLoad(Environment.CurrentDirectory),
                };

                dispatcher.Register(() => new HelloCommandHandler(context));
                dispatcher.Register(() => new PrepareCommandHandler(context));
                dispatcher.Register(() => new InitCommandHandler(context));
                dispatcher.Register(() => new ExecuteCommandHandler(context));
                dispatcher.Register(() => new ExportCommandHandler(context));
                dispatcher.Register(() => new MigrateCommandHandler(context));
                dispatcher.Register(() => new RefreshCommandHandler(context));

                var input = new InputAdapter<DefaultCommandDispatcher>(dispatcher);

                input.Filter()
                    .FlipSwitch("dbg", "debug", d => System.Diagnostics.Debugger.Launch())
                    .FlipSwitch("np", "noprompt", d => noPrompt = true)
                    .FlipSwitch("q", "quiet", d => context.Log = TextWriter.Null)
                    .ValueSwitch("l", "log", (d, v) => context.Log = new StreamWriter(File.Open(v, FileMode.Create)))
                    .ValueSwitch("o", "output", (d, v) => context.Output = new StreamWriter(File.Open(v, FileMode.Create)))
                    .ValueSwitch("p", "project", (d, v) => context.Project = Project.Load(v))
                    .ValueSwitch("t", "target", (x, v) => context.TargetName = v);

                input.Map<HelloCommand>("hello");

                input.Map<PrepareCommand>("prepare")
                    .Init(x => x.DirectoryPath = Environment.CurrentDirectory)
                    .Arg(0, (x, v) => x.DirectoryPath = v);

                input.Map<InitCommand>("init");

                input.Map<ExecuteCommand>("execute");

                input.Map<ExportCommand>("export")
                    .ValueSwitch("sv", (x, v) => x.SourceVersion = Version.Parse(v))
                    .ValueSwitch("tv", (x, v) => x.TargetVersion = Version.Parse(v));

                input.Map<MigrateCommand>("migrate")
                    .ValueSwitch("tv", "targetversion", (x, v) => x.TargetVersion = Version.Parse(v));

                input.Map<RefreshCommand>("refresh")
                    .ValueSwitch("tv", "targetversion", (x, v) => x.TargetVersion = Version.Parse(v));

                try
                {
                    return input.Apply(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(FormatException(ex));
                    return 1;
                }
                finally
                {
                    if (!noPrompt)
                    {
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                    }
                }
            }
        }

        private static string FormatException(Exception ex)
        {
            var message = new StringBuilder();
            message.AppendLine("Fatal Error:");
            message.AppendLine(ex.Message);

            var cex = ex.InnerException;
            while (cex != null)
            {
                message.AppendLine("  -> " + cex.Message);
                cex = cex.InnerException;
            }

            message.AppendLine("Trace:");
            message.AppendLine(ex.StackTrace);

            return message.ToString();
        }
    }
}
