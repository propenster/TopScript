using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TopScript.RunValue;

namespace TopScript.StandardLib
{
    public struct StringObject
    {
        Common _common = default;
        StringBuilder _sb = new StringBuilder();
        public StringObject()
        {
            _common = new Common();

        }

        public NativeMethodCallback Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            NativeMethodCallback callback = null;
            switch(name.ToLowerInvariant().Trim())
            {
                case "contains": callback = string_contains; break;
                case "starts_with": callback = string_starts_with; break;
                case "ends_with": callback = string_ends_with; break;
                case "finish": callback = string_finish; break;
                case "append": callback = string_append; break;
                case "tap": callback = string_tap; break;
                case "to_upper": callback = string_to_upper; break;
                case "to_lower": callback = string_to_lower; break;
                case "is_null_or_whitespace": callback = string_is_null_or_whitespace; break;

                default: throw new ArgumentException(string.Format("Undefined method: {0}", name));

            }


            return callback;
        }

        public RunValue string_contains(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("String.contains", 1, arguments);

            var myString = context.to_string();
            foreach(var arg in arguments)
            {
                if (myString.Contains(arg.to_string())) return new BooleanRunValue(true);
            }
            return new BooleanRunValue(false);
        }
        public RunValue string_starts_with(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("String.starts_with", 1, arguments);

            var myString = context.to_string();
            foreach (var arg in arguments)
            {
                if (myString.StartsWith(arg.to_string())) return new BooleanRunValue(true);
            }
            return new BooleanRunValue(false);
        }
        public RunValue string_ends_with(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("String.ends_with", 1, arguments);

            var myString = context.to_string();
            foreach (var arg in arguments)
            {
                if (myString.EndsWith(arg.to_string())) return new BooleanRunValue(true);
            }
            return new BooleanRunValue(false);
        }
        public RunValue string_finish(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _sb.Clear();
            _common.arity("String.finish", 1, arguments);

            var myString = context.to_string();
            var append = arguments?.FirstOrDefault()?.to_string();
            _sb.Append(myString);
            if(!myString.EndsWith(append)) _sb.Append(append);
            return new StringRunValue(_sb.ToString());
        }
        public RunValue string_append(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _sb.Clear();
            _common.arity("String.append", 1, arguments);

            var myString = context.to_string();
            var append = arguments?.FirstOrDefault()?.to_string();
            _sb.Append(myString);
            _sb.Append(append);
            return new StringRunValue(_sb.ToString());
        }
        public RunValue string_tap(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {



            return new BooleanRunValue(false);
        }
        public RunValue string_to_upper(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("String.to_upper",0, arguments);
            return new StringRunValue(context.to_string().ToUpper());
        }
        public RunValue string_to_lower(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("String.to_lower", 0, arguments);
            return new StringRunValue(context.to_string().ToLower());
        }
        public RunValue string_is_null_or_whitespace(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("String.is_null_or_whitespace", 0, arguments);

            var myString = context.to_string();

            if (string.IsNullOrWhiteSpace(myString)) return new BooleanRunValue(true);
            
            return new BooleanRunValue(false);
        }
    }
}
