using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TopScript.Exceptions;
using TopScript.StandardLib;
using static TopScript.RunValue;

namespace TopScript
{
    public interface IInterpreter
    {
        RunValue? Call(RunValue callable, List<RunValue> arguments);
        bool Execute(AstProgram program);
    }
    public class Interpreter : IInterpreter
    {
        public Interpreter(List<Statement> ast, InterpreterEnvironment environment, Dictionary<string, RunValue> globals, string filePath)
        {
            Ast = ast;
            Environment = environment;
            Globals = globals;
            FilePath = filePath;
        }

        public List<Statement> Ast { get; }
        public InterpreterEnvironment Environment { get; set; }
        public Dictionary<string, RunValue> Globals { get; }
        public string FilePath { get; }

        public RunValue? Call(RunValue callable, List<RunValue> arguments)
        {
            if (callable is NativeFunctionRunValue nativeFunction)
            {
                nativeFunction.Callback(this, arguments);
            }
            else if (callable is NativeMethodRunValue nativeMethod)
            {
                var context = RunExpression(nativeMethod?.Context);
                nativeMethod.Callback(this, context, arguments);
            }
            else if (callable is FunctionRunValue functionRunValue)
            {
                if (functionRunValue?.Parameters.FirstOrDefault() is null && functionRunValue?.Parameters.Count != arguments.Count)
                {
                    throw new InterpreterExceptions.TooFewArgumentsException("missing arguments.");
                }
                var oldEnv = this.Environment;
                var newEnv = functionRunValue?.Environment ?? new InterpreterEnvironment();
                if (functionRunValue?.Context != null && functionRunValue?.Parameters.FirstOrDefault() != null)
                {
                    var context = RunExpression(functionRunValue?.Context);
                    newEnv.Set("this", context);
                }

                var w = functionRunValue.Parameters.Where(x => x.Name != "this").Zip(arguments);
                foreach ((var Param, var value) in w)
                {
                    newEnv.Set(Param?.Name, value);
                }

                Environment = newEnv;
                RunValue returnValue = default;
                foreach (var stmt in functionRunValue?.Block)
                {
                    RunStatement(stmt);
                }
                Environment = oldEnv;


                return returnValue;


            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public bool Execute(AstProgram program)
        {
            var ast = program.Statements;
            try
            {
                foreach (var statement in ast)
                {
                    RunStatement(statement);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        private RunValue RunExpression(StatementExpression expression)
        {
            if (expression is NumericExpression n)
            {
                return new NumberRunValue(n.Literal);
            }
            else if (expression is StringExpression s)
            {
                return new StringRunValue(s.Literal);
            }
            else if (expression is BooleanExpression b)
            {
                return new BooleanRunValue(b.Boolean);
            }
            else if (expression is IdentifierExpression id)
            {
                if (!Globals.TryGetValue(id.Identifier, out var value))
                {
                    var v = Environment.Get(id.Identifier);
                    if (v != null) return v;
                    else throw new InterpreterExceptions.UndefinedVariableException(id.Identifier);
                }

                return value;
            }
            else if (expression is ListIndexExpression index)
            {
                var instance = RunExpression(index.Expression);
                var indexFromExpression = RunExpression(index.Index ?? throw new InterpreterExceptions.ExpectedIndexException("expected index")).to_number();
                var actualIndex = Convert.ToUInt32(indexFromExpression);

                if (instance is ListRunValue ls)
                {
                    var v = ls.Values.ElementAt((int)actualIndex);
                    if (v != null) return v;
                    throw new InterpreterExceptions.UndefinedIndexException(actualIndex.ToString());
                }

            }
            else if (expression is GetExpression get)
            {
                var instance = RunExpression(get?.Expression);
                if (instance is null) throw new InterpreterExceptions.ExpressionEvaluationException(get.Expression.ToString());
            }
            else if(expression is InfixExpression infix)
            {
                throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }
        private void RunStatement(Statement statement)
        {

            if (statement != null)
            {
                if (statement is VarStatement varStatement)
                {
                    if (varStatement?.Expression is null) Environment.Set(varStatement?.Name, new NullRunValue());
                    else
                    {
                        var value = RunExpression(varStatement?.Expression);
                        Environment.Set(varStatement?.Name, value);
                    }
                }
                else if (statement is FunctionDeclarationStatement functionDeclarationStatement)
                {
                    Globals.Add(functionDeclarationStatement?.Name, new FunctionRunValue(functionDeclarationStatement?.Name, functionDeclarationStatement?.Parameters, functionDeclarationStatement?.Body, null, null));
                }
                else if (statement is StructDeclarationStatement structDeclarationStatement)
                {
                    Globals.Add(structDeclarationStatement?.Name, new StructRunValue(structDeclarationStatement?.Name, structDeclarationStatement?.Fields, new Dictionary<string, RunValue>()));

                }
                else if (statement is ForStatement forStatement)
                {
                    var iterable = RunExpression(forStatement?.Iterable);
                    var items = new List<RunValue>();
                    if (iterable is ListRunValue listRunValue) items = listRunValue.Values;
                    else throw new InterpreterExceptions.InvalidIterableException(iterable.typestring());

                    if (!items.Any()) return;

                    var indexIsSet = forStatement?.Index != null;
                    for (int i = 0; i < items.Count; i++)
                    {
                        Environment.Set(forStatement?.Value, items[i]);
                        if (indexIsSet) Environment.Set(forStatement?.Index, new NumberRunValue((double)i));

                        foreach (var stmt in forStatement?.Block)
                        {
                            RunStatement(stmt);
                        }
                    }

                    Environment.Drop(forStatement?.Value);
                    if (indexIsSet) Environment.Drop(forStatement?.Index);

                }
                else if (statement is IfStatement ifStatement)
                {
                    var condition = RunExpression(ifStatement?.Condition);
                    if (condition.to_bool())
                    {
                        foreach (var stmt in ifStatement?.Then)
                        {
                            RunStatement(stmt);
                        }
                    }
                    else
                    {
                        foreach (var stmt in ifStatement?.Otherwise)
                        {
                            RunStatement(stmt);
                        }
                    }
                }
                else if (statement is ExpressionStatement expressionStatement)
                {
                    RunExpression(expressionStatement.Expression);
                }
                else if (statement is ReturnStatement returnStatement)
                {
                    var t = RunExpression(returnStatement?.Expression);
                    if (t is null) throw new InterpreterExceptions.ReturnValueException();
                }
                throw new NotImplementedException();
            }


        }
        private void DefineGlobalFunction(string name, NativeFunctionCallback callback)
        {

        }

        private RunValue GetProperty(RunValue value, string field, StatementExpression target)
        {
            if (value is StructInstanceRunValue structInstance)
            {
                var fieldValue = Environment.Get(field);
                if (fieldValue != null)
                {
                    if (fieldValue is FunctionRunValue functionRunValue)
                    {
                        return new FunctionRunValue(functionRunValue?.Name, functionRunValue?.Parameters, functionRunValue?.Block, functionRunValue?.Environment, functionRunValue?.Context);
                    }
                    return fieldValue;
                }
                else
                {
                    var name = string.Empty;
                    if (structInstance?.Definition is StructRunValue st)
                    {
                        name = st?.Name;
                    }
                    else
                    {
                        throw new Exception();
                    }

                    throw new InterpreterExceptions.UndefinedFieldException(string.Format("Name: {0} Field: {1}", name, field));
                }
            }
            else if (value is StructRunValue str)
            {
                if (!str.Methods.TryGetValue(field, out var methodValue))
                {
                    throw new InterpreterExceptions.UndefinedMethodException(string.Format("Name: {0} Field: {1}", str.Name, field));
                }
                return methodValue;

            }
            else if (value is StringRunValue strv)
            {
                var _so = new StringObject();
                var v = new NativeMethodRunValue(field, _so.Get(field), target);
                return v;
            }
            else if (value is NumberRunValue nrv)
            {
                var _no = new NumberObject();
                var v = new NativeMethodRunValue(field, _no.Get(field), target);
                return v;
            }
            else if (value is ListRunValue lrv)
            {
                var _lo = new ListObject();
                var v = new NativeMethodRunValue(field, _lo.Get(field), target);
                return v;
            }
            //add consts later... 

            throw new NotImplementedException();
        }
    }
}
