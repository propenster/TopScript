using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopScript
{
    public enum TokenKind
    {
        Identifier,
        ReservedKeyword,
        Number,
        Var,
        Assign,



        LeftCurlyBrace,
        RightCurlyBrace,
        Comma,
        Colon,
        SemiColon,
        OpenSquareBracket,
        CloseSquareBracket,


        SingleQuote,

        StringLiteral,
        NumericLiteral,
        LeftParen,
        RightParen,




        EOFToken,
        While,
        For,
        Function,




        Plus,
        Minus,
        Divide,
        Multiply,
        Not,
        Pipe,
        If,
        Else,
        In,
        True,
        False,
        Null,
        Modulo,
        Equality,
        LessThan,
        GreaterThan,
        LessThanOrEquals,
        GreaterThanOrEquals,
        And,
        Or,
        Ampersand,
        NotEqual,
        Dot,
        Pow,
        EndLine,
        Struct,
        Return,
    }
}
