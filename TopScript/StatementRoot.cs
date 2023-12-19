using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
    public abstract class Statement
    {
        protected Statement() { }


    }
}
