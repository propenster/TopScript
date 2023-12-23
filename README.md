# TopScript

**A dynamic interpreted scripting language written from scratch in C#**

**This is to confirm the fact that there is now absolutely nothing (sounding overly confident) that I have not built using C# (the most elegant high-level language there is)** - Ratio this 🪨 😲 😆 💩

- Lexing for dummies
- Parsing for dummies
- AST
- Runtime from scratch

## References 
- [Wikipedia Parsing](https://en.wikipedia.org/wiki/Parsing)
- [Writing an interpreter in Go by Thorsten Ball](https://interpreterbook.com/)
- [Inspired by Lagoon by Ryan Chandler my Rust buddy](https://github.com/ryangjchandler/lagoon)
- Watch my [Create a JSON parser from scratch in C# video](https://www.youtube.com/watch?v=mAYgIPCc1vs&list=PL0DHMcUfPntZ9yLUJ7vi9H6jGz0dJyGJU)

## Features
- Nothing special just another programming language but we stole a few things from C#, JavaScript and Rust.
- Check the examples folder for examples
- Update December 23rd 2023 - We are able to parse most grammar and our interpreter is working for variables, var, functions and other statements.
- Addition of a StandardLibrary which contains fundamental objects like the StringObject, ListObject, NumberObject, and Common which has utility native methods like print, println, range(int,int), and load (for including modules)

## Examples

**1. Hello World**
Check [print.top]() file in examples folder on this repo
```
print("Hello, World!");
println("Hello, somebody from github");

```

**2. Variables and Var Declaration**
Check [variables.top]() file in examples folder on this repo
```
var age = 28;
var name = "Faith";

var amount = 2_000_000;

println("Age is: " + age);
println("Name is: " + name);
println("Amount is: " + amount);

```

**3. Functions**
Check [functions.top]() file in examples folder on this repo
```
function sayHello(name) {
    println("Hello, " + name);
}

function greetings(player){
print("Welcome player: " + player + " to the game of life called chess");
}

sayHello("Faith");
greetings("John Doe");

```

**4. Loop - For**

```
for(i in range(10)){
    println(i);
}
```



©️ propenster 2023
