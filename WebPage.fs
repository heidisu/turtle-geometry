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

let svg width height pos dir path viewBox =
    let strokeWidth = 1.0
    let dirLength = sqrt (dir.Dx * dir.Dx + dir.Dy * dir.Dy)

    let normalized = {
        Dx = dir.Dx / dirLength
        Dy = dir.Dy / dirLength
    }

    let turtleWidth = viewBox.XRange.Max - viewBox.XRange.Min
    let turtleHeight = viewBox.YRange.Max - viewBox.YRange.Min
    let arrowSize = turtleHeight * 20.0 / (float) height

    let arrow =
        $"""<path d="M {pos.X} {pos.Y} l {arrowSize * normalized.Dx} {arrowSize * normalized.Dy}" stroke="red" stroke-width="%.1f{strokeWidth}"  vector-effect="non-scaling-stroke" />"""


    $"""
    <svg xmlns="http://www.w3.org/2000/svg" overflow="visible" box-shadow="0px -0px 100px transparent" style="background-color:white" width="%i{width}" height="%i{height}" viewBox="%.4f{viewBox.XRange.Min - strokeWidth},%.4f{viewBox.YRange.Min - strokeWidth},%.4f{turtleWidth + 2.0 * strokeWidth},%.4f{turtleHeight + 2.0 * strokeWidth}">
        <path stroke="%s{color}" stroke-width="%.1f{strokeWidth}" fill="white" vector-effect="non-scaling-stroke" d="%s{path}"/>
        {arrow}
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

let rec calculatePath (pos: Point) (dir: Direction) (pathBuilder: StringBuilder) (viewBox: ViewBox) turtlePath =
    match turtlePath with
    | [] -> pos, dir, pathBuilder, viewBox
    | command :: xs ->
        match command with
        | Forward a ->
            let afloat = float a
            let factor = sqrt (afloat * afloat / (dir.Dx * dir.Dx + dir.Dy * dir.Dy))

            let newPoint = {
                X = pos.X + factor * dir.Dx
                Y = pos.Y + factor * dir.Dy
            }

            pathBuilder.Append($" L%.4f{newPoint.X},%.4f{newPoint.Y}") |> ignore
            calculatePath newPoint dir pathBuilder (updateViewBox newPoint viewBox) xs
        | Right a ->
            let phi = atan2 dir.Dy dir.Dx
            let apol = float a / 360.0 * 2.0 * Math.PI
            let newPhi = phi + apol
            calculatePath pos { Dx = cos newPhi; Dy = sin newPhi } pathBuilder viewBox xs
        | Left a ->
            let phi = atan2 dir.Dy dir.Dx
            let apol = float a / 360.0 * 2.0 * Math.PI
            let newPhi = phi - apol
            calculatePath pos { Dx = cos newPhi; Dy = sin newPhi } pathBuilder viewBox xs
        | Back a ->
            let afloat = float a
            let factor = sqrt (afloat * afloat / (dir.Dx * dir.Dx + dir.Dy * dir.Dy))

            let newPoint = {
                X = pos.X - factor * dir.Dx
                Y = pos.Y - factor * dir.Dy
            }

            pathBuilder.Append($" M%.4f{newPoint.X},%.4f{newPoint.Y}") |> ignore
            calculatePath newPoint dir pathBuilder (updateViewBox newPoint viewBox) xs

let turtleToSvgPath turtlePath =
    let stringBuilder = new StringBuilder()

    let pos, dir, pathBuilder, viewBox =
        calculatePath
            { X = 0.0; Y = 0.0 }
            { Dx = 0.0; Dy = -1.0 }
            (stringBuilder.Append("M0,0"))
            {
                XRange = { Min = 0.0; Max = 0.0 }
                YRange = { Min = 0.0; Max = 0.0 }
            }
            turtlePath

    pos, dir, pathBuilder.ToString(), viewBox


let htmlPage turtlePath showArrow =
    let pos, dir, path, viewBox = turtleToSvgPath turtlePath

    html [] [
        head [] [ title [] [ str "Turtle Geometry" ] ]
        body [ attr "style" $"color: {color}" ] [
            div [ attr "align" "center" ] [
                h1 [] [ str "Turtle Geometry" ]
                div [] [ rawText (svg 450 450 pos dir path viewBox) ]
                div [] [
                    div [] [ str $"x range: [%.2f{viewBox.XRange.Min}, %.2f{viewBox.XRange.Max}]" ]
                    div [] [ str $"y range: [%.2f{viewBox.YRange.Min}, %.2f{viewBox.YRange.Max}]" ]
                ]
            ]
        ]
    ]
