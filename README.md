# TopScript

**A dynamic interpreted scripting language written from scratch in C#**

**This is to confirm the fact that there is now absolutely nothing (sounding overly confident) that I have not built using C# (the most elegant high-level language there is)** - Ratio this 🪨 😲 😆 💩

- Lexing for normal people
- Parsing for normal people
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

All examples can be found in the [examples](https://github.com/propenster/TopScript/tree/master/TopScript/examples) directory on this repository.

**1. Hello World**
Check [print.top](https://github.com/propenster/TopScript/tree/master/TopScript/examples/print.top) file in examples folder on this repo
```
print("Hello, World!");
println("Hello, somebody from github");

```

**2. Variables and Var Declaration**
Check [variables.top](https://github.com/propenster/TopScript/tree/master/TopScript/examples/variables.top) file in examples folder on this repo
```
var age = 28;
var name = "Faith";

var amount = 2_000_000;

println("Age is: " + age);
println("Name is: " + name);
println("Amount is: " + amount);

```

**3. Functions**
Check [functions.top](https://github.com/propenster/TopScript/tree/master/TopScript/examples/functions.top) file in examples folder on this repo
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
Check [forloop.top](https://github.com/propenster/TopScript/tree/master/TopScript/examples/forloop.top) file in examples folder on this repo
```
for(i in range(10)){
    println(i);
}

```


## Standard Library

**Strings - StringObject**
Check [strings.top](https://github.com/propenster/TopScript/tree/master/TopScript/examples/strings.top) file in examples folder on this repo
```
var name = "Tolu";

println(name);

var upperCaseText = "This should be in upperCase".to_upper();
var nameToUpper = name.to_upper();


println(upperCaseText);
println(nameToUpper);

var stringContainsWordHello = "World, Hello, Welcome to my wonderland".contains("Hello");
println("String contains hello = " + stringContainsWordHello);


var appendString = "Hello,".append(" World");
println(appendString);

var stringStartsWithHello = "World, Hello, Welcome to my wonderland".starts_with("Hello");
println("Does string start with Hello ? " + stringStartsWithHello);

var nullString = "";
println("Is nullString null or whitespace ? " + nullString.is_null_or_whitespace());

```




©️ propenster 2023
