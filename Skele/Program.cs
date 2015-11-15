using Autofac;
using Autofac.Integration.Mef;
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
using System.ComponentModel.Composition;
using System.Reflection;
using System.ComponentModel.Composition.Hosting;
using Autofac.Core;

namespace Skele
{
    class Program
    {
        public static int Main(string[] args)
        {
            return new Program().Run(args);
        }

        private string projectLocationParam;
        private string projectTargetParam;
        private TextWriter outputWriter;
        private TextWriter logWriter;

        public Program()
        {
            projectLocationParam = Environment.CurrentDirectory;
            projectTargetParam = "default";
            logWriter = Console.Out;
        }

        private int Run(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Interactive mode:");
                Console.Write("> ");
                string input = Console.ReadLine();
                while (input != "exit")
                {
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine();
                    }
                    else
                    {
                        Run(Win32.CommandLineToArgs(input + " /np"));
                        Console.WriteLine();
                        Console.Write("> ");
                        input = Console.ReadLine();
                    }
                }

                return 0;
            }
            else
            {
                var noPrompt = false;
                var container = ComposeContainer();
                var input = new InputAdapter(container.Resolve<ICommandDispatcher>());

                input.Filter()
                    .FlipSwitch("np", "noprompt", d => noPrompt = true)
                    .FlipSwitch("q", "quiet", d => logWriter = TextWriter.Null)
                    .FlipSwitch("e", "echo", d => outputWriter = Console.Out)
                    .ValueSwitch("l", "log", (d, v) => logWriter = new StreamWriter(File.Open(v, FileMode.Create)))
                    .ValueSwitch("o", "output", (d, v) => outputWriter = new StreamWriter(File.Open(v, FileMode.Create)))
                    .ValueSwitch("p", "project", (d, v) => projectLocationParam = v)
                    .ValueSwitch("t", "target", (x, v) => projectTargetParam = v);

                input.Map<HelloCommand>("hello");

                input.Map<PrepareCommand>("prepare")
                    .Init(x => x.DirectoryPath = Environment.CurrentDirectory)
                    .Arg(0, (x, v) => x.DirectoryPath = v);

                input.Map<InitCommand>("init");

                input.Map<InfoCommand>("info");

                input.Map<ExecuteCommand>("exec")
                    .Arg(0, (x, v) => x.FilePath = v);
                input.Map<ExecuteCommand>("execute")
                    .Arg(0, (x, v) => x.FilePath = v);

                input.Map<ExportCommand>("export")
                    .ValueSwitch("sv", (x, v) => x.SourceVersion = Version.Parse(v))
                    .ValueSwitch("tv", (x, v) => x.TargetVersion = Version.Parse(v));

                input.Map<JavaScriptStubCommand>("js-stub")
                    .Arg(0, (x, v) => x.TableName = v);

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

        private IContainer ComposeContainer()
        {
            var composer = new ContainerBuilder();

            //Register all local command handlers:
            composer
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => ImplementsGenericInterface(typeof(ICommandHandler<>), x))
                .AsImplementedInterfaces();

            //Register autofac command registry
            composer
                .RegisterType<AutofacCommandRegistry>()
                .As<ICommandRegistry>();

            //Register command dispatcher
            composer
                .RegisterType<DefaultCommandDispatcher>()
                .As<ICommandDispatcher>()
                .Exported(x => x.As<ICommandDispatcher>());

            //Register integrated SqlServer database driver
            composer
                .RegisterType<SqlServerDriver>()
                .Named<IDatabaseDriver>("SqlServer")
                .Exported(x => x.As<IDatabaseDriver>());

            //Register type factory for database drivers
            composer
                .RegisterType<AutofacNamedTypeFactory<IDatabaseDriver>>()
                .As<INamedTypeFactory<IDatabaseDriver>>()
                .Exported(x => x.As<INamedTypeFactory<IDatabaseDriver>>());

            //Register UI service
            composer
                .RegisterType<ConsolePresenter>()
                .As<IPresenter>()
                .Exported(x => x.As<IPresenter>());

            //Register package factory
            composer
                .RegisterType<PackageFactory>()
                .Exported(x => x.As<PackageFactory>());

            //Register command context
            composer
                .RegisterType<DefaultCommandContext>()
                .As<ICommandContext>()
                .Exported(x => x.As<ICommandContext>())
                .OnActivated(DecorateCommandContext);

            //Register version strategy
            composer
                .RegisterType<DefaultVersionStrategy>()
                .As<IVersionStrategy>()
                .Exported(x => x.As<IVersionStrategy>());

            //Register extensions
            //composer
            //    .RegisterComposablePartCatalog(new DirectoryCatalog(
            //        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ext")));

            return composer.Build();
        }

        private void DecorateCommandContext(IActivatedEventArgs<DefaultCommandContext> e)
        {
            var cc = e.Instance;
            cc.Project = Project.Load(projectLocationParam);
            cc.SetTarget(projectTargetParam);
            cc.Output = outputWriter ?? TextWriter.Null;
            cc.Log = logWriter ?? TextWriter.Null;
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

        private static bool ImplementsGenericInterface(Type g, Type t)
        {
            return t.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == g);
        }
    }
}
