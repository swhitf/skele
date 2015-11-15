using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Util
{
    public static class DefaultValues
    {
        public static T For<T>()
        {
            return (T)For(typeof(T));
        }

        public static object For(Type type)
        {
            if (type == typeof(DateTime))
            {
                return DateTime.Now;
            }
            if (type == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Now;
            }

            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}
