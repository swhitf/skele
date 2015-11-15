using Jint;
using Newtonsoft.Json;
using Skele.Core;
using Skele.Interop;
using Skele.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    class JavaScriptStubCommandHandler : CommandHandlerBase<JavaScriptStubCommand>
    {
        public JavaScriptStubCommandHandler(ICommandContext context)
            : base(context)
        {
        }

        public override int Execute(JavaScriptStubCommand input)
        {
            var session = Context.GetDatabaseSession();
            var metadata = session.Describe();
            var table = metadata.Tables[input.TableName];

            var stub = new Dictionary<string, object>();
            foreach (var column in table.Columns)
            {
                object value = null;

                if (!column.Nullable)
                {
                    value = DefaultValues.For(column.DataType) ?? column.DataType.Name;
                }

                stub[column.Name] = value;
            }

            var json = JsonConvert.SerializeObject(stub, Formatting.Indented);
            var code = new StringBuilder();

            code.AppendFormat("{0}.add({1}", table.Name, Environment.NewLine);
            code.Append(json);
            code.AppendLine(");");

            Output.WriteLine(code);
            return SuccessResult();
        }
    }
}
