using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Skele.Interop.Mapping
{
    class ExpandoDataMapper : AutoDataMapper<ExpandoObject>
    {
        protected override void MapProperty(ExpandoObject item, string name, object value)
        {
            ((IDictionary<string, object>)item)[name] = value;
        }
    }
}
