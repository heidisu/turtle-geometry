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

// Part 2: Trees

let rec branch length level =
    if level = 0 then
        []
    else
        [ Forward length; Left 45 ]
        @ branch (length / 2) (level - 1)
        @ [ Right 90 ]
        @ branch (length / 2) (level - 1)
        @ [ Left 45; Back length ]

let tree = branch 1000 10

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
let webPagePath = snowflake 10000 7
