using Skele.Interop;
using Skele.Interop.Mapping;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    class SessionAdapter
    {
        private IDatabaseSession _session;

        public SessionAdapter(IDatabaseSession session)
        {
            _session = session;
        }

        public ExpandoObject querySingle(string sql)
        {
            return _session.QuerySingle(sql, new ExpandoDataMapper());
        }

        public ExpandoObject[] query(string sql)
        {
            return _session.Query(sql, new ExpandoDataMapper()).ToArray();
        }
    }
}
