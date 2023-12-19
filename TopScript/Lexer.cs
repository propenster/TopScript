using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopScript
{
    public class Lexer
    {
        public List<Token> _Tokens { get; set; }
        public string _Source { get; }
        private char _CurrentChar;
        private char _NextChar;
        private int _Position;

        public Lexer(string source)
        {
            _Source = source ?? throw new ArgumentNullException(nameof(source));
            _Tokens = new List<Token>();    

            _Position = -2;

            _CurrentChar = '\0';
            _NextChar = '\0';

            Next();
        }

        public void ConsumeWhiteSpace()
        {
            while(_CurrentChar != '\0' && char.IsWhiteSpace(_CurrentChar))
            {
                Next();
            }
        }
        public void Next()
        {
            _Position++;
            _CurrentChar = _NextChar;

            if (_Position <= (_Source.Length - 2))
            {
                _NextChar = _Source[_Position+1];
            }
            else
            {
                _NextChar = '\0';
            }
        }

        public Token Lex()
        {
            var token = new Token(TokenKind.EOFToken, "\0");
            Next();
            ConsumeWhiteSpace();
            var currentChar = _CurrentChar;
            var charString = currentChar.ToString();
            if (currentChar == '\0')
            {
                token = new Token(TokenKind.EOFToken, charString);
                //_Tokens.Add(token);
                return token;
            }
            else if(char.IsLetter( currentChar)) //possible start of a reserved keyWord - var
            {
                var sb = new StringBuilder();
                var buffer = string.Empty;
                while(_CurrentChar != '\0' && char.IsLetter(_CurrentChar))
                {
                    sb.Append(_CurrentChar);
                    Next();
                }
                buffer = sb.ToString();
                if(IsReservedKeyword(buffer, out var kind))
                {
                    //this is almost probably var keyword...
                    //return MakeReservedKeyword();
                    return new Token(kind, buffer);
                }

                //probably an identifier
                return new Token(TokenKind.Identifier, buffer);
                
            }
            else if(currentChar == '=')
            {
                return new Token(TokenKind.Assign, charString);
            }
            else if (currentChar == '\"')
            {
                return MakeStringLiteral();
            }
            else if (char.IsDigit(currentChar))
            {
                return MakeNumericLiteral();
            }else if(currentChar == '+')
            {
                return new Token(TokenKind.Plus, charString);
            }else if(currentChar == '-')
            {
                return new Token(TokenKind.Minus, charString);
            }else if(currentChar == '*')
            {
                return new Token(TokenKind.Multiply, charString);   
            }else if(currentChar == '/')
            {
                return new Token(TokenKind.Divide, charString);
            }
            else if (currentChar == ':')
            {
                return new Token(TokenKind.SemiColon, charString);
            }
            else if (currentChar == ',')
            {
                return new Token(TokenKind.Comma, charString);
            }
            else if (currentChar == '{')
            {
                return new Token(TokenKind.LeftCurlyBrace, charString);
            }
            else if (currentChar == '}')
            {
                return new Token(TokenKind.RightCurlyBrace, charString);
            }
            else if (currentChar == '(')
            {
                return new Token(TokenKind.LeftParen, charString);
            }
            else if (currentChar == ')')
            {
                return new Token(TokenKind.RightParen, charString);
            }
            else if (currentChar == '[')
            {
                return new Token(TokenKind.OpenSquareBracket, charString);
            }
            else if (currentChar == ']')
            {
                return new Token(TokenKind.CloseSquareBracket, charString);
            }else if(currentChar == '!')
            {
                return new Token(TokenKind.Not, charString);
            }
            else if (currentChar == '|')
            {
                return new Token(TokenKind.Pipe, charString);
            }



            return new Token(TokenKind.EOFToken, "\0");

        }
        private bool IsReservedKeyword(string keyword, out TokenKind kind)
        {
            
            //var kind = TokenKind.EOFToken;
            switch(keyword.ToLowerInvariant())
            {
                case "var":
                    kind = TokenKind.Var;
                    break;

                case "for":
                    kind = TokenKind.For;
                    break;
                case "while":
                    kind =  TokenKind.While;
                    break;

                case "true":
                case "false":
                    kind = TokenKind.BooleanLiteral;
                    break;

                case "function":
                    kind = TokenKind.Function;
                    break;
               
                default:
                    kind = TokenKind.EOFToken;
                    break;
                
            }

            if(kind != TokenKind.EOFToken)
            {
                return true;
            }

            return false;


        }
        private Token MakeVarKeyword(int currentPos)
        {
            while (_CurrentChar != '\0' && _CurrentChar != 'r')
            {
                Next();
            }
            return new Token(TokenKind.Var, _Source.Substring(currentPos, _Position - currentPos));
        }


        private Token MakeNumericLiteral()
        {
            var currentPos = _Position;
            var dots = 0;
            while (_CurrentChar != '\0' && "0123456789._".Contains(_NextChar))
            {
                if (_CurrentChar == '.')
                {
                    dots++;
                }
                Next();
            }
            if (dots > 1) throw new FormatException("Invalid number format");
            var numberValue = _Source.Substring(currentPos, _Position - currentPos + 1);
            var cleaned = numberValue.Replace("_", "");
            //if (dots == 0)
            //{

            //    return new Token(TokenKind.NumericLiteral, int.Parse(cleaned));
            //}
            return new Token(TokenKind.NumericLiteral, double.Parse(cleaned));
        }



        private Token MakeStringLiteral()
        {
            Next();
            var currentPos = _Position;
            while(_CurrentChar != '\0' && _CurrentChar != '\"')
            {
                Next();
            }

            var literal = _Source.Substring(currentPos, _Position - currentPos );

            return new Token(TokenKind.StringLiteral, literal);
        }
    }
}
