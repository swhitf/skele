using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migrations
{
    public abstract class Package
    {
        public abstract Uri Location
        {
            get;
        }

        public abstract IReadOnlyList<Migration> GetMigrations();
    }
}
