module TurtleGeometry.WebPage

open Giraffe.ViewEngine
open TurtleGeometry.Domain
open System

let xMax = 10.0
let yMax = 10.0
let xMin = -20.0
let yMin = -10.0

let mino = min xMin yMin
let maxo = max xMax yMax
let size = maxo - mino


let svg width height path (xMin, xMax, yMin, yMax) = 
    let size = max (xMax - xMin) (yMax - yMin)
    let strokeWidth = 2
    $"""
    <svg xmlns="http://www.w3.org/2000/svg" width="{width}" height="{height}" viewBox="{xMin - strokeWidth},{yMin - strokeWidth},{size + 2 * strokeWidth},{size + 2 * strokeWidth}">
        <path stroke="black" stroke-width="{strokeWidth}" fill="white" vector-effect="non-scaling-stroke" d="{path}">
        </path>
    </svg>
"""

let rec calculatePath (x, y) (dx, dy) svgPath xVals yVals turtlePath=
    match turtlePath with
    | [] -> svgPath, xVals, yVals
    | command :: xs ->
        match command with
        | Forward a ->
            let afloat = float a
            let factor = sqrt (afloat * afloat /(dx * dx + dy * dy))
            let newX, newY = (x + factor * dx, y + factor * dy)
            let newPath = svgPath + $" L{int <| Math.Round newX},{int <| Math.Round newY}"
            printfn $"{a} ({x}, {y}) ({newX},{newY})"
            calculatePath (newX, newY) (dx, dy) newPath (newX :: xVals) (newY :: yVals) xs
        | Right a -> 
            let phi = atan2 dy dx
            let apol = float a / 360.0 * 2.0 * Math.PI
            let newPhi = phi + apol
            calculatePath (x, y) (cos newPhi, sin newPhi) svgPath xVals yVals xs
        | Left a -> 
            let phi = atan2 dy dx
            let apol = float a / 360.0 * 2.0 * Math.PI
            let newPhi = phi - apol
            calculatePath (x, y) (cos newPhi, sin newPhi) svgPath xVals yVals xs

let turtleToSvgPath turtlePath =
    let path, xVals, yVals = calculatePath (0.0, 0.0) (0.0, -1.0) "M0,0" [] [] turtlePath
    path, (int <| List.min xVals, int <| List.max xVals, int <| List.min yVals, int <| List.max yVals)

let htmlPage turtlePath = 
    let path, viewBox = turtleToSvgPath turtlePath
    printfn $"{path} {viewBox}"
    html [] [
        head [] [
            title [] [ str "Turtle Geometry" ]
        ]
        body [] [
            h1 [] [ str "Turtle Geometry" ]
            div [] [rawText (svg 500 500 path viewBox)]
        ]
    ]