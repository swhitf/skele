using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    public class MigrationStep : List<Resource>
    {
        public MigrationStep(Version target)
	    {
            Target = target;
	    }

        public Version Target { get; private set; }
    }
}
