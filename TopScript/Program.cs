﻿using TopScript;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("TopScript interpreted programming language");
        Console.WriteLine();
        Console.WriteLine();
        var source = File.ReadAllText("test1.top");
        var lexer = new Lexer(source);

        //while (true)
        //{
        //    var token = lexer.Lex();
        //    Console.WriteLine(token);
        //    if (token.kind == TokenKind.EOFToken) break;

        //}

        var parser = new Parser(lexer);
        var program = parser.Parse();

        Console.WriteLine(program);

        Console.ReadKey();
    }
}