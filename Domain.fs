module TurtleGeometry.Domain

type TurtleCommmand = 
    | Forward of int
    | Right of int
    | Left of int