using Jint.Runtime.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.Native;
using Jint.Parser;
using System.IO;

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
            CreateJint().Execute(source);
        }

        public IScriptContext GetExecutingContext()
        {
            return new JavaScriptContext();
        }

        public void Set(string name, Object obj)
        {
            rootObjects[name] = obj;
        }

        private Jint.Engine CreateJint()
        {
            var jint = new Jint.Engine(x => x.Strict(true));
            jint.SetValue("file", new Func<string, string>(FileFunc));
            jint.SetValue("format", new Func<string, object[], string>(FormatFunc));

            jint.Execute(Scripts.env);

            foreach (var item in rootObjects)
            {
                jint.SetValue(item.Key, item.Value);
            }

            jint.SetValue("console", new ConsoleAdapter());
            return jint;
        }

        private string FormatFunc(string format, object[] args)
        {
            return String.Format(format, args);
        }

        private string FileFunc(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        //private JsValue Require(Jint.Engine jint, string filePath)
        //{
        //    var source = File.ReadAllText(filePath);
        //    jint.Execute("var module = {}");
        //    jint.Execute("var exports = module.exports = {};");
        //    jint.Execute(source);
            
        //    return jint.GetValue("exports");
        //}
    }
}
