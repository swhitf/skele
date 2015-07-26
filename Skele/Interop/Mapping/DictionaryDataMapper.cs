using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Skele.Interop.Mapping
{
    class DictionaryDataMapper : AutoDataMapper<Dictionary<string, Object>>
    {
        protected override void MapProperty(Dictionary<string, Object> item, string name, Object value)
        {
            item.Add(name, value);
        }
    }
}
