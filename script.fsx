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

let pos, dir, path, viewBox = turtleToSvgPath webPagePath
let svg = getSvg 450 450 pos dir path viewBox false

File.WriteAllText("image.svg", svg)
