using Jint.Runtime.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.Native;
using Jint.Parser;

namespace Skele.Scripting
{
    public class JavaScriptEngine : IScriptEngine, IScriptContextProvider
    {
        private Dictionary<string, object> rootObjects;

        public JavaScriptEngine()
        {
            rootObjects = new Dictionary<string, object>();
        }

        public void Execute(string source)
        {
            var engine = new Jint.Engine(x => x.Strict(true));

            foreach (var item in rootObjects)
            {
                engine.SetValue(item.Key, item.Value);
            }

            engine.SetValue("console", JavaScriptObjectBinder.Create(new ConsoleAdapter(), this));
            engine.Execute(source);
        }

        public IScriptContext GetExecutingContext()
        {
            return new JavaScriptContext();
        }

        public void Set(string name, Object obj)
        {
            rootObjects[name] = obj;
        }
    }
}
