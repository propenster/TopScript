using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TopScript.RunValue;

namespace TopScript.StandardLib
{
    public struct ListObject
    {
        Common _common = default;
        StringBuilder _sb = new StringBuilder();
        public ListObject()
        {
            _common = new Common();

        }

        public NativeMethodCallback Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            NativeMethodCallback callback = null;
            switch (name.ToLowerInvariant().Trim())
            {
                case "is_empty": callback = list_is_empty; break;
                case "reverse": callback = list_reverse; break;
                case "join": callback = list_join; break;
                case "filter": callback = list_filter; break;
                case "append": callback = list_append; break;
                case "each": callback = list_each; break;
                case "map": callback = list_map; break;
                case "first": callback = list_first; break;
                case "last": callback = list_last; break;
                //case "get": callback = list_get; break;

                default: throw new ArgumentException(string.Format("Undefined method: {0}", name));

            }


            return callback;
        }

        public RunValue list_is_empty(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("List.is_empty()", 0, arguments);

            return new BooleanRunValue(!context.to_list().Any());
        }
        public RunValue list_reverse(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("List.reverse()", 0, arguments);
            var list = context.to_list().Reverse().ToList();

            return new ListRunValue(list);
        }
        public RunValue list_join(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("List.join()", 1, arguments);
            var list = context.to_list().ToList();
            var separator = arguments?.FirstOrDefault()?.to_string();

            var result = string.Join(separator, list.Select(x => x.to_string()).ToList());

            return new StringRunValue(result);
        }
        public RunValue list_filter(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("List.filter()", 1, arguments);
            
            var callback = arguments?.FirstOrDefault();
            var list = new List<RunValue>();

            foreach(var item in context.to_list().ToList())
            {
                var ls = new List<RunValue>();
                ls.Add(item);
                if(interpreter.Call(callback,  ls).to_bool())
                {
                    list.Add(item);
                }
               
            }

            return new ListRunValue(list);
        }
        public RunValue list_each(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("List.each()", 1, arguments);

            var callback = arguments?.FirstOrDefault();
            var list = new List<RunValue>();

            foreach (var item in context.to_list().ToList())
            {
                interpreter.Call(callback, new List<RunValue> { item }).to_bool();     
            }

            return context;
        }
        public RunValue list_map(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("List.map()", 1, arguments);

            var callback = arguments?.FirstOrDefault();
            var list = context.to_list().ToList();

            for (int i = 0; i < list.Count; i++)
            {
                var res = interpreter.Call(callback, new List<RunValue> { list[i] });
                list[i] = res;
            }

            return new ListRunValue(list);
        }
        public RunValue list_first(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            //_common.arity("List.map()", 1, arguments);

            
            var list = context.to_list().ToList();

            if (!list.Any())
            {
                return new NullRunValue();
            }
            if(arguments.Count == 1)
            {
                var callback = arguments?.FirstOrDefault();
                foreach(var item in list)
                {
                    var result = interpreter.Call(callback, new List<RunValue> { item });
                    if (result.to_bool()) return item;
                }

            }

            
            return list.First();
        }
        public RunValue list_last(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            //_common.arity("List.map()", 1, arguments);
            var list = context.to_list().Reverse().ToList();

            if (!list.Any())
            {
                return new NullRunValue();
            }
            if (arguments.Count == 1)
            {
                var callback = arguments?.FirstOrDefault();
                foreach (var item in list)
                {
                    var result = interpreter.Call(callback, new List<RunValue> { item });
                    if (result.to_bool()) return item;
                }
            }
            return list.First();
        }

        public RunValue list_append(Interpreter interpreter, RunValue context, List<RunValue> arguments)
        {
            _common.arity("List.append()", 1, arguments);

            var callback = arguments?.FirstOrDefault();
            var list = new List<RunValue>();

            foreach (var item in context.to_list().ToList())
            {
                var ls = new List<RunValue>();
                ls.Add(item);
                var r = interpreter.Call(callback, ls);
                list.Add(item);

            }
            return new ListRunValue(list);
        }
    }
}
