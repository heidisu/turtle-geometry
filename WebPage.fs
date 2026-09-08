module TurtleGeometry.WebPage

open Giraffe.ViewEngine
open TurtleGeometry.Core
open System
open System.Text

let color = "#5d009b"
let arrowColor = "#D97706"
let strokeWidth = 1.0

type Range = { Min: float; Max: float }
type ViewBox = { XRange: Range; YRange: Range }
type Point = { X: float; Y: float }
type Direction = { Dx: float; Dy: float }

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

let getNewPosition length pos dir factor =
    let scale = sqrt (length * length / (dir.Dx * dir.Dx + dir.Dy * dir.Dy))

    {
        X = pos.X + factor * scale * dir.Dx
        Y = pos.Y + factor * scale * dir.Dy
    }

let getNewDirection angle dir factor =
    let phi = atan2 dir.Dy dir.Dx
    let polar = angle / 360.0 * 2.0 * Math.PI
    let newPhi = phi + factor * polar
    { Dx = cos newPhi; Dy = sin newPhi }

let rec calculatePath (pos: Point) (dir: Direction) (pathBuilder: StringBuilder) (viewBox: ViewBox) turtlePath =
    match turtlePath with
    | [] -> pos, dir, pathBuilder, viewBox
    | command :: xs ->
        match command with
        | Forward a ->
            let newPosition = getNewPosition (float a) pos dir 1.0
            pathBuilder.Append($" L%.4f{newPosition.X},%.4f{newPosition.Y}") |> ignore
            calculatePath newPosition dir pathBuilder (updateViewBox newPosition viewBox) xs
        | Right a ->
            let newDirection = getNewDirection (float a) dir 1.0
            calculatePath pos newDirection pathBuilder viewBox xs
        | Left a ->
            let newDirection = getNewDirection (float a) dir -1.0
            calculatePath pos newDirection pathBuilder viewBox xs
        | Back a ->
            let newPosition = getNewPosition (float a) pos dir -1.0
            pathBuilder.Append($" M%.4f{newPosition.X},%.4f{newPosition.Y}") |> ignore
            calculatePath newPosition dir pathBuilder (updateViewBox newPosition viewBox) xs

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

let getArrowPath pos dir scaleFactor =
    let arrowSize = 20.0 * scaleFactor
    let arrowTip = getNewPosition arrowSize pos dir 1.0
    let leftTip = getNewPosition arrowSize pos (getNewDirection 150.0 dir -1.0) 1.0
    let rightTip = getNewPosition arrowSize pos (getNewDirection 150.0 dir 1.0) 1.0

    let arrow =
        $"""<path 
        d="M{pos.X},{pos.Y} L%.4f{rightTip.X},%.4f{rightTip.Y} L%.4f{arrowTip.X},%.4f{arrowTip.Y} L%.4f{leftTip.X},%.4f{leftTip.Y}" 
        fill="{arrowColor}" 
        stroke="{arrowColor}" 
        stroke-width="%.1f{strokeWidth}"  
        vector-effect="non-scaling-stroke" />"""

    arrow

let getSvg width height pos dir path viewBox showDirection =
    let turtleWidth = viewBox.XRange.Max - viewBox.XRange.Min
    let turtleHeight = viewBox.YRange.Max - viewBox.YRange.Min
    let floatWidth = float width
    let floatHeight = float height

    let scaleFactor =
        max
            (max (turtleHeight / floatHeight) (turtleWidth / floatWidth))
            (2.0 * strokeWidth / max floatWidth floatHeight)

    let arrowPath =
        if showDirection then
            getArrowPath pos dir scaleFactor
        else
            ""

    $"""
    <svg xmlns="http://www.w3.org/2000/svg" 
        overflow="visible" 
        box-shadow="0px -0px 100px transparent" ¨
        style="background-color:white" 
        width="%i{width}" 
        height="%i{height}" 
        viewBox="%.4f{viewBox.XRange.Min - strokeWidth},%.4f{viewBox.YRange.Min - strokeWidth},%.4f{turtleWidth + 2.0 * strokeWidth},%.4f{turtleHeight + 2.0 * strokeWidth}">
        {arrowPath}
        <path stroke="%s{color}" stroke-width="%.1f{strokeWidth}" fill="white" fill-opacity=0.0 vector-effect="non-scaling-stroke" d="%s{path}"/>
    </svg>
"""

let htmlPage turtlePath showDirection =
    let pos, dir, path, viewBox = turtleToSvgPath turtlePath

    html [] [
        head [] [ title [] [ str "Turtle Geometry" ] ]
        body [ attr "style" $"color: {color}" ] [
            div [ attr "align" "center" ] [
                h1 [] [ str "Turtle Geometry" ]
                div [] [ rawText (getSvg 450 450 pos dir path viewBox showDirection) ]
                div [] [
                    div [] [ str $"x range: [%.2f{viewBox.XRange.Min}, %.2f{viewBox.XRange.Max}]" ]
                    div [] [ str $"y range: [%.2f{viewBox.YRange.Min}, %.2f{viewBox.YRange.Max}]" ]
                ]
            ]
        ]
    ]
