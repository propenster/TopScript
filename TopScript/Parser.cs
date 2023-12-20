using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public Statement ParseStatement()
        {
            //Next();
            Statement statement = default;
            if (_CurrentToken.kind != TokenKind.EOFToken && _CurrentToken.kind == TokenKind.Var)
            {
                //parse out a var statement
                statement = ParseVarDeclaration();
                //statement = ParseVar();
            }
            else if (_CurrentToken.kind != TokenKind.EOFToken && _CurrentToken.kind == TokenKind.For)
            {
                statement = ParseFor();
            }
            else if (_CurrentToken.kind != TokenKind.EOFToken && _CurrentToken.kind == TokenKind.If)
            {
                statement = ParseIf();
            }
            else if (_CurrentToken.kind != TokenKind.EOFToken && _CurrentToken.kind == TokenKind.Function)
            {
                statement = ParseFunction(true);
            }


            return statement;
        }
        public AstProgram Parse()
        {
            var program = new AstProgram();
            var statements = new List<Statement>();

            while (true)
            {

                Next();
                var Nextt = _NextToken;
                if (_CurrentToken.kind == TokenKind.EOFToken) break;
                var stmt = ParseStatement();
                if (stmt is null) break;
                statements.Add(stmt);

            }
            program.Statements = statements;

            return program;
        }

        private Token ExpectTokenAndRead(TokenKind token)
        {
            //Next();
            var result = ExpectTokenAndThrows(token);

            Next();

            return result;
        }
        private Token ExpectIdentifierAndRead()
        {
            return ExpectTokenAndRead(TokenKind.Identifier);
        }

        private Token ExpectTokenAndThrows(TokenKind token)
        {
            if (_CurrentToken.kind == token) return _CurrentToken;
            throw new UnexpectedTokenException(_CurrentToken.literal.ToString());
        }

        private Statement ParseFunction(bool hasIdentifier)
        {
            ExpectTokenAndRead(TokenKind.Function);
            var name = hasIdentifier ? ExpectIdentifierAndRead()?.literal : "<Closure>";
            ExpectTokenAndRead(TokenKind.LeftParen);
            //now gather the params
            var parameters = new List<Parameter>();
            while (_CurrentToken.kind != TokenKind.EOFToken && _CurrentToken.kind != TokenKind.RightParen)
            {
                if (_CurrentToken.kind == TokenKind.Comma) ExpectTokenAndRead(TokenKind.Comma);
                var param = ExpectIdentifierAndRead()?.literal.ToString();
                parameters.Add(new Parameter(param));
            }

            ExpectTokenAndRead(TokenKind.RightParen);

            var body = ParseBlock();

            return new FunctionDeclarationStatement(name.ToString(), parameters, body);
        }

        private Statement ParseIf()
        {
            ExpectTokenAndRead(TokenKind.If);
            var condition = ParseExpression2(Precedence.Statement);
            var then = ParseBlock();
            var otherwise = new List<Statement>();
            if(_CurrentToken.kind == TokenKind.Else)
            {
                ExpectTokenAndRead(TokenKind.Else);
                otherwise = ParseBlock();
            }
            else
            {
                otherwise = null;
            }


            return new IfStatement(condition, then, otherwise);

        }
        private List<Statement> ParseBlock()
        {
            ExpectTokenAndRead(TokenKind.LeftCurlyBrace);
            var block = new List<Statement>();

            while (_CurrentToken.kind != TokenKind.RightCurlyBrace)
            {
                block.Add(ParseStatement());
            }
            ExpectTokenAndRead(TokenKind.RightCurlyBrace);
            return block;
        }

        private Statement ParseFor()
        {
            ExpectTokenAndRead(TokenKind.For); //now let's advance...

            if (_CurrentToken.kind != TokenKind.EOFToken && _CurrentToken.kind == TokenKind.LeftParen)
            {
                ExpectTokenAndRead(TokenKind.LeftParen);
                var index = ExpectIdentifierAndRead();

            }


            return null;


        }

        private Statement ParseVar()
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
            return varStatement;
        }
        private Statement ParseVarDeclaration()
        {
            ExpectTokenAndRead(TokenKind.Var);
            var name = ExpectIdentifierAndRead().ToString();
            StatementExpression initial = default;
            if(_CurrentToken.kind== TokenKind.Assign)
            {
                ExpectTokenAndRead(TokenKind.Assign);
                initial = ParseExpression2(Precedence.Lowest);
            }
            else
            {
                initial = null;
            }
            var current = _CurrentToken;
            return new VarStatement(name, initial);
        }
        private StatementExpression ParseExpression2(Precedence precedence)
        {
            StatementExpression left = default;
            switch (_CurrentToken.kind)
            {
                case TokenKind.StringLiteral:
                    ExpectTokenAndRead(TokenKind.StringLiteral);
                    left = new StringExpression(_CurrentToken.literal.ToString());
                    break;

                case TokenKind.Null:
                    ExpectTokenAndRead(TokenKind.Null);
                    left = new NullExpression();
                    break;

                case TokenKind.NumericLiteral:
                    var number = (double)_CurrentToken.literal;
                    ExpectTokenAndRead(TokenKind.NumericLiteral);
                    left = new NumericExpression(number);
                    break;

                case TokenKind.True:
                    ExpectTokenAndRead(TokenKind.True);
                    left = new BooleanExpression(true);
                    break;
                case TokenKind.False:
                    ExpectTokenAndRead(TokenKind.False);
                    left = new BooleanExpression(false);
                    break;

                case TokenKind.Identifier:
                    ExpectIdentifierAndRead();
                    left = new IdentifierExpression(_CurrentToken.literal.ToString());
                    break;

                case TokenKind.Function:
                    var functionStatement = ParseFunction(false) as FunctionDeclarationStatement;
                    left = new ClosureExpression(functionStatement?.Parameters, functionStatement?.Body);
                    break;

                case TokenKind.Minus:
                    ExpectTokenAndRead(TokenKind.Minus);
                    left = new PrefixExpression(_CurrentToken, ParseExpression2(Precedence.Prefix));
                    break;

                case TokenKind.Not:
                    ExpectTokenAndRead(TokenKind.Not);
                    left = new PrefixExpression(_CurrentToken, ParseExpression2(Precedence.Prefix));
                    break;

                case TokenKind.OpenSquareBracket: //[1,2,3]
                    ExpectTokenAndRead(TokenKind.OpenSquareBracket);
                    var items = new List<StatementExpression>();
                    while (_CurrentToken.kind != TokenKind.CloseSquareBracket)
                    {
                        items.Add(ParseExpression2(Precedence.Lowest));
                        if (_CurrentToken.kind == TokenKind.Comma)
                        {
                            ExpectTokenAndRead(TokenKind.Comma);
                        }
                    }
                    ExpectTokenAndRead(TokenKind.CloseSquareBracket);

                    left = new ListExpression(items);
                    break;

                default:
                    throw new UnexpectedTokenException(_CurrentToken.literal.ToString());

            }
            while (_CurrentToken.kind != TokenKind.EOFToken && (int)precedence < (int)ConvertTokenKindToPrecedence(_CurrentToken.kind))
            {
                StatementExpression expP = ParsePostfixExpression(left);
                StatementExpression expIn = ParseInfixExpression(left);

                if (expP != null) { left = expP; }
                else if (expIn != null)
                {
                    left = expIn;
                }
                else
                {
                    break;
                }

            }


            return left;

        }

        private StatementExpression ParsePostfixExpression(StatementExpression left)
        {
            //StatementExpression left = default;
            switch (_CurrentToken.kind)
            {
                case TokenKind.Dot:
                    ExpectTokenAndRead(TokenKind.Dot);
                    var field = ExpectIdentifierAndRead().ToString();
                    return new GetExpression(left, field);

                case TokenKind.OpenSquareBracket:
                    ExpectTokenAndRead(TokenKind.OpenSquareBracket);
                    var index = _CurrentToken.kind == TokenKind.CloseSquareBracket ? null : ParseExpression2(Precedence.Lowest);
                    ExpectTokenAndRead(TokenKind.CloseSquareBracket);
                    return new ListIndexExpression(left, index);


                case TokenKind.LeftCurlyBrace:
                    ExpectTokenAndRead(TokenKind.LeftCurlyBrace);
                    var fields = new Dictionary<string, StatementExpression>();
                    while (_CurrentToken.kind != TokenKind.RightCurlyBrace)
                    {
                        var fieldKey = ExpectIdentifierAndRead().ToString();
                        StatementExpression value = default;
                        if (_CurrentToken.kind == TokenKind.SemiColon)
                        {
                            ExpectTokenAndRead(TokenKind.SemiColon);
                            value = ParseExpression2(Precedence.Lowest);
                        }
                        else
                        {
                            value = new IdentifierExpression(fieldKey);
                        }

                        fields.Add(fieldKey, value);
                        if (_CurrentToken.kind == TokenKind.Comma) Next();

                    }
                    ExpectTokenAndRead(TokenKind.RightCurlyBrace);
                    return new StructExpression(left, fields);


                case TokenKind.LeftParen:
                    ExpectTokenAndRead(TokenKind.LeftParen);
                    var args = new List<StatementExpression>();
                    while (_CurrentToken.kind != TokenKind.RightParen)
                    {
                        args.Add(ParseExpression2(Precedence.Lowest));
                        if (_CurrentToken.kind == TokenKind.Comma) Next();
                    }
                    ExpectTokenAndRead(TokenKind.RightParen);
                    return new CallExpression(left, args);


                default:
                    return null;
            }
        }
        private StatementExpression ParseInfixExpression(StatementExpression left)
        {
            switch (_CurrentToken.kind)
            {
                case TokenKind.Plus:
                case TokenKind.Minus:
                case TokenKind.Multiply:
                case TokenKind.Divide:
                case TokenKind.Equality:
                case TokenKind.NotEqual:
                case TokenKind.LessThanOrEquals:
                case TokenKind.LessThan:
                case TokenKind.GreaterThan:
                case TokenKind.GreaterThanOrEquals:
                case TokenKind.And:
                case TokenKind.Or:

                    var token = _CurrentToken;
                    Next();
                    var right = ParseExpression2(ConvertTokenKindToPrecedence(token.kind));

                    return new InfixExpression(left, ConvertTokenKindToOperand(token?.kind), right);

                case TokenKind.Assign:
                    Next();
                    var rght = ParseExpression2(Precedence.Lowest);
                    return new AssignmentExpression(left, rght);

                default:
                    return null;
            }
        }

        private Op ConvertTokenKindToOperand(TokenKind? kind)
        {
            switch (kind)
            {
                case TokenKind.Plus: return Op.Add;
                case TokenKind.Minus: return Op.Subtract;
                case TokenKind.Multiply: return Op.Multiply;
                case TokenKind.Divide: return Op.Divide;
                case TokenKind.Not: return Op.NotEquals;
                case TokenKind.Modulo: return Op.Modulo;
                case TokenKind.Equality: return Op.Equals;
                case TokenKind.Assign: return Op.Assign;
                case TokenKind.LessThan: return Op.LessThan;
                case TokenKind.GreaterThan: return Op.GreaterThan;
                case TokenKind.LessThanOrEquals: return Op.LessThanOrEquals;
                case TokenKind.GreaterThanOrEquals: return Op.GreaterThanOrEquals;
                case TokenKind.And: return Op.And;
                case TokenKind.Or: return Op.Or;
                case TokenKind.Pow: return Op.Pow;

                default: throw new NotImplementedException();
            }
        }

        private Precedence ConvertTokenKindToPrecedence(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Multiply:
                case TokenKind.Divide:
                    return Precedence.Product;

                case TokenKind.Plus:
                case TokenKind.Minus:
                    return Precedence.Sum;

                case TokenKind.LeftParen:
                case TokenKind.Dot:
                case TokenKind.OpenSquareBracket:
                    return Precedence.Call;

                case TokenKind.LessThan:
                case TokenKind.GreaterThan:
                case TokenKind.LessThanOrEquals:
                case TokenKind.GreaterThanOrEquals:
                    return Precedence.LessThanGreaterThan;


                case TokenKind.Equality:
                case TokenKind.NotEqual:
                    return Precedence.Equals;

                case TokenKind.And:
                case TokenKind.Or:
                    return Precedence.AndOr;

                case TokenKind.Assign:
                    return Precedence.Assign;

                case TokenKind.LeftCurlyBrace:
                    return Precedence.Statement;

                case TokenKind.Pow:
                    return Precedence.Pow;

                default:
                    return Precedence.Lowest;
            }


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
            switch (op)
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
