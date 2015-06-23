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
        public ExportCommandHandler()
        {

        }

        public override void Execute(ExportCommand input)
        {
            var pkg = input.Package;
            var sv = input.TargetVersion;
            var tv = input.TargetVersion;

            Log.WriteLine("Exporting {0} to {1}...", sv, tv);

            var plan = MigrationPlan.Build(pkg, sv, tv);

            if (plan.Migrations.Count == 0)
            {
                Log.WriteLine("Terminating export due to 0 applicable migrations.");
                return;
            }

            Log.WriteLine("Found {0} migrations.", plan.Migrations.Count);

            foreach (var migration in plan)
            {
                Log.WriteLine("Exporting {0}:", migration.Target);

                foreach (var resource in migration)
                {
                    Log.WriteLine("  {0}", resource.Name);

                    Output.WriteLine("PRINT '{0}'", resource.Name);
                    Output.WriteLine(resource.ReadFull());
                    Output.WriteLine("GO");
                }

                Output.WriteLine("INSERT INTO __Version(Major, Minor, Patch), InstallDate)");
                Output.WriteLine("VALUES({0}, {1}, {2}, GETUTCDATE())",
                    migration.Target.Major,
                    migration.Target.Minor,
                    migration.Target.Revision);
            }
        }
    }
}
