using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TopScript.RunValue;

namespace TopScript.StandardLib
{
    public struct NumberObject
    {

        Common _common = default;
        StringBuilder _sb = new StringBuilder();
        public NumberObject()
        {
            _common = new Common();

        }

        public NativeMethodCallback Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            NativeMethodCallback callback = null;
            switch (name.ToLowerInvariant().Trim())
            {
                case "is_integer": callback = number_is_integer; break;
                case "is_double": callback = number_is_double; break;
                default: throw new ArgumentException(string.Format("Undefined method: {0}", name));

            }


            return callback;
        }
        public RunValue number_is_integer(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("Number.is_integer", 0, arguments);
            var number = context.to_number();

            return new BooleanRunValue(int.TryParse(number.ToString(), out int n));
        }
        public RunValue number_is_double(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("Number.is_double", 0, arguments);
            var number = context.to_number();

            return new BooleanRunValue(double.TryParse(number.ToString(), out double n));
        }

    }
}
