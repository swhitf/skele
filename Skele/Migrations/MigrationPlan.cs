using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migrations
{
    class MigrationPlan : IEnumerable<Migration>
    {
        public MigrationPlan(Version sourceVersion, IEnumerable<Migration> migrations)
        {
            SourceVersion = sourceVersion;
            Migrations = migrations.ToList();
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

        public IReadOnlyList<Migration> Migrations
        {
            get;
            private set;
        }

        public IEnumerator<Migration> GetEnumerator()
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
