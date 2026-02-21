# 🐢 Fun with F# and turtle geometry

The content of the workshop is based on content from the first two chapters of [Turtle Geometry: The Computer as a Medium for Exploring Mathematics](https://direct.mit.edu/books/oa-monograph/4663/Turtle-GeometryThe-Computer-as-a-Medium-for) by Harold Abelson and Andrea diSessa.

**🚧 Under construction 🚧**

Getting ready for [Booster 2026](https://www.boosterconf.no/2026/program/wednesday/9_short_talks_and_workshops/dreggen_4/fun-with-f-and-turtle-geometry/)

## 🚀 Getting started

Make sure you have .NET 10 SDK installed by running `dotnet --version` in the terminal. The .NET 10 SDK can be downloaded from https://dotnet.microsoft.com/en-us/download. 

1. Clone the repository
1. Run `dotnet restore` from the project root the first time
1. Run `dotnet watch run`. This will run the app and automatically rebuild the app when you make changes in the code
1. Navigate to <http://localhost:5000> in your browser
1. If you see a square you are good to go! 

### Alternative ways of running the code

There is setup for devcontainer, which can be used to work with the code on GitHub, using GitHub Codespaces, or locally from an IDE like VS Code. 

## 🐢 What is a turtle?

A turtle is a small animal moving around in a plane. The turtle doesn't move randomly, instead it responds to commands. 

The four simple commands we will be using are `Forward`, `Left`, `Right` and `Back`, and they all take an integer as input. For forward and back, the integer is the distance the turtle should move, for left and right, it is the degrees the turtle should rotate. The forward command will cause the turtle to leave a trace the distance it moved, while back does not. Forward and back change the turtles position, while left and right change the direction of the turtle.

In our program we will be most interested in the commands, and less interested in how the turtle executes them. We will create lists of  `TurtleCommands` which each are a complete set of commands the turtle need in order to create a specific path. 

## 🟥 Polygons

### ✍️ Triangle

Our webpage is showing a square, so the natural first step is to make an [equilateral triangle](https://en.wikipedia.org/wiki/Equilateral_triangle). Look at the file `Core.fs`, and the value `square`, which contains a list of turtle commands. Take inspiration from `square`, and make a new value `triangle` containing a new list of commands, consisting of a combination of `Forward` and `Right`. Update `webPagePath` to point to `triangle`, and check how it looks in the browser.

### ✍️ Repeat

Both `square` and `triangle` seem to repeat two commands, four and three times respectively. It might be nice to have a function `repeat` to do the repetitions for us, so let's make it. 

The function should take two arguments, an integer `count` for the number of repetitions,  and a list `commands`, the commands we want to repeat. The first part of the function declaration should look like `let repeat count commands =`, then followed by what should be the function body. Functions are let-expressions, just like for values, and the function arguments follows after the function name, without parenthesis around. (`(count, commands)` is a tuple in F#). 

The function body can be implemented in (at least) two ways. The easiest way is probably to use the functions `List.replicate` and `List.collect`, available from the [List module](https://fsharp.github.io/fsharp-core-docs/reference/fsharp-collections-listmodule.html). Replicate will make a list with the input repeated n times. The result of this will be of type list of list, since our initial input is a list of turtle commands. Collect will flatten the list, and make the elements of the inner list as separate elements of the result list. Collect needs a mapping function as argument, in our case it should be the identity function `id`.

Another way to implement `repeat` is by making a recursive helper function, that keeps track of the remaining repetitions and the accumulated result. The function should test if the counter is zero, then it should return the result, otherwise it should join the result with the input list, and then call itself with the counter decreased, and the accumulated result as the joined list. For this recursive function you will need to know that it must be declared with `let rec <function name>`, in order to be able to call itself, and that two lists can be joined with the `@` operator.

When repeat is finished, test that it works by rewriting square and triangle, and check that it looks as it should.

### ✍️ Poly

Now that we have `repeat` we can generalise `square` and `triangle`, and make a function `poly` that makes the turtle first move forward a given side distance, and then move right a given angle. This pattern repeats until the turtle closes the path it is drawing. We don't know yet when that happens, so for now we will just repeat the the two commands "forever", like 500 times. Make the function `poly` that takes two arguments `side` and `angle`. 

This function will make the turtle create some cool paths. Experiment with different angles: small, large, prime numbers etc. 

❓ When is a path complete so that the turtle walks the same pattern multiple times? When does a path not repeat itself within the limit of 500 repetitions? 

### 💡Poly closing theorem

> The path the turtle walks when given the commands produced by `poly` will be closed when the total turning is a multiple of 360

The total turning number is how much the turtle turns during a path, adding the degrees to the turning number when moving right, and subtracting the degrees when moving left. 

One of the proofs of the theorem sketched in the book is based on the property that all vertices of a path drawn by `poly` lies on a circle. This property can be proved by using that for three points, not on a line, there is a unique circle passing through them. Then one can use congruent triangles formed by the vertices to show that all the vertices has the same distance to the origin, and thus lie on the circle. With this established, we know that the turtle will walk along chords of equal lengths in this circle. But for any heading of the turtle, there is exactly one possible chord of the required length, so when the turtle reach its initial heading, which occurs when the total turning is a multiple of 360, it must also be at the initial position, and about to trace that first chord again.      

### ✍️ Polystop

Make an improved version of `poly`, the book calls it `polystop`, that uses the poly closing theorem, and stops repeating when the total turning is a multiple of 360. This function will need a recursive helper function, which keeps track of the total turning and the accumulated turtle commands, and returns the commands when the total turning is a multiple of 360. You might want to use the operator `%` that returns the remainder of dividing the first integer argument by the second.

Test how this new function works, but keep in mind that the web page might stop to work if the paths get too long.

## 🪾Growth and recursion

