using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopScript
{
    public interface IInterpreterEnvironment
    {
        void Set(string key, RunValue value);
        RunValue? Get(string key);
        void Drop(string key);
        void Dump();
    }
    public class InterpreterEnvironment : IInterpreterEnvironment
    {
        public Dictionary<string, RunValue> Values { get; }

        public InterpreterEnvironment()
        {
            Values = new Dictionary<string, RunValue>();
        }

        public void Set(string key, RunValue value)
        {
            if(!Values.TryAdd(key, value))
            {
                throw new InvalidOperationException(string.Format("Unit interpretation error. Could not set key: {0} value: {1}", key, value));
            }
        }

        public RunValue? Get(string key)
        {
            Values.TryGetValue(key, out RunValue value);
            return value ?? default;
        }

        public void Drop(string key)
        {
            Values.Remove(key);
        }

        public void Dump()
        {
            Debug.WriteLine(Values);
        }
    }
}
