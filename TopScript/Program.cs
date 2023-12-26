using TopScript;

internal class Program
{
    private static void Main(string[] args)
    {
        var isInt = int.TryParse("23.58", out int res);

        //variables, functions, strings, 
        var sourcePath = "examples/strings.top";
        var source = File.ReadAllText(sourcePath);

        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var program = parser.Parse();

        var interpreter = new Interpreter(program, sourcePath);
        interpreter.Interprete();

        //Console.WriteLine(program); -> AST

        Console.ReadKey();
    }
}