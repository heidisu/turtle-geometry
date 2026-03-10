module TurtleGeometry.Core

type TurtleCommand =
    | Forward of int
    | Back of int
    | Right of int
    | Left of int

let square = [
    Forward 20
    Right 90
    Forward 20
    Right 90
    Forward 20
    Right 90
    Forward 20
    Right 90
]

// Part 1: polygons

let triangle = [ Forward 20; Right 120; Forward 20; Right 120; Forward 20; Right 120 ]

let repeat count commands =
    commands |> List.replicate count |> List.collect id

let repeat' count commands =
    let rec accumulate count result =
        if count = 0 then
            result
        else
            accumulate (count - 1) (result @ commands)

    accumulate count []


let square' = repeat 4 [ Forward 20; Right 90 ]
let triangle' = repeat' 3 [ Forward 20; Right 120 ]

let poly side angle =
    repeat 500 [ Forward side; Right angle ]

let polystop side angle =
    let rec loop totalTurning result =
        if totalTurning > 0 && totalTurning % 360 = 0 then
            result
        else
            loop (angle + totalTurning) (result @ [ Forward side; Right angle ])

    loop 0 []

// Part 2: Trees

let rec branch length level =
    if level = 0 then
        []
    else
        let subBranch = branch (length / 2) (level - 1)

        [ Forward length; Left 45 ]
        @ subBranch
        @ [ Right 90 ]
        @ subBranch
        @ [ Left 45; Back length ]

let tree = branch 1000 10

// Tree experiment
type Branch =
    | LeftBranch
    | RightBranch

let rec branch' length angle branchType level =
    if level = 0 then
        []
    else
        let newLength =
            match branchType with
            | LeftBranch -> 2 * length
            | RightBranch -> length

        [ Forward newLength; Left angle ]
        @ branch' length angle LeftBranch (level - 1)
        @ [ Right <| 2 * angle ]
        @ branch' length angle RightBranch (level - 1)
        @ [ Left angle; Back newLength ]

// Part 3: Snowflake

let rec side size level =
    if level = 0 then
        [ Forward size ]
    else
        let subSide = side (size / 3) (level - 1)

        subSide
        @ [ Left 60 ]
        @ subSide
        @ [ Right 120 ]
        @ subSide
        @ [ Left 60 ]
        @ subSide

let snowflake size level =
    repeat 3 (side size level @ [ Right 120 ])

let rec side' size level =
    if level = 0 then
        [ Forward size ]
    else
        let subSide = side' size (level - 1)

        subSide
        @ [ Left 90 ]
        @ subSide
        @ [ Right 90 ]
        @ subSide
        @ [ Right 90 ]
        @ subSide
        @ [ Left 90 ]
        @ subSide

let squareflake size level =
    repeat 4 (side' size level @ [ Right 90 ])

// Part 4: Hilbert curve

let rec lHilbert size level =
    if level = 0 then
        []
    else
        [ Left 90 ]
        @ rHilbert size (level - 1)
        @ [ Forward size; Right 90 ]
        @ lHilbert size (level - 1)
        @ [ Forward size ]
        @ lHilbert size (level - 1)
        @ [ Right 90; Forward size ]
        @ rHilbert size (level - 1)
        @ [ Left 90 ]

and rHilbert size level =
    if level = 0 then
        []
    else
        [ Right 90 ]
        @ lHilbert size (level - 1)
        @ [ Forward size; Left 90 ]
        @ rHilbert size (level - 1)
        @ [ Forward size ]
        @ rHilbert size (level - 1)
        @ [ Left 90; Forward size ]
        @ lHilbert size (level - 1)
        @ [ Right 90 ]


let rec pathLenght lst acc =
    match lst with
    | [] -> acc
    | x :: xs ->
        let dist =
            match x with
            | Forward d -> d
            | _ -> 0

        dist + pathLenght xs acc

// the turtle commands used to create svg on web page
let webPagePath = lHilbert 20 1

let rec toPointSet (x, y) (dx, dy) points commands = 
    match commands with
    | [] -> points
    | c :: cs -> 
        match c with 
        | Forward _ ->
            let xVals = if dx <> 0 then [x .. x + 2] |> List.map ((*) dx) else [x]
            let yVals = if dy <> 0 then [y .. y + 2] |> List.map ((*) dy) else [y]
            let newPoints = 
                xVals 
                |> List.fold (fun s x -> 
                    let points = yVals |> List.map (fun y -> (x, y)) |> Set.ofList
                    Set.union s points
                ) points
            toPointSet (x + 2*dx, y + 2*dy) (dx, dy) newPoints cs
        | Back _ -> toPointSet (x, y) (dx, dy) points cs
        | Right _ -> 
            let (ndx, ndy) = 
                match (dx, dy) with
                | (-1, 0) -> (0, -1)
                | (1, 0) -> (0, 1)
                | (0, 1) -> (-1, 0)
                | (0, -1) -> (1,0)
            toPointSet (x, y) (ndx, ndy) points cs
        | Left _ -> 
            let (ndx, ndy) = 
                match (dx, dy) with
                | (-1, 0) -> (0, 1)
                | (1, 0) -> (0, -1)
                | (0, 1) -> (1, 0)
                | (0, -1) -> (-1,0)
            toPointSet (x, y) (ndx, ndy) points cs

let points = toPointSet (0,0) (0, -1) Set.empty (lHilbert 10 1)

let normalizePoints points = 
    let xMin = points |> Set.map fst |> Set.minElement
    let yMin= points |> Set.map snd |> Set.minElement
    let xDiff = 0 - xMin
    let yDiff = 0 - yMin
    points |> Set.map (fun (x, y) -> (x + xDiff, y + yDiff))

printfn "POINTS: %A" points
printfn "NPoints: %A" (normalizePoints points)
