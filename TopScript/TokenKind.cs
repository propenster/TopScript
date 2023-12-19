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
        BooleanLiteral,
        Function,




        Plus,
        Minus,
        Divide,
        Multiply,
        Not,
        Pipe,
    }
}
