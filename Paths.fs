module TurtleGeometry.Paths

open TurtleGeometry.Domain

let square () = [
    Forward 10
    Right 90
    Forward 20
    Right 90
    Forward 30
    Right 90
    Forward 40
]

let webPagePath = square