module TurtleGeometry.WebPage

open Giraffe.ViewEngine
open TurtleGeometry.Core
open System
open System.Text

let color = "#5d009b"

type Range = { Min: float; Max: float }
type ViewBox = { XRange: Range; YRange: Range }
type Point = { X: float; Y: float }
type Direction = { Dx: float; Dy: float }

let svg width height path viewBox =
    let strokeWidth = 1.0

    $"""
    <svg xmlns="http://www.w3.org/2000/svg" style="background-color:white" width="%i{width}" height="%i{height}" viewBox="%.4f{viewBox.XRange.Min - strokeWidth},%.4f{viewBox.YRange.Min - strokeWidth},%.4f{viewBox.XRange.Max - viewBox.XRange.Min + 2.0 * strokeWidth},%.4f{viewBox.YRange.Max - viewBox.YRange.Min + 2.0 * strokeWidth}">
        <path stroke="%s{color}" stroke-width="%.1f{strokeWidth}" fill="white" vector-effect="non-scaling-stroke" d="%s{path}">
        </path>
    </svg>
"""

let updateViewBox point viewBox = {
    XRange = {
        Min = min viewBox.XRange.Min point.X
        Max = max viewBox.XRange.Max point.X
    }
    YRange = {
        Min = min viewBox.YRange.Min point.Y
        Max = max viewBox.YRange.Max point.Y
    }
}

let rec calculatePath (point: Point) (dir: Direction) (pathBuilder: StringBuilder) (viewBox: ViewBox) turtlePath =
    match turtlePath with
    | [] -> pathBuilder, viewBox
    | command :: xs ->
        match command with
        | Forward a ->
            let afloat = float a
            let factor = sqrt (afloat * afloat / (dir.Dx * dir.Dx + dir.Dy * dir.Dy))

            let newPoint = {
                X = point.X + factor * dir.Dx
                Y = point.Y + factor * dir.Dy
            }

            pathBuilder.Append($" L%.4f{newPoint.X},%.4f{newPoint.Y}") |> ignore
            calculatePath newPoint dir pathBuilder (updateViewBox newPoint viewBox) xs
        | Right a ->
            let phi = atan2 dir.Dy dir.Dx
            let apol = float a / 360.0 * 2.0 * Math.PI
            let newPhi = phi + apol
            calculatePath point { Dx = cos newPhi; Dy = sin newPhi } pathBuilder viewBox xs
        | Left a ->
            let phi = atan2 dir.Dy dir.Dx
            let apol = float a / 360.0 * 2.0 * Math.PI
            let newPhi = phi - apol
            calculatePath point { Dx = cos newPhi; Dy = sin newPhi } pathBuilder viewBox xs
        | Back a ->
            let afloat = float a
            let factor = sqrt (afloat * afloat / (dir.Dx * dir.Dx + dir.Dy * dir.Dy))

            let newPoint = {
                X = point.X - factor * dir.Dx
                Y = point.Y - factor * dir.Dy
            }

            pathBuilder.Append($" M%.4f{newPoint.X},%.4f{newPoint.Y}") |> ignore
            calculatePath newPoint dir pathBuilder (updateViewBox newPoint viewBox) xs

let turtleToSvgPath turtlePath =
    let stringBuilder = new StringBuilder()

    let pathBuilder, viewBox =
        calculatePath
            { X = 0.0; Y = 0.0 }
            { Dx = 0.0; Dy = -1.0 }
            (stringBuilder.Append("M0,0"))
            {
                XRange = { Min = 0.0; Max = 0.0 }
                YRange = { Min = 0.0; Max = 0.0 }
            }
            turtlePath

    pathBuilder.ToString(), viewBox


let htmlPage turtlePath =
    let path, viewBox = turtleToSvgPath turtlePath

    html [] [
        head [] [ title [] [ str "Turtle Geometry" ] ]
        body [ attr "style" $"color: {color}" ] [
            div [ attr "align" "center" ] [
                h1 [] [ str "Turtle Geometry" ]
                div [] [ rawText (svg 450 450 path viewBox) ]
                div [] [
                    div [] [ str $"x range: [%.2f{viewBox.XRange.Min}, %.2f{viewBox.XRange.Max}]" ]
                    div [] [ str $"y range: [%.2f{viewBox.YRange.Min}, %.2f{viewBox.YRange.Max}]" ]
                ]
            ]
        ]
    ]
