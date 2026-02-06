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

The four simple commmands we will be using are `Forward`, `Left`, `Right` and `Back`, and they all take an integer as input. For forward and back, the integer is the distance the turtle should move, for left and right, it is the degrees the turtle should rotate. The forward command will cause the turtle to leave a trace the distance it moved, while back does not. Forward and back change the turtles position, while left and rich change the direction of the turtle.

In our program we will be most interested in the commands, and less interested in how the turtle executes them. We will create lists of  `TurtleCommands` which each are a complete set of commands the turtle need in order to create a specific path. 

## 🟥 Polygons

### ✍️ Triangle

Our webpage is showing a square, so the natural first step is to make an [equilateral triangle](https://en.wikipedia.org/wiki/Equilateral_triangle)
Look at the file `Core.fs`, and the variable `square`, which contains a list of turtle commands. Take inspiration from `square`, and make a new variable `triangle` containing a new list of commands, consting of a combination of `Forward` and `Right`. Update `webPagePath` to point to `triangle`, and check how it looks in the browser.

### ✍️ Repeat



### ✍️ Poly

## 🟣 Circles

## More recursion

