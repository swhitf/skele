using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    public abstract class Package
    {
        public abstract Uri Location
        {
            get;
        }

        public abstract IReadOnlyList<MigrationStep> GetMigrations();
    }
}
