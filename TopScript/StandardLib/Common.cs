using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TopScript.Exceptions;
using static TopScript.RunValue;

namespace TopScript.StandardLib
{
    public struct Common
    {
        public void arity(string name, uint arity, List<RunValue> arguments)
        {
            if (arity != arguments.Count) throw new ArgumentException(string.Format("Method {0} expected {1} arguments, received {2}.", name, arity, arguments.Count));
        }
        public RunValue println(Interpreter interpreter, List<RunValue> args)
        {
            var arg = args.FirstOrDefault();
            Console.WriteLine(arg?.ToString());

            return new NullRunValue();
        }
        public RunValue print(Interpreter interpreter, List<RunValue> args)
        {
            var arg = args.FirstOrDefault();
            Console.Write(arg?.ToString());

            return new NullRunValue();
        }
        public RunValue receive_correct_type(Interpreter interpreter, List<RunValue> args)
        {
            if(!args.Any() || args.Count > 1)
            {
                throw new ArgumentException(string.Format("Function {0} expects {1} arguments, received {2}.", "type", 1, args.Count));
            }
            var arg = args.FirstOrDefault();
            return new StringRunValue(arg?.typestring() ?? string.Empty);
        }
        /// <summary>
        /// usage load "../myothermodule.top"
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InterpreterExceptions.UnitCompilationException"></exception>
        /// <exception cref="InterpreterExceptions.LoadModuleException"></exception>
        public RunValue load(Interpreter interpreter, List<RunValue> args)
        {
            arity("load", 1, args);

            var libraryPath = args?.FirstOrDefault()?.to_string();
            var interpreterDir = Directory.GetParent(interpreter.FilePath);

            if (libraryPath.StartsWith("."))
            {
                var modulePath = interpreterDir.FullName;
                if(libraryPath.EndsWith(".top"))
                {
                    modulePath = Path.Combine(modulePath, libraryPath);
                }
                else
                {
                    modulePath = Path.Combine(modulePath, string.Format("{0}{1}", libraryPath, ".top"));

                }

                var contents = File.ReadAllText(modulePath);
                var lexer = new Lexer(contents);
                var parser = new Parser(lexer);
                var program = parser.Parse();

                var execution = interpreter.Execute(program);
                if (execution) return new NullRunValue();

                throw new InterpreterExceptions.UnitCompilationException("could not compile unit source");


            }

            throw new InterpreterExceptions.LoadModuleException("could not find module");

        }

    }
}
