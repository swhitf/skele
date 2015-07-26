using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    abstract class InputTransform<T>
    {
        public InputTransform()
        {
            Args = new Dictionary<int, Action<T, string>>();
            FlipSwitches = new Dictionary<string, Action<T>>();
            ValueSwitches = new Dictionary<string, Action<T, string>>();
        }

        protected Dictionary<int, Action<T, string>> Args
        {
            get;
            private set;
        }

        protected Dictionary<string, Action<T>> FlipSwitches
        {
            get;
            private set;
        }

        protected Action<T> Initializer
        {
            get;
            private set;
        }

        protected Dictionary<string, Action<T, string>> ValueSwitches
        {
            get;
            private set;
        }

        public InputTransform<T> Arg(int index, Action<T, string> setter)
        {
            Args.Add(index, setter);
            return this;
        }

        public InputTransform<T> Init(Action<T> initializer)
        {
            Initializer = initializer;
            return this;
        }

        public InputTransform<T> FlipSwitch(string code, Action<T> setter)
        {
            return FlipSwitch(code, null, setter);
        }

        public InputTransform<T> ValueSwitch(string code, Action<T, string> setter)
        {
            return ValueSwitch(code, null, setter);
        }

        public InputTransform<T> FlipSwitch(string code, string name, Action<T> setter)
        {
            FlipSwitches.Add(code, setter);

            if (name != null)
            {
                FlipSwitches.Add(name, setter);
            }

            return this;
        }

        public InputTransform<T> ValueSwitch(string code, string name, Action<T, string> setter)
        {
            ValueSwitches.Add(code, setter);

            if (name != null)
            {
                ValueSwitches.Add(name, setter);
            }

            return this;
        }

        protected void TriggerArg(T obj, int index, string value)
        {
            if (Args.ContainsKey(index))
            {
                Args[index](obj, value);
            }
        }

        protected void TriggerFlipSwitch(T obj, string name)
        {
            if (FlipSwitches.ContainsKey(name))
            {
                FlipSwitches[name](obj);
            }
        }

        protected void TriggerValueSwitch(T obj, string name, string value)
        {
            if (ValueSwitches.ContainsKey(name))
            {
                ValueSwitches[name](obj, value);
            }
        }
    }
}
