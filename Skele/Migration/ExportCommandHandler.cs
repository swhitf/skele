using Skele.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    class ExportCommandHandler : CommandHandlerBase<ExportCommand>
    {
        private PackageFactory packages;
        private IVersionStrategy versioning;

        public ExportCommandHandler(ICommandContext context, PackageFactory packages, IVersionStrategy versioning)
            : base(context)
        {
            this.packages = packages;
            this.versioning = versioning;
        }

        public override int Execute(ExportCommand input)
        {
            var sv = input.TargetVersion;
            var tv = input.TargetVersion;
            var proj = Context.Project;

            var pkg = packages.Create(proj.Location ?? Environment.CurrentDirectory);
            
            if (sv == null)
            {
                sv = new Version(0, 0, 0, 0);
            }
            if (tv == null)
            {
                tv = pkg.GetMigrations().Max(x => x.Target);
            }

            Log.WriteLine("Exporting {0} to {1}...", sv, tv);

            var plan = MigrationPlan.Build(pkg, sv, tv);

            if (plan.Migrations.Count == 0)
            {
                Log.WriteLine("Terminating export due to 0 applicable migrations.");
                return FailureResult();
            }

            Log.WriteLine("Found {0} migrations.", plan.Migrations.Count);

            if (plan.Any())
            {
                versioning.PreScript(Output);

                foreach (var migration in plan)
                {
                    Log.WriteLine("Exporting {0}:", migration.Target);

                    versioning.PreMigration(Output, migration);

                    foreach (var resource in migration)
                    {
                        Log.WriteLine("  {0}", resource.Name);

                        Output.WriteLine("PRINT '{0}'", resource.Name);
                        Output.WriteLine(resource.ReadFull());
                        Output.WriteLine("GO");
                    }

                    versioning.PostMigration(Output, migration);
                }

                versioning.PostScript(Output);
            }

            return SuccessResult();
        }
    }
}
