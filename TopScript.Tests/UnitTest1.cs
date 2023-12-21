namespace TopScript.Tests
{
    public class UnitTest1
    {


        [Fact]
        public void ItCanRecogniseReservedKeywords()
        {
            var source = "var function true false while for";
            var lexer = new Lexer(source);
            Assert.Equal(new Token(TokenKind.Var, "var"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Function, "function"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.True, "true"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.False, "false"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.While, "while"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.For, "for"), lexer.Lex());
        }
        [Fact]
        public void ItCanRecogniseNumbers()
        {
            var source = "123 28 1000.45 2_000_000";
            var lexer = new Lexer(source);
            Assert.Equal(new Token(TokenKind.NumericLiteral, 123.0), lexer.Lex());
            Assert.Equal(new Token(TokenKind.NumericLiteral, 28.0), lexer.Lex());
            Assert.Equal(new Token(TokenKind.NumericLiteral, 1000.45), lexer.Lex());
            Assert.Equal(new Token(TokenKind.NumericLiteral, 2000000.0), lexer.Lex());
        }
        [Fact]
        public void ItCanRecogniseIdentifiers()
        {
            var source = "foo bar helloWorld age myInt myFloat myVariable";
            var lexer = new Lexer(source);
            Assert.Equal(new Token(TokenKind.Identifier, "foo"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Identifier, "bar"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Identifier, "helloWorld"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Identifier, "age"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Identifier, "myInt"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Identifier, "myFloat"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Identifier, "myVariable"), lexer.Lex());
            
        }
        [Fact]
        public void ItCanRecogniseStringLiterals()
        {
            var source = "\"foo\" \"bar\" \"helloWorld\" \"age\" \"myInt\" \"myFloat\" \"myVariable\"";
            var lexer = new Lexer(source);
            Assert.Equal(new Token(TokenKind.StringLiteral, "foo"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.StringLiteral, "bar"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.StringLiteral, "helloWorld"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.StringLiteral, "age"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.StringLiteral, "myInt"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.StringLiteral, "myFloat"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.StringLiteral, "myVariable"), lexer.Lex());

        }
        [Fact]
        public void ItCanRecogniseSymbols()
        {
            var source = "( ) { } [ ] , : + * - / ! |";
            var lexer = new Lexer(source);
            Assert.Equal(new Token(TokenKind.LeftParen, "("), lexer.Lex());
            Assert.Equal(new Token(TokenKind.RightParen, ")"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.LeftCurlyBrace, "{"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.RightCurlyBrace, "}"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.OpenSquareBracket, "["), lexer.Lex());
            Assert.Equal(new Token(TokenKind.CloseSquareBracket, "]"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Comma, ","), lexer.Lex());
            
            Assert.Equal(new Token(TokenKind.Colon, ":"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Plus, "+"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Multiply, "*"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Minus, "-"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Divide, "/"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Not, "!"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Pipe, "|"), lexer.Lex());

        }
        [Fact]
        public void Test1()
        {
            var source = "var foo = 28";
            var lexer = new Lexer(source);

            Assert.Equal(new Token(TokenKind.Var, "var"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Identifier, "foo"), lexer.Lex());
            Assert.Equal(new Token(TokenKind.Assign, "="), lexer.Lex());
            Assert.Equal(new Token(TokenKind.NumericLiteral, 28.0), lexer.Lex());
        }
    }
}