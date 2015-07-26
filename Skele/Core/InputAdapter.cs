using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Skele.Core
{
    class InputAdapter<TDispatcher> 
        where TDispatcher : ICommandDispatcher
    {
        private TDispatcher dispatcher;
        private Dictionary<string, IAdapterTransform> commandTransforms;
        private List<IAdapterTransform> filterTransforms;

        public InputAdapter(TDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;

            commandTransforms = new Dictionary<string, IAdapterTransform>();
            filterTransforms = new List<IAdapterTransform>();
        }

        public InputTransform<TCommand> Map<TCommand>(string commandName) 
            where TCommand : ICommand, new()
        {
            return Map(commandName, () => new TCommand());
        }

        public InputTransform<TCommand> Map<TCommand>(string commandName, Func<TCommand> factory) 
            where TCommand : ICommand
        {
            var ct = new CommandTransform<TCommand>(factory, dispatcher);
            commandTransforms.Add(commandName, ct);

            return ct;
        }

        public InputTransform<TDispatcher> Filter()            
        {
            var ct = new FilterTransform(dispatcher);
            filterTransforms.Add(ct);

            return ct;
        }

        public int Apply(string[] input)
        {
            if (input == null || input.Length == 0)
            {
                throw new ArgumentException("input must not be null or empty.");
            }

            foreach (var ft in filterTransforms)
            {
                ft.Apply(input);
            }

            if (commandTransforms.ContainsKey(input[0]))
            {
                return commandTransforms[input[0]].Apply(
                   input.Skip(1).ToArray());
            }

            throw new InvalidOperationException(string.Format("Unrecognized command: {0}", input[0]));
        }

        private interface IAdapterTransform
        {
            int Apply(string[] input);
        }

        private class CommandTransform<TCommand> : InputTransform<TCommand>, IAdapterTransform
            where TCommand : ICommand
        {
            private Func<TCommand> factory;
            private TDispatcher dispatcher;

            public CommandTransform(Func<TCommand> factory, TDispatcher dispatcher)
            {
                this.factory = factory;
                this.dispatcher = dispatcher;
            }

            public int Apply(string[] input)
            {
                var command = factory();

                if (Initializer != null)
                {
                    Initializer(command);
                }

                foreach (var a in input)
                {
                    int index = 0;

                    if (a.StartsWith("/") || a.StartsWith("-") || a.StartsWith("--"))
                    {
                        var parts = Regex.Replace(a, "^(\\/)|(--?)", "").Split('=');
                        var name = parts[0];

                        if (parts.Length == 1)
                        {
                            TriggerFlipSwitch(command, parts[0]);
                        }
                        else
                        {
                            TriggerValueSwitch(command, parts[0], parts[1]);
                        }
                    }
                    else
                    {
                        TriggerArg(command, index++, a);
                    }
                }

                return dispatcher.Dispatch(command);
            }
        }
    
        private class FilterTransform : InputTransform<TDispatcher>, IAdapterTransform
        {
            private TDispatcher dispatcher;

            public FilterTransform(TDispatcher dispatcher)
            {
                this.dispatcher = dispatcher;
            }

            public int Apply(string[] input)
            {
                if (Initializer != null)
                {
                    Initializer(dispatcher);
                }

                foreach (var a in input)
                {
                    int index = 0;

                    if (a.StartsWith("/") || a.StartsWith("-") || a.StartsWith("--"))
                    {
                        var parts = Regex.Replace(a, "^(\\/)|(--?)", "").Split('=');
                        var name = parts[0];

                        if (parts.Length == 1)
                        {
                            TriggerFlipSwitch(dispatcher, parts[0]);
                        }
                        else
                        {
                            TriggerValueSwitch(dispatcher, parts[0], parts[1]);
                        }
                    }
                    else
                    {
                        TriggerArg(dispatcher, index++, a);
                    }
                }

                return 0;
            }
        }
    }
}
