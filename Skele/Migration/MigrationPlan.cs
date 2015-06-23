using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    class MigrationPlan : IEnumerable<MigrationStep>
    {
        public MigrationPlan(Version sourceVersion, IEnumerable<MigrationStep> migrations)
        {
            Migrations = migrations.ToList();
            SourceVersion = sourceVersion;
            TargetVersion = migrations.Max(x => x.Target);
        }

        public Version SourceVersion
        {
            get;
            private set;
        }

        public Version TargetVersion
        {
            get;
            private set;
        }

        public IReadOnlyList<MigrationStep> Migrations
        {
            get;
            private set;
        }

        public IEnumerator<MigrationStep> GetEnumerator()
        {
            return Migrations.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static MigrationPlan Build(Package pkg, Version sv, Version tv)
        {
            var migs = pkg.GetMigrations()
                .Where(x => x.Target > sv && x.Target <= tv)
                .OrderBy(x => x.Target)
                .ToList();

            return new MigrationPlan(sv, migs);
        }
    }
}
