using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopScript.Exceptions;

namespace TopScript
{
    public class Parser
    {
        public Lexer _Lexer { get; set; }
        private Token _CurrentToken;
        private Token _NextToken;
        public Parser(Lexer lexer)
        {
            _Lexer = lexer;
            _CurrentToken = new Token(TokenKind.EOFToken, "\0");
            _NextToken = new Token(TokenKind.EOFToken, "\0");

            Next();

        }

        private void Next()
        {
            _CurrentToken = _NextToken;
            _NextToken = _Lexer.Lex();
        }

        public AstProgram Parse()
        {
            var program = new AstProgram();
            var statements = new List<Statement>();

            while (true)
            {

                Next();
                if (_CurrentToken.kind == TokenKind.EOFToken) break;

                if (_CurrentToken.kind != TokenKind.EOFToken && _CurrentToken.kind == TokenKind.Var)
                {
                    //parse out a var statement
                    Next();
                    var identifier = _CurrentToken;
                    //Console.WriteLine(identifier);

                    if (identifier.kind != TokenKind.Identifier)
                    {
                        throw new SyntaxException($"Expected identifier after \"var\" statement found {identifier.kind}");
                    }
                    if (_NextToken.kind != TokenKind.Assign)
                    {
                        throw new SyntaxException($"Expected assignment operator \"=\" found {_NextToken.kind}");
                    }
                    Next();

                    StatementExpression expression = ParseExpression(0);
                    if (expression is null) throw new SyntaxException("Could not parse expression");

                    // Console.WriteLine(expression.ToString());

                    





                    var varStatement = new VarStatement((string)identifier.literal, expression);

                    statements.Add(varStatement);

                }

            }

            program.Statements = statements;



            return program;


        }

        private StatementExpression ParseExpression(byte bindingPower)
        {
            StatementExpression lhs = default;
            StatementExpression infix_ex = default;
            Next();
            if (_CurrentToken.kind != TokenKind.EOFToken && _CurrentToken.kind == TokenKind.NumericLiteral)
            {
                lhs = new NumericExpression((double)_CurrentToken.literal);
            }
            while (_CurrentToken.kind != TokenKind.EOFToken)
            {
                var infix = _NextToken;
                if (infix.kind == TokenKind.EOFToken) break;

                var (lbp, rbp) = BindingPower(infix.kind);
                if ((lbp, rbp) == (0, 0)) break;
                //if (lbp < rbp) break;

                //get next operator
                Next();

                var op = _CurrentToken;

                var rhs = ParseExpression(rbp);

                lhs = MakeInfixExpression(lhs, op.kind, rhs);

                continue;
            }
           
            


            return lhs;
        }

        private StatementExpression? MakeInfixExpression(StatementExpression? lhs, TokenKind op, StatementExpression rhs)
        {
            switch(op)
            {
                case TokenKind.Plus:
                    return new BinaryExpression(lhs, BinaryOperand.Plus, rhs);

                    case TokenKind.Minus:
                    return new BinaryExpression(lhs, BinaryOperand.Minus, rhs);
                    case TokenKind.Multiply:
                    return new BinaryExpression(lhs, BinaryOperand.Multiply, rhs);

                case TokenKind.Divide:
                    return new BinaryExpression(lhs, BinaryOperand.Divide, rhs);

                default:
                    return null;
            }
        }

        private Tuple<byte, byte> BindingPower(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Plus:
                case TokenKind.Minus:
                    return new Tuple<byte, byte>(6, 7);
                case TokenKind.Multiply:
                case TokenKind.Divide:
                    return new Tuple<byte, byte>(8, 9);
                default: return new Tuple<byte, byte>(0, 0);
            }
        }
    }
}
