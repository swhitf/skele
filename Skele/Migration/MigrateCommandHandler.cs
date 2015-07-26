using Skele.Core;
using Skele.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    class MigrateCommandHandler : CommandHandlerBase<MigrateCommand>
    {
        public MigrateCommandHandler(ICommandContext context)
            : base(context)
        {
        }

        public override int Execute(MigrateCommand input)
        {
            var sv = input.TargetVersion;
            var tv = input.TargetVersion;
            var pkg = new PackageFactory().Create(Project);
            var session = GetDatabaseSession();

            if (sv == null)
            {
                sv = DetectVersion(session) ?? new Version(0, 0);
            }
            if (tv == null)
            {
                tv = pkg.GetMigrations().Max(x => x.Target);
            }

            var plan = MigrationPlan.Build(pkg, sv, tv);

            foreach (var migration in plan)
            {
                Log.WriteLine("Migrating to " + migration.Target);

                foreach (var resource in migration)
                {
                    Log.WriteLine("  " + resource.Name);
                    session.ExecuteBatch(resource.ReadFull());
                }

                SignVersion(session, migration.Target);
                Log.WriteLine("Version: " + migration.Target);
            }

            return SuccessResult();
        }

        private void SignVersion(IDatabaseSession session, Version version)
        {
            var sql = session.Build()
                .Insert("__Version")
                .Data(new
                {
                    Major = version.Major,
                    Minor = version.Minor,
                    Revision = version.Revision,
                    InstallDate = DateTime.UtcNow,
                })
                .ToSql();
            Console.WriteLine(sql);
            session.Execute(sql);
        }
        
        private Version DetectVersion(IDatabaseSession session)
        {
            var sql = session.Build()
                .Query("__Version")
                .Select("Major", "Minor", "Patch|Revision")
                .OrderBy("Major|DESC", "Minor|DESC", "Patch|DESC")
                .Limit(1)
                .ToSql();

            var version = session.QuerySingle<Version>(sql);

            if (version != null)
            {
                Log.WriteLine("Found version {0}", version);
            }

            return version;
        }
    }
}
