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

let rec branch length level =
    if level = 0 then []
    else
        [ Forward length; Left 45]
        @ branch (length / 2) (level - 1)
        @ [ Right 90]
        @ branch (length / 2) (level - 1)
        @ [Left 45; Back length]



// the turtle commands used to create svg on web page
let webPagePath = branch 100 3
