module TurtleGeometry.WebPage

open Giraffe.ViewEngine
open TurtleGeometry.Core
open System


let color = "#5d009b"

let svg width height path (xMin, xMax, yMin, yMax) =
    let strokeWidth = 1

    $"""
    <svg xmlns="http://www.w3.org/2000/svg" width="{width}" height="{height}" viewBox="{xMin - strokeWidth},{yMin - strokeWidth},{xMax - xMin + 2 * strokeWidth},{yMax - yMin + 2 * strokeWidth}">
        <path stroke="{color}" stroke-width="{strokeWidth}" fill="white" vector-effect="non-scaling-stroke" d="{path}">
        </path>
    </svg>
"""

let rec calculatePath (x, y) (dx, dy) svgPath xMin xMax yMin yMax turtlePath =
    match turtlePath with
    | [] -> svgPath, (xMin, xMax, yMin, yMax)
    | command :: xs ->
        match command with
        | Forward a ->
            let afloat = float a
            let factor = sqrt (afloat * afloat / (dx * dx + dy * dy))
            let newX, newY = (x + factor * dx, y + factor * dy)
            let newPath = svgPath + $" L{newX},{newY}"
            calculatePath (newX, newY) (dx, dy) newPath (min xMin newX) (max xMax newX) (min yMin newY) (max yMax newY) xs
        | Right a ->
            let phi = atan2 dy dx
            let apol = float a / 360.0 * 2.0 * Math.PI
            let newPhi = phi + apol
            calculatePath (x, y) (cos newPhi, sin newPhi) svgPath xMin xMax yMin yMax xs
        | Left a ->
            let phi = atan2 dy dx
            let apol = float a / 360.0 * 2.0 * Math.PI
            let newPhi = phi - apol
            calculatePath (x, y) (cos newPhi, sin newPhi) svgPath xMin xMax yMin yMax xs
        | Back a ->
            let afloat = float a
            let factor = sqrt (afloat * afloat / (dx * dx + dy * dy))
            let newX, newY = (x - factor * dx, y - factor * dy)
            let newPath = svgPath + $" M{newX},{newY}"
            calculatePath (newX, newY) (dx, dy) newPath (min xMin newX) (max xMax newX) (min yMin newY) (max yMax newY) xs

let turtleToSvgPath turtlePath =
    calculatePath (0.0, 0.0) (0.0, -1.0) "M0,0" 0.0 0.0 0.0 0.0 turtlePath


let htmlPage turtlePath =
    let path, (xMin, xMax, yMin, yMax) = turtleToSvgPath turtlePath

    html [] [
        head [] [ title [] [ str "Turtle Geometry" ] ]
        body [attr "style" $"color: {color}" ] [
            div [ attr "align" "center" ] [
                h1 [] [ str "Turtle Geometry" ]
                div [] [ rawText (svg 450 450 path (int xMin, int xMax, int yMin, int yMax)) ]
                div [] [
                    div [] [ str $"x range: [{int xMin}, {int xMax}]" ]
                    div [] [ str $"y range: [{int yMin}, {int yMax}]" ]
                ]
            ]
        ]
    ]
