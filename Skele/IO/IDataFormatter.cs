using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.IO
{
    public interface IDataFormatter
    {
        void WriteObject(Object obj, Stream output);

        void WriteList(List<Object> list, Stream output);
    }
}
