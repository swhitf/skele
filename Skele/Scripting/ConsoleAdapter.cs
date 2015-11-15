using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    public class ConsoleAdapter
    {
        public ConsoleAdapter()
        {
        }

        public void log(string output)
        {
            Console.WriteLine(output);
        }

        public string prompt(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        public void dir(Object obj)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            log(JsonConvert.SerializeObject(obj, settings));
        }
    }
}
