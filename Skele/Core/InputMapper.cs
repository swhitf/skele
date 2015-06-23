using Skele.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    class InputMapper
    {
        private Dictionary<string, MappingBase> mappings;

        public InputMapper(ICommandDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;

            mappings = new Dictionary<string, MappingBase>();
        }

        public IMapping<T> Map<T>(string command) where T : ICommand, new()
        {
            var m = new MappingImpl<T>(dispatcher);
            mappings.Add(command, m);

            return m;
        }

        public void Process(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                throw new ArgumentException("args must not be null or empty.");
            }

            if (mappings.ContainsKey(args[0]))
            {
                mappings[args[0]].CreateAndExecute(
                   args.Skip(1).ToArray());
            }

            throw new InvalidOperationException(string.Format("Unrecognized command: {0}", args[0]));
        }

        private abstract class MappingBase
        {
            public abstract void CreateAndExecute(string[] args);
        }

        private class MappingImpl<T> : MappingBase, IMapping<T> where T : ICommand, new()
        {
            private Dictionary<string, Action<T, string>> args;
            private Dictionary<string, Action<T>> switches;
            private Func<T> factory;
            private Action<T> initializer;

            public MappingImpl(ICommandDispatcher dispatcher)
            {
                this.dispatcher = dispatcher;

                args = new Dictionary<string, Action<T, string>>();
                switches = new Dictionary<string, Action<T>>();
                factory = () => new T();
            }

            public override void CreateAndExecute(string[] args)
            {
                var command = factory();

                if (initializer != null)
                {
                    initializer(command);
                }

                foreach (var a in args)
                {
                    if (a.StartsWith("/") || a.StartsWith("-"))
                    {
                        var parts = a.Substring(1).Split('=');
                        var name = parts[0];

                        if (parts.Length == 1)
                        {
                            TriggerSwitch(command, parts[0]);
                        }
                        else
                        {
                            TriggerArg(command, parts[0], parts[1]);
                        }
                    }
                }

                dispatcher.Dispatch(command);
            }

            private void TriggerSwitch(T command, string name)
            {
                if (switches.ContainsKey(name))
                {
                    switches[name](command);
                }
            }

            private void TriggerArg(T command, string name, string value)
            {
                if (args.ContainsKey(name))
                {
                    args[name](command, value);
                }
            }

            public IMapping<T> Arg(string name, Action<T, string> setter)
            {
                args.Add(name, setter);
                return this;
            }

            public IMapping<T> Switch(string code, Action<T> setter)
            {
                switches.Add(code, setter);
                return this;
            }


            public IMapping<T> Factory(Func<T> factory)
            {
                this.factory = factory;
                return this;
            }

            public IMapping<T> Init(Action<T> initializerd)
            {
                this.initializer = initializer;
                return this;
            }

            public ICommandDispatcher dispatcher { get; set; }
        }

        public interface IMapping<T> where T : ICommand
        {
            IMapping<T> Init(Action<T> initializer);

            IMapping<T> Arg(string name, Action<T, string> setter);

            IMapping<T> Factory(Func<T> factory);

            IMapping<T> Switch(string code, Action<T> setter);
        }

        public ICommandDispatcher dispatcher { get; set; }
    }
}
