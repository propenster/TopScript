using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TopScript
{
    public class NumericExpression : StatementExpression
    {
        public NumericExpression(double literal)
        {
            Literal = literal;
        }

        public double Literal { get; }

        public override string ToString()
        {
            return string.Format("Number({0})", Literal);
        }
    }

    public class StringExpression : StatementExpression
    {
        public StringExpression(string literal)
        {
            Literal = literal ?? string.Empty;
        }
        public string Literal { get; }
        public override string ToString()
        {
            return string.Format("String({0})", Literal);
        }

    }
    public class NullExpression : StatementExpression
    {
        public NullExpression()
        {
            
        }
    }
    public class PrefixExpression : StatementExpression
    {
        public PrefixExpression(Token token, StatementExpression expression)
        {
            Token = token;
            Expression = expression;
        }

        public Token Token { get; }
        public StatementExpression Expression { get; }

        public override string ToString()
        {
            return string.Format("{{ Token: {0}, Expression: {1} }}", Token, Expression);
        }


    }
    public class ListExpression : StatementExpression
    {
        public ListExpression(List<StatementExpression> items)
        {
            Items = items;
        }

        public List<StatementExpression> Items { get; }
        public override string ToString()
        {
            return string.Format("[ Items: {0} ]", string.Join(", ", Items));
        }
    }
    public class ListIndexExpression : StatementExpression
    {
        public ListIndexExpression(StatementExpression expression, StatementExpression? index)
        {
            Expression = expression;
            Index = index;
        }

        public StatementExpression Expression { get; }
        public StatementExpression? Index { get; }

        public override string ToString()
        {
            return string.Format("{{ Expression: {0}, Index: [{1}] }}", Expression, Index);
        }
    }
    public class StructExpression : StatementExpression
    {
        public StructExpression(StatementExpression expression, Dictionary<string, StatementExpression> fields)
        {
            Expression = expression;
            Fields = fields;
        }

        public StatementExpression Expression { get; }
        public Dictionary<string, StatementExpression> Fields { get; }

        public override string ToString()
        {
            return string.Format("{{ Expression: {0}, Fields: [{1}] }}", Expression, Fields);
        }
    }

    public class CallExpression : StatementExpression
    {
        public CallExpression(StatementExpression expression, List<StatementExpression> args)
        {
            Expression = expression;
            Args = args;
        }
        public StatementExpression Expression { get; }
        public List<StatementExpression> Args { get; }

        public override string ToString()
        {
            return string.Format("{{ Expression: {0}, Args: [{1}] }}", Expression, string.Join(", ", Args));
        }
    }
    public class InfixExpression : StatementExpression
    {
        public InfixExpression(StatementExpression left, Op operand, StatementExpression right)
        {
            Left = left;
            Operand = operand;
            Right = right;
        }

        public StatementExpression Left { get; }
        public Op Operand { get; }
        public StatementExpression Right { get; }

        public override string ToString()
        {
            return string.Format("{{ Left: {0}, Op: {1}, Right: {2} }}", Left, Operand, Right);
        }
    }
    public class AssignmentExpression : StatementExpression
    {
        public AssignmentExpression(StatementExpression left, StatementExpression right)
        {
            Left = left;
            Right = right;
        }

        public StatementExpression Left { get; }
        public StatementExpression Right { get; }

        public override string ToString()
        {
            return string.Format("{{ Left: {0}, Right: {1} }}", Left, Right);
        }
    }

    public class GetExpression : StatementExpression
    {
        public GetExpression(StatementExpression expression, string field)
        {
            Expression = expression;
            Field = field;
        }

        public StatementExpression Expression { get; }
        public string Field { get; }

        public override string ToString()
        {
            return string.Format("{{ Expression: {0}, Field: {1} }}", Expression, Field);
        }
    }
    public class BooleanExpression : StatementExpression
    {
        public BooleanExpression(bool boolean)
        {
            Boolean = boolean;
        }
        public bool Boolean { get; }

        public override string ToString()
        {
            return string.Format("Bool({0})", Boolean);
        }
    }
    public class IdentifierExpression : StatementExpression
    {
        public IdentifierExpression(string identifier)
        {
            Identifier = identifier;
        }
        public string Identifier { get; }
        public override string ToString()
        {
            return string.Format("Identifier({0})", Identifier);
        }
    }

    public class BinaryExpression : StatementExpression
    {
        public BinaryExpression(StatementExpression lhs, BinaryOperand operand, StatementExpression rhs)
        {
            Lhs = lhs;
            Operand = operand;
            Rhs = rhs;
        }
        public StatementExpression Lhs { get; }
        public BinaryOperand Operand { get; }
        public StatementExpression Rhs { get; }

        public override string ToString()
        {
            return string.Format("Binary({0}, {1}, {2})", Lhs.ToString(), Operand, Rhs.ToString());
        }

    }
    public class ClosureExpression : StatementExpression
    {
        public ClosureExpression(List<Parameter> parameters, List<Statement> body)
        {
            Parameters = parameters;
            Body = body;
        }

        public List<Parameter> Parameters { get; }
        public List<Statement> Body { get; }
        public override string ToString()
        {
            return string.Format("{{ Parameters: [{0}], Body: [{1}] }}", string.Join(", ", Parameters), string.Join(", ", Body));
        }
    }
    public enum BinaryOperand
    {
        Plus,
        Multiply,
        Minus,
        Divide,
    }

    public abstract class StatementExpression
    {

    }
    public interface IStatement
    {
        StatementKind Kind { get; }

    }
    public interface IVarStatement : IStatement
    {
        string Name { get; }
        StatementExpression Expression { get; }
    }
    public interface IExpressionStatement : IStatement
    {
        StatementExpression Expression { get; }
    }
    public interface IFunctionDeclarationStatement : IStatement
    {
        string Name { get; }
        List<Parameter> Parameters { get; }
        List<Statement> Body { get; }
    }
    public interface IStructDeclarationStatement : IStatement
    {
        string Name { get; }
        List<Parameter> Fields { get; }
    }
    public interface IForStatement : IStatement
    {
        StatementExpression Iterable { get; }
        string Identifier { get; }
        string? Index { get; }
        List<Statement> Block { get; }
    }
    public class ForStatement : Statement, IForStatement
    {
        public StatementKind Kind => StatementKind.For;

        public ForStatement(StatementExpression iterable, string value, string? index, List<Statement> block)
        {
            Iterable = iterable;
            Value = value;
            Index = index;
            Block = block;
        }

        public StatementExpression Iterable { get; }
        public string Value { get; }
        public string Identifier { get; }

        public string? Index { get; }

        public List<Statement> Block { get; }
        
    }
    public class VarStatement : Statement, IVarStatement
    {
        public StatementKind Kind => StatementKind.Var;
        public VarStatement(string name, StatementExpression expression)
        {
            Name = name;
            Expression = expression;
        }
        public string Name { get; }

        public StatementExpression Expression { get; }

        public override string ToString()
        {
            return string.Format("Var {{ name: {0}, expression: {1} }}", Name, Expression.ToString());
        }
    }
    public class ExpressionStatement : Statement, IExpressionStatement
    {
        public StatementKind Kind => StatementKind.Expression;
        public ExpressionStatement(StatementExpression expression)
        {
            Expression = expression;
        }

        public StatementExpression Expression { get; }

        public override string ToString()
        {
            return string.Format("Expression {{ Expression: {0} }}", Expression);
        }
    }
    public class ReturnStatement : Statement, IExpressionStatement
    {
        public StatementKind Kind => StatementKind.Expression;
        public ReturnStatement(StatementExpression expression)
        {
            Expression = expression;
        }

        public StatementExpression Expression { get; }

        public override string ToString()
        {
            return string.Format("Expression {{ Expression: {0} }}", Expression);
        }
    }
    public class FunctionDeclarationStatement : Statement, IFunctionDeclarationStatement
    {
        public StatementKind Kind => StatementKind.Function;

        public FunctionDeclarationStatement(string name, List<Parameter> parameters, List<Statement> body)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Parameters = parameters;
            Body = body;
        }

        public string Name { get; }

        public List<Parameter> Parameters { get; }

        public List<Statement> Body { get; }

        public override string ToString()
        {
            return string.Format("{{ Name: {0}, Parameters: [{1}], Body: [{2}] }}", Name, string.Join(", ", Parameters), string.Join(", ", Body));
        }
    }
    public class StructDeclarationStatement : Statement, IStructDeclarationStatement
    {
        public StatementKind Kind => StatementKind.Struct;

        public StructDeclarationStatement(string name, List<Parameter> fields)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Fields = fields;
        }

        public string Name { get; }
        public List<Parameter> Fields { get; }
        public override string ToString()
        {
            return string.Format("{{ Name: {0}, Fields: [{1}]}}", Name, string.Join(", ", Fields));
        }
    }
    public interface IIfStatement
    {
        StatementExpression Condition { get; }
        List<Statement> Then { get; }
        List<Statement> Otherwise { get; }
    }
    public class IfStatement : Statement, IIfStatement
    {
        public IfStatement(StatementExpression condition, List<Statement> then, List<Statement> otherwise)
        {
            Condition = condition;
            Then = then;
            Otherwise = otherwise;
        }
        public StatementExpression Condition { get; }
        public List<Statement> Then { get; }
        public List<Statement> Otherwise { get; }

        public override string ToString()
        {
            return string.Format("{{ Condition: {0}, Parameters: [{1}], Body: [{2}] }}", Condition, string.Join(", ", Then), string.Join(", ", Otherwise));
        }
    }
    public abstract class Statement
    {
        protected Statement() { }


    }
}
