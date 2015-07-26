using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.Mapping
{
    public interface IDataMapper<T>
    {
        T Map(IDataRecord record);
    }
}