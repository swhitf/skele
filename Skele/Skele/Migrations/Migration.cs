using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migrations
{
    public class Migration : List<Resource>
    {
        public Migration(Version target)
	    {
            Target = target;
	    }

        public Version Target { get; private set; }
    }
}
