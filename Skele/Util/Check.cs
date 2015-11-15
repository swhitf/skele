using System;

namespace Skele.Util
{
    internal static class Check
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