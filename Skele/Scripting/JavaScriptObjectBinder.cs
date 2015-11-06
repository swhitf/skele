using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    class JavaScriptObjectBinder : Dictionary<string, object>
    {
        public static JavaScriptObjectBinder Create(object obj, IScriptContextProvider contextProvider)
        {
            var binder = new JavaScriptObjectBinder(obj, contextProvider);
            binder.Bind();

            return binder;
        }

        private object target;
        private IScriptContextProvider contextProvider;

        private JavaScriptObjectBinder(object target, IScriptContextProvider contextProvider)
        {
            this.target = target;
            this.contextProvider = contextProvider;
        }

        private void Bind()
        {
            var type = target.GetType();
            foreach (var m in type.GetMethods())
            {
                var sea = m.GetCustomAttribute<ScriptExportAttribute>();
                if (sea == null)
                    continue;

                this[CamelCase(sea.Name)] = BindMethod(m);
            }
        }

        private object BindMethod(MethodInfo method)
        {
            var parameters = method
                .GetParameters();

            if (parameters.Any() && parameters[0].ParameterType == typeof(IScriptContext))
            {
                return WrapMethod(method);
            }
            else
            {
                return ForwardMethod(method);
            }
        }

        private object WrapMethod(MethodInfo method)
        {
            var parameters = method
                .GetParameters()
                .ToList();

            var wrapperSignature = parameters
                .Skip(1)
                .Select(x => x.ParameterType)
                .Concat(new[] { method.ReturnType })
                .ToArray();

            var delegateType = Expression.GetDelegateType(wrapperSignature);

            var callSignature = new List<Expression>();
            callSignature.Add(Expression.Constant(null, typeof(IScriptContext)));
            callSignature.AddRange(parameters
                .Skip(1)
                .Select(x => Expression.Parameter(x.ParameterType)));

            //var wrapper = Expression.Lambda(delegateType,
            //    Expression.Call(
            //        Expression.Constant(target),
            //        method,
            //        callSignature
            //    ));

            var wrapper = Expression.Lambda(
                delegateType,
                Expression.Throw(Expression.Constant(new NotImplementedException())),
                method.GetParameters().Skip(1).Select(x => Expression.Parameter(x.ParameterType, x.Name)));
            
            return wrapper.Compile();
        }

        private object ForwardMethod(MethodInfo method)
        {
            var signature = method
                .GetParameters()
                .Skip(1)
                .Select(x => x.ParameterType)
                .Concat(new[] { method.ReturnType })
                .ToArray();

            var delegateType = Expression.GetDelegateType(signature);
            return method.CreateDelegate(delegateType, target);
        }

        private string CamelCase(string str)
        {
            return Char.ToLower(str[0]) + str.Substring(1);
        }
    }
}
