module TurtleGeometry.Paths

open TurtleGeometry.Domain

let square () = [
    Forward 20
    Right 90
    Forward 20
    Right 90
    Forward 20
    Right 90
    Forward 20
]

let triangle = [
    Forward 20
    Right 120
    Forward 20
    Right 120
    Forward 20
]

let repeat n operations = 
    operations
    |> List.replicate n
    |> List.collect id

let arcr r deg = 
    [ Forward r; Right 1]
    |> repeat deg

let arcl r deg = 
    [ Forward r; Left 1]
    |> repeat deg

let ray r = 
    [
        arcl r 90
        arcr r 90
    ]
    |> List.collect id
    |> repeat 2

let sun size = 
    ray size
    @ [Right 160]
    |> repeat 9

let circles = 
    arcr 1 360
    @ [Right 40]
    |> repeat 9

let petal size =
    arcr size 60
    @ [Right 120]
    @ arcr size 60
    @ [Right 120] 

let flower size = 
    petal size
    @ [Right 60]
    |> repeat 6

let rec polyacc side angle sum = 
    printfn "%A" sum
    if sum > 0 && sum % 360 = 0 then []
    else [Forward side; Right angle] @ polyacc side angle (sum + angle)
    

let poly side angle =
    [ Forward side; Right angle]
    |> repeat 1000

let rec polyspi side angle = 
    match side with
    | 0 -> []
    | _ -> [Forward side; Right angle] @ polyspi (side - 5) angle

let newpoly side angle = 
    [
        Forward side
        Right angle
        Forward side
        Right (2 * angle)
    ]
    |> repeat 100


let webPagePath () = triangle