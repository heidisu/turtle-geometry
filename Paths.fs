module TurtleGeometry.Paths

open TurtleGeometry.Domain

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

let triangle = [
    Forward 20
    Right 120
    Forward 20
    Right 120
    Forward 20
]

let repeat n commands = 
    commands 
    |> List.replicate n
    |> List.collect id

let repeat' n commands = 
    let rec accumulate n result = 
        if n < 1 then  result
        else accumulate (n - 1) (result @ commands)
    accumulate n []

let poly deg dist = 
    repeat 100 [Forward dist; Right deg]


printfn "HALLO: %A" (repeat' 2 [Forward 10])
// the turtle commands used to create svg on web page
let webPagePath = square @ triangle @ square @ triangle @ square @ triangle @ square