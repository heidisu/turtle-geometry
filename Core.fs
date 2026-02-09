module TurtleGeometry.Core

type TurtleCommmand = 
    | Forward of int
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

// the turtle commands used to create svg on web page
let webPagePath = square