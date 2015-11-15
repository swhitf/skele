using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop
{
    public interface IDatabaseDriver
    {
        IDatabaseManager Connect(string connectionString);
    }
}
