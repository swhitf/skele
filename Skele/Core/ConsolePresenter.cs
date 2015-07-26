using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    class ConsolePresenter : IPresenter
    {
        public bool Confirm(string message)
        {
            Console.WriteLine(message);
            return new[] { "y", "yes", "true", "1" }
                .Contains(Console.ReadLine().Trim().ToLower());
        }

        public string Prompt(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine().Trim();
        }

        public T Prompt<T>(string message) where T : IConvertible
        {
            Console.WriteLine(message);
            return (T)Convert.ChangeType(Console.ReadLine().Trim(), typeof(T));
        }
    }
}
