# A very short intro to F#

F# is a multi-paradigm language with a functional-first mindset. It is strongly typed and includes features like immutability by default, type inference, and functions as first-class citizens. The language is in the [ML](https://en.wikipedia.org/wiki/ML_(programming_language)) family, similar to [OCaml](https://en.wikipedia.org/wiki/OCaml), and runs on .NET.

## Order of code and files

F# requires files and code within each file to be organised in dependency order. For instance, a function in a file must be defined above the place where you want to use it.

## Syntax

This is a short walkthrough of basic F# syntax, with an emphasis on what we use in the workshop.
In F#, indentation is used to define code blocks, similar to Python. That means no semicolons, and fewer parentheses and curly braces.

### Values

The keyword `let` is used to bind a value to a name. The type of the value is inferred, but explicit type annotations can be added.

```fsharp
let x = 2
let greeting = "Hello"
```

### Functions

Functions are not so different from values, hence also defined with the keyword `let`. 
Function parameters are defined after the function name, and the types of the parameters and the return value are inferred.
All functions return a value: the last expression in the function body.

```fsharp
let double a = 2 * a
let add a b = 
    a + b
```

#### Pipe operator

The pipe forward operator `|>` takes a value and a function, and applies the function with the value as argument. This operator is useful for piping data through a series of functions, and is the reason why you might see F# functions with the most significant value as the last parameter.

```fsharp
2
|> (+) 4
|> (*) 2
```

The parentheses around `+` are needed to use the infix operator `a + b` as the prefix function `(+) a b`.

#### Function composition

The function composition operator `>>` takes two functions, `f` and `g`, as arguments, and `(f >> g) x` is the same as `g(f(x))`.

```fsharp
add 3 >> double
```

#### Recursion

To be able to use the name of a function inside its own function body, the keyword `rec` must be used in the declaration of the function.
A recursive function consists of two main parts: base case(s), where the function returns a direct result without recursive calls, and a recursive case, where the result is calculated using a call to the function itself. The argument passed to the recursive call should move toward the base case(s).

```fsharp
let rec factorial n =
    if n = 0 then 1 
    else n * factorial (n - 1)
```

### Lists

`List` is one of the collection types in F#, and it is an immutable, singly linked list.

To create a list:

```fsharp
let numbers = [1; 2; 3; 4]
```

Add an element to the start of the list with `::`:

```fsharp
let numbers = 1 :: [2; 3; 4]
```
Join two lists into one with `@`:
```fsharp
let numbers = [1; 2] @ [3; 4]
```

The [List module](https://fsharp.github.io/fsharp-core-docs/reference/fsharp-collections-listmodule.html) contains many useful operations for working with lists, like  `List.filter`, `List.map`, `List.replicate` and `List.collect`.

```fsharp
[1; 2; 3; 4]
|> List.map ((+) 3) // [4; 5; 6; 7]
|> List.filter (fun n -> n > 5) // [6; 7]
|> List.replicate 2 // [[6; 7]; [6; 7]]
```

### Algebraic data types

F# has very useful types that make it easy to define a domain precisely and concisely.

#### Product types

A product type is a type where each member must contain a certain set of values. F# has tuples and records as product types.

Tuples are defined by values separated by commas. The tuple `(1, 2)` has type `int * int`, and `("Bob", 42)` has type `string * int`.

```fsharp
let point = (1, 2)
let (x, y) = point  // x = 1, y = 2
// fst and snd work on tuples with two elements
fst point // 1
snd point // 2
```

Records are like tuples, but with named fields.

```fsharp
type Person = { Name: string; Age: int }
let bob = { Name = "Bob"; Age = 42 }
bob.Name
```

#### Sum types

A sum type, or discriminated union, is a type where a member of the type is exactly one of one or more possible values. In the case of `Shape` below, a member of this type is either a circle or a rectangle.

```fsharp
type Shape =
    | Circle of float
    | Rectangle of float * float

let circle = Circle 4.0
let rectangle = Rectangle (3.0, 4.0)
```

### Pattern matching

The `match` expression is a way to decompose data and extract information from wrapped values.

```fsharp
let area shape =
    match shape with
    | Circle r -> Math.PI * r * r
    | Rectangle (a, b) ->  a * b
```

```fsharp
let rec sum l =
    match l with
    | [] -> 0
    | x :: xs -> x + sum xs
```

The compiler gives a warning, `warning FS0025: Incomplete pattern matches on this expression.`, if pattern matching is not exhaustive.

### Conditionals

To control the execution of code based on the result of boolean expressions, an if-else-expression can be used.
Each branch of the expression must return a value of the same type.

```fsharp
let test x = 
    if x > 0 then "positive"
    elif x < 0 then "negative"
    else "zero"
```

### Comments
Single-line comments start with `//`, and block comments are text between `(* *)`.
```fsharp
// Single-line comment

(*
    Multi-line comments
    Useful for temporarily commenting out code sections
*)
```

### Print to stdout

```fsharp
let circle = Circle 3.0
printfn "Shape: %A" circle

// with string interpolation
printfn $"Shape: {circle}"
```

## Learn more

* [The F# Documentation](https://learn.microsoft.com/en-us/dotnet/fsharp/)
* [Learning F# (fsharp.org)](https://fsharp.org/learn/)
* [Learn F# (dotnet.microsoft.com)](https://dotnet.microsoft.com/en-us/learn/fsharp)
* [https://fsharpforfunandprofit.com/](https://fsharpforfunandprofit.com/)




