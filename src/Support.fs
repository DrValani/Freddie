[<AutoOpen>]
module ProgNet.Support

let __YOUR_IMPLEMENTION_HERE__<'T> : 'T = raise <| new System.NotImplementedException("You must implement this to continue") 

let assertEqual x y = 
    if x <> y then failwith (sprintf "%A is not equal to %A" x y)

let assertFirstGreaterThanSecond x y = 
    if x > y then failwith (sprintf "%A is not greater than %A" x y)

let assertFirstLessThanSecond x y = 
    if x < y then failwith (sprintf "%A is not less than %A" x y)