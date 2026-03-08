module TurtleGeometry.WebPage

open Giraffe.ViewEngine
open TurtleGeometry.Core
open System
open System.Text

let color = "#5d009b"

let svg width height path (xMin, xMax, yMin, yMax) =
    let strokeWidth = 1.0

    $"""
    <svg xmlns="http://www.w3.org/2000/svg" width="%i{width}" height="%i{height}" viewBox="%.4f{xMin - strokeWidth},%.4f{yMin - strokeWidth},%.4f{xMax - xMin + 2.0 * strokeWidth},%.4f{yMax - yMin + 2.0 * strokeWidth}">
        <path stroke="%s{color}" stroke-width="%.1f{strokeWidth}" fill="white" vector-effect="non-scaling-stroke" d="%s{path}">
        </path>
    </svg>
"""

let rec calculatePath (x, y) (dx, dy) (pathBuilder: StringBuilder) xMin xMax yMin yMax turtlePath =
    match turtlePath with
    | [] -> pathBuilder, (xMin, xMax, yMin, yMax)
    | command :: xs ->
        match command with
        | Forward a ->
            let afloat = float a
            let factor = sqrt (afloat * afloat / (dx * dx + dy * dy))
            let newX, newY = (x + factor * dx, y + factor * dy)
            pathBuilder.Append($" L%.4f{newX},%.4f{newY}") |> ignore

            calculatePath
                (newX, newY)
                (dx, dy)
                pathBuilder
                (min xMin newX)
                (max xMax newX)
                (min yMin newY)
                (max yMax newY)
                xs
        | Right a ->
            let phi = atan2 dy dx
            let apol = float a / 360.0 * 2.0 * Math.PI
            let newPhi = phi + apol
            calculatePath (x, y) (cos newPhi, sin newPhi) pathBuilder xMin xMax yMin yMax xs
        | Left a ->
            let phi = atan2 dy dx
            let apol = float a / 360.0 * 2.0 * Math.PI
            let newPhi = phi - apol
            calculatePath (x, y) (cos newPhi, sin newPhi) pathBuilder xMin xMax yMin yMax xs
        | Back a ->
            let afloat = float a
            let factor = sqrt (afloat * afloat / (dx * dx + dy * dy))
            let newX, newY = (x - factor * dx, y - factor * dy)
            pathBuilder.Append($" M%.4f{newX},%.4f{newY}") |> ignore

            calculatePath
                (newX, newY)
                (dx, dy)
                pathBuilder
                (min xMin newX)
                (max xMax newX)
                (min yMin newY)
                (max yMax newY)
                xs

let turtleToSvgPath turtlePath =
    let stringBuilder = new StringBuilder()

    let pathBuilder, viewBox =
        calculatePath (0.0, 0.0) (0.0, -1.0) (stringBuilder.Append("M0,0")) 0.0 0.0 0.0 0.0 turtlePath

    pathBuilder.ToString(), viewBox


let htmlPage turtlePath =
    let path, (xMin, xMax, yMin, yMax) = turtleToSvgPath turtlePath

    html [] [
        head [] [ title [] [ str "Turtle Geometry" ] ]
        body [ attr "style" $"color: {color}" ] [
            div [ attr "align" "center" ] [
                h1 [] [ str "Turtle Geometry" ]
                div [] [ rawText (svg 450 450 path (xMin, xMax, yMin, yMax)) ]
                div [] [
                    div [] [ str $"x range: [%.2f{xMin}, %.2f{xMax}]" ]
                    div [] [ str $"y range: [%.2f{yMin}, %.2f{yMax}]" ]
                ]
            ]
        ]
    ]
