using TopScript;

internal class Program
{
    private static void Main(string[] args)
    {
        var isInt = int.TryParse("23.58", out int res);


        Console.WriteLine("TopScript interpreted programming language");
        Console.WriteLine();
        Console.WriteLine();
        var sourcePath = "examples/functions.top";
        var source = File.ReadAllText(sourcePath);
        var lexer = new Lexer(source);

        //while (true)
        //{
        //    var token = lexer.Lex();
        //    Console.WriteLine(token);
        //    if (token.kind == TokenKind.EOFToken) break;

        //}
        var c = char.IsLetter('(');
        var parser = new Parser(lexer);
        var program = parser.Parse();

        var interpreter = new Interpreter(program, sourcePath);
        interpreter.Interprete();

        Console.WriteLine(program);

        Console.ReadKey();
    }
}