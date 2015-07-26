using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Util
{
    static class Check
    {
        public static void ForNull(Object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(
                    string.Format("{0} cannot be null.", name));
            }
        }
    }
}
