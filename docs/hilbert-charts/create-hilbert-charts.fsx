#r "../../bin/Debug/net10.0/TurtleGeometry.dll"

open TurtleGeometry.Core
open System.IO
open System.Text

let toSvg padding pixels =
    let maxX = pixels |> Set.map fst |> Set.maxElement
    let maxY = pixels |> Set.map snd |> Set.maxElement
    let cols = maxX + 2 * padding + 1
    let rows = maxY + 2 * padding + 1
    printfn "Rows and cols: %A %A" rows cols
    let cellSize = 10
    let strokeWidth = 1.0
    let imageWidth = float (cellSize * cols) + strokeWidth / 2.0
    let imageHeight = float (cellSize * rows) + strokeWidth / 2.0

    let toRect (x, y) =
        $"""<rect x="{cellSize * (x + padding)}" y="{cellSize * (y + padding)}" width="{cellSize}" height="{cellSize}" />
        """

    let stringBuilder = new StringBuilder()

    let rectangles =
        pixels
        |> Set.fold (fun s p -> stringBuilder.Append(toRect p)) stringBuilder
        |> fun s -> s.ToString()

    $"""<svg width="{imageWidth}" height="{imageHeight}" xmlns="http://www.w3.org/2000/svg" style="background-color:white">
  <defs>
    <pattern id="grid" width="10" height="10" patternUnits="userSpaceOnUse">
      <path d="M {cellSize} 0 L 0 0 0 {cellSize}" fill="none" stroke="gray" stroke-width="{strokeWidth}"/>
    </pattern>
  </defs>
  {rectangles}
  <rect width="{imageWidth}" height="{imageHeight}" fill="url(#grid)" />
</svg>
    """

let normalize points =
    let xMin = points |> Set.map fst |> Set.minElement
    let yMin = points |> Set.map snd |> Set.minElement
    let xDiff = 0 - xMin
    let yDiff = 0 - yMin
    points |> Set.map (fun (x, y) -> (x + xDiff, y + yDiff))

let rec hilbertToPixels (x, y) (dx, dy) points commands =
    match commands with
    | [] -> points
    | c :: cs ->
        match c with
        | Forward _ ->
            let xVals = if dx <> 0 then [ x; x + dx; x + 2 * dx ] else [ x ]

            let yVals = if dy <> 0 then [ y; y + dy; y + 2 * dy ] else [ y ]

            let newPoints =
                xVals
                |> List.fold
                    (fun s x ->
                        let points = yVals |> List.map (fun y -> (x, y)) |> Set.ofList
                        Set.union s points)
                    points

            hilbertToPixels (x + 2 * dx, y + 2 * dy) (dx, dy) newPoints cs
        | Back _ -> hilbertToPixels (x, y) (dx, dy) points cs
        | Right _ ->
            let (ndx, ndy) =
                match (dx, dy) with
                | (-1, 0) -> (0, -1)
                | (1, 0) -> (0, 1)
                | (0, 1) -> (-1, 0)
                | (0, -1) -> (1, 0)
                | _ -> failwith "Unsuppoerted value"

            hilbertToPixels (x, y) (ndx, ndy) points cs
        | Left _ ->
            let (ndx, ndy) =
                match (dx, dy) with
                | (-1, 0) -> (0, 1)
                | (1, 0) -> (0, -1)
                | (0, 1) -> (1, 0)
                | (0, -1) -> (-1, 0)
                |  _ -> failwith "Unsuppoerted value"

            hilbertToPixels (x, y) (ndx, ndy) points cs


[ 1..5 ]
|> List.iter (fun i ->
    let points = hilbertToPixels (0, 0) (0, -1) Set.empty (lHilbert 10 i)
    let svg = toSvg 1 (normalize points)
    File.WriteAllText($"chart-hilbert-{i}.svg", svg))
