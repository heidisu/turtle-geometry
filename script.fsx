(*
    This script can be used instead of running the web api
    Watch changes with the command `dotnet watch build`
    Run the script with the command `dotnet fsi script.fsx
    The file `image.svg` will contain the turtle path
*)

#r "bin/Debug/net10.0/TurtleGeometry.dll"

open TurtleGeometry.Core
open TurtleGeometry.WebPage
open System.IO

let svgPath, (xMin, xMax, yMin, yMax) = turtleToSvgPath webPagePath
let svg = svg 450 450 svgPath (xMin, xMax, yMin, yMax)

File.WriteAllText("image.svg", svg)
