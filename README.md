# 🐢 Fun with F# and turtle geometry

This workshop is based on content from the first two chapters of [Turtle Geometry: The Computer as a Medium for Exploring Mathematics](https://direct.mit.edu/books/oa-monograph/4663/Turtle-GeometryThe-Computer-as-a-Medium-for) by Harold Abelson and Andrea diSessa.

<p>
<img src="./docs/imgs/poly-501.png" alt="Polygon" title="Polygon" height="200"/>
<img src="./docs/imgs/snowflake.png" alt="Snowflake" title="Snowflake" height="200"/>
<img src="./docs/imgs/hilbert-5.png" alt="Hilbert curve" title="Hilbert curve" height="200"/>
</p>

## 🚀 Getting started

Make sure you have .NET 10 SDK installed by running `dotnet --version` in the terminal. The .NET 10 SDK can be downloaded from https://dotnet.microsoft.com/en-us/download. 

1. Clone the repository
1. Run `dotnet restore` from the project root the first time
1. Run `dotnet watch run`. This will run the app and automatically rebuild the app when you make changes in the code
1. Navigate to <http://localhost:5000> in your browser
1. If you see a square you are good to go! 

### Editor

If you want good F# language support, and don't already have a preferred setup for working in .NET, I would recommend using [VS Code](https://code.visualstudio.com) with the [Ionide extension](https://github.com/ionide/ionide-vscode-fsharp).

### Alternative ways of running the code

There is setup for devcontainer, which can be used to work with the code on GitHub, using GitHub Codespaces, or locally from an IDE like VS Code. 

## 🐢 What is a turtle?

A turtle is a small animal moving around in a plane. The turtle doesn't move randomly, instead it responds to commands. 

The four simple commands we will be using are `Forward`, `Left`, `Right` and `Back`, and they all take an integer as input. For forward and back, the integer is the distance the turtle should move, for left and right, it is the degrees the turtle should rotate. The forward command will cause the turtle to leave a trace the distance it moved, while back does not. Forward and back change the turtle's position, while left and right change the direction of the turtle.

In our program we will be most interested in the commands, and less interested in how the turtle executes them, the effect of the commands. We will work with lists of `TurtleCommand`, each of which is a complete set of the commands the turtle must be told to create a specific path. To put together the commands, we will create various functions, some will be building blocks for larger functions, while others are recursive.

What we do need to know about the drawing, however, is that the turtle's initial position is at (0,0), heading upwards, and the path is scaled to fit the SVG size.  

## ♯ What is F#?

F# is a friendly, function-first language, running on the .NET platform. The team developing F# say that the F is for fun ([cited on Wikipedia](https://en.wikipedia.org/wiki/F_Sharp_(programming_language)#cite_note-41])), it really is for functional and [System F](https://en.wikipedia.org/wiki/System_F).

If you are new to F# you might want to take a look at [A very short intro to F#](/docs/fsharp-intro.md) to get a basic overview of the language and the syntax, and revisit it during the workshop. 

### F# interactive

F# interactive lets you play with the F# code directly in the console. You can start an interactive session with the command `dotnet fsi`. Then you can type F# expressions and execute them by typing `;;`.

If you are using VS Code you can select code in a `.fs` file and send the selection to F# interactive with the shortcut `Alt` + `Enter`.

The F# interactive can be useful when working with functions, to test that they behave as they should, without having to wait for code to rebuild.

## 🏗️ The workshop

The workshop consists of the following parts:
* [🟥 Part 1: Polygons](#-part-1-polygons) - Get familiar with lists and functions 
* [🌳 Part 2: Trees](#-part-2-trees)
* [❄️ Part 3: Snowflakes](#️-part-3-snowflakes)
* [🚀 Part 4: Space-filling curves](#-part-4-space-filling-curves)

If you have got the project running by following the steps in the [Getting started](#-getting-started) section, you are ready to start on [Part 1](#-part-1-polygons). You can read and follow the instructions at your own pace. We might also look at some topics together in the group. Don't hesitate to ask questions or share thoughts and ideas!

Some of the emojis are used to mark different types of content:
* ✍️ = A programming task to do, other tasks might depend on them
* 🎨 = Suggestions for further explorations, explore as much or little as you like
* 🧮 = A more theoretical exercise, skip if it doesn't interest you

Spend time on the parts you find entertaining, whether it is to play with turtle designs or fiddle with F#. Follow your own ideas, and don't let the instructions limit you. The most important thing is to have fun! 🎉

## 🟥 Part 1: Polygons

### ✍️ Triangle

Our webpage is showing a square, so the natural first step is to make an [equilateral triangle](https://en.wikipedia.org/wiki/Equilateral_triangle). Look at the file `Core.fs`, and the value `square`, which contains a list of turtle commands. Take inspiration from `square`, and make a new value `triangle` containing a new list of commands, consisting of a combination of `Forward` and `Right`. Update `webPagePath` to point to `triangle`, and check how it looks in the browser.

### 🎨 Experiment

Experiment with the square, the triangle and the basic turtle commands. Combine them into something fun!

### ✍️ Repeat

Both `square` and `triangle` seem to repeat two commands, four and three times respectively. It might be nice to have a function `repeat` to do the repetitions for us, so let's make it. 

The function should take two arguments, an integer `count` for the number of repetitions,  and a list `commands`, the commands we want to repeat. The first part of the function declaration should look like `let repeat count commands =`, then followed by what should be the function body.

The function body can be implemented in (at least) two ways. The easiest way is probably to use the functions `List.replicate` and `List.collect`, available from the [List module](https://fsharp.github.io/fsharp-core-docs/reference/fsharp-collections-listmodule.html). Replicate will make a list with the input repeated n times. The result of this will be of type list of list, since our initial input is a list of turtle commands. Collect will flatten the list, and make the elements of the inner list as separate elements of the result list. Collect needs a mapping function as argument, in our case it should be the identity function `id`. The pipe operator `|>` can be use to chain the function calls.

Another way to implement `repeat` is by making a recursive helper function, that keeps track of the remaining repetitions and the accumulated result. The function should test if the counter is zero, then it should return the result, otherwise it should join the result with the input list, and then call itself with the counter decreased, and the accumulated result as the joined list. For this recursive function you will need to know that it must be declared with `let rec <function name>`, in order to be able to call itself, and that two lists can be joined with the `@` operator.

When repeat is finished, test that it works by rewriting square and triangle, and check that it looks as it should.

### ✍️ Poly

Now that we have `repeat` we can generalise `square` and `triangle`, and make a function `poly` that makes the turtle first move forward a given side distance, and then move right a given angle. This pattern repeats until the turtle closes the path it is drawing. We don't know yet when that happens, so for now we will just repeat the the two commands "forever", like 500 times. Make the function `poly` that takes two arguments `side` and `angle`. 

This function will make the turtle create some cool paths. Experiment with different angles: small, large, prime numbers etc. 

❓ When is a path complete so that the turtle walks the same pattern multiple times? When does a path not repeat itself within the limit of 500 repetitions? 

### 💡 Poly closing theorem

> The path the turtle walks when given the commands produced by `poly` will be closed when the total turning is a multiple of 360

The total turning number is how much the turtle turns during a path, adding the degrees to the turning number when moving right, and subtracting the degrees when moving left. 

One of the proofs of the theorem sketched in the book is based on the property that all vertices of a path drawn by `poly` lie on a circle. This property can be proved by using that for three points, not on a line, there is a unique circle passing through them. Then one can use congruent triangles formed by the vertices to show that all the vertices has the same distance to the origin, and thus lie on the circle. With this established, we know that the turtle will walk along chords of equal lengths in this circle. But for any heading of the turtle, there is exactly one possible chord of the required length, so when the turtle reach its initial heading, which occurs when the total turning is a multiple of 360, it must also be at the initial position, and about to trace that first chord again.      

### ✍️ Polystop

Make an improved version of `poly`, the book calls it `polystop`, that uses the poly closing theorem, and stops repeating when the total turning is a multiple of 360. This function will need a recursive helper function, which keeps track of the total turning and the accumulated turtle commands, and returns the commands when the total turning is a multiple of 360. You might want to use the operator `%` that returns the remainder of dividing the first integer argument by the second.

Test how this new function works for various angles.

## 🌳 Part 2: Trees

Now that we have got the hang of recursion, let's do even more! 
It turns out that the turtle is very good at fractal patterns. Fractals are recursive patterns that have are self-similar. If you zoom in and look at a part of the fractal, it will have the same shape as the whole.

We will start with a regular binary tree. This tree consists of branches, where each branch has two child branches of half the length, at 45 degrees to the left and right from the top. 

<img src="./docs/imgs/branch-3.png" alt="branches level=3" title="branches level=3" width="200"/>

### ✍️ Branch

Make the recursive function `branch` that creates the list of turtle commands for making a tree. The function should have `length` and `level` as arguments. 
If the level equals zero, the function should return an empty list, otherwise it should return a list consisting of moving forward the given `length`, then move 45 degrees left, call branch with `length/2` and `level - 1`, then move right 90 degrees (which will be 45 degrees right of the parent branch), and call branch with `length/2` and `level - 1`.  

Think about how this function should work. If the level equals 1 the turtle should just draw a vertical line, if the level is 2, the path is the vertical line, with two branches on the top, of half the length, in 45 degrees left and right from main branch. The tricky part is that the state of the turtle has to be restored so that the turtle returns back to where it was before each call to branch. The book calls this property *state-transparency*. To get back to where the turtle started, we have to rotate it a bit left, and use `Back` to send it to where it started.

See how the trees produced by `branch` look for different values of level. 

### 🎨 Experiment

Since the turtle will have returned to the start of the tree when it is finished, it is fun to combine multiple trees in the same path. Experiment with making a chain of trees by rotating some degrees in the same direction between each tree, or make them into an avenue. 

If you want to make more realistic looking trees you can experiment with the angle between the sibling branches, and the length of each branch. The length might for instance vary depending on whether it is a left branch or a right branch. You can for instance create a sum type to distinguish between the left and the right branch, and use it as an argument to the branch function.

### 🧮 Math exercise

What is the total length of the lines drawn by the turtle with the original branch function?

## ❄️ Part 3: snowflakes

Another nice fractal pattern is the snowflake. The ground shape is an equilateral triangle, but instead of straight lines as sides, the sides can recursively be split in three segments of equal length. The first and last segments are kept, but the middle segment is replaced by two segments of same length, such that they would form an equilateral triangle with the segment we are replacing.

<img src="./docs/imgs/snowflake-construction.png" alt="Snowflake construction" title="Snowflake construction"/>

This pattern can of course be repeated recursively, and that is how the snowflake is made. 

### ✍️ Snowflake

Start by making the function `snowflake` with the parameters `size` and `level`, for the length of the sides and the level of recursion, and the function body can initially be similar to the `triangle` list, after we rewrote it to use the [repeat](#%EF%B8%8F-repeat) function. 

Then the `Forward` instruction has to be replaced by a function that recursively makes the pattern in the illustration. We will call this function `side`, it will be recursive, and has the same parameters as `snowflake`.  If the level is zero `side` should return `Forward size`, otherwise it should make the pattern, by calling itself with `size / 3` and `level - 1` where the sides are in the illustration. Notice that if `snowflake` is called with `level` zero, and same `size` as in `triangle`, it will be identical to `triangle`.

The function `side` behaves differently from the branch function that was state transparent, and did nothing at level zero. `side` does not change the heading, but it changes the position.

Test the snowflake function with different sizes and levels. Is it necessary to divide the size by three?

### 🎨 Experiment

Make a similar pattern with a square as the basis. 


### 🧮 Math exercise

What is the length of a snowflake curve of level `n`, and what area does it enclose? What happens as `n` approaches infinity?

## 🚀 Part 4: Space-filling curves

The last theme for today is space-filling curves. As the name suggests, these curves pass through every point of a two dimensional region, like the unit square. We will look at the Hilbert curve, named after the German mathematician David Hilbert, who published a [paper (in German)](https://webhomes.maths.ed.ac.uk/~v1ranick/papers/hilbert.pdf) about the curve in 1891. The space-filling Hilbert curve is the limit of piecewise linear curves. We will work with these approximation curves, and also refer to them as Hilbert curves.

The Hilbert curve is constructed from recursively dividing a square into four subsquares. At each step, the squares are numbered such that a square numbered `n` has a common side with the square numbered `n - 1`, and when a square is divided into four new squares in the next iteration, these four squares will be consecutively numbered. The Hilbert curve will at each level pass through the numbered squares in the given order.

<p>
<img src="./docs/imgs/hilbert-0.png" alt="Initial Hilbert curve" title="Initial Hilbert curve" height="200"/>
<img src="./docs/imgs/hilbert-1.png" alt="Hilbert curve level 2" title="Hilbert curve level 2" height="200"/>
</p>


### ✍️ lHilbert and rHilbert

To make a Hilbert curve we will need two functions, one that gives the commands for traversing from right towards left (`lHilbert`), and one that goes from left to right (`rHilbert`). The functions take `size` and `level` as arguments, and the figures below illustrate how `lHilbert` and `rHilbert` are constructed recursively, by joining curves of one level lower.

<p>
<img src="./docs/imgs/lhilbert.png" alt="lHilbert" title="lHilbert" height="200"/>
<img src="./docs/imgs/rhilbert.png" alt="rHilbert" title="rHilbert" height="200"/>
</p>

It seems like we have run into a F# problem then. The two functions depend on each other, while in F# you cannot reference anything that isn't defined before you want to use it. But there is a solution, the keyword `and` is there for mutually recursive bindings. So use `and` instead of `let rec` for the second function.

Implement the functions and test them out for different levels.

### 🧮 Math exercise

What is the length of the Hilbert curve of level `n` with size equal one? Start by finding the length of the Hilbert curve at level `n` as a function of the length of the curve at level `n - 1`.

### 💡 Application of the Hilbert curve

The mapping from the line to the two dimensional space given by the Hilbert curve, preserves the locality quite well, which means that points that are close to each other on the line will be close in two dimensional space. This has been used to visualise the utilisation of the IPv4 address space. See [Mapping the whole internet with Hilbert curves](https://blog.benjojo.co.uk/post/scan-ping-the-internet-hilbert-curve).

<img src="https://imgs.xkcd.com/comics/map_of_the_internet.jpg" alt="xkcd comic: map of the internet" title="xkcd map of the internet" height="300">

## 🏁 The end

Congratulations, you have reached the end of the workshop! 🎉 I hope you have enjoyed this little journey with F# and geometry.

If you want to explore more F# or geometry, take a look at the [resource page](./docs/resources.md), where I have collected various resources, maybe they can be useful next steps in your journey.

There is also a branch `solutions` with suggestions on how the programming exercises in the workshop can be solved.
