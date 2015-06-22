using Skele.Core;
using Skele.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migrations
{
    class MigrateCommandHandler : CommandHandlerBase<MigrateCommand>
    {
        private DatabaseDriver driver;

        public MigrateCommandHandler(
            DatabaseDriver driver)
        {
            this.driver = driver;
        }

        public override void Execute(MigrateCommand input)
        {
            var pkg = input.Package;
            var sv = input.TargetVersion;
            var tv = input.TargetVersion;

            var session = driver.Open("Katana");

            if (sv == null)
            {
                sv = DetectVersion(session) ?? new Version(0, 0);
            }

            var plan = MigrationPlan.Build(pkg, sv, tv);

            foreach (var migration in plan)
            {
                Log.WriteLine("Migrating to " + migration.Target);

                foreach (var r in migration)
                {
                    Log.WriteLine("  " + r.Name);
                    session.ExecuteBatch(r.ReadFull());
                }

                Console.WriteLine("version: " + migration.Target);
            }
        }

        private Version DetectVersion(DatabaseSession session)
        {
            var sql =
                "SELECT TOP 1 Major, Minor, Patch AS Revision, 0 AS Build " +
                "FROM __VERSION ORDER BY Major, Minor, Patch DESC";

            var version = session.QuerySingle<Version>(sql);

            if (version != null)
            {
                Log.WriteLine("Found version {0}", version);
            }

            return version;
        }
    }
}
