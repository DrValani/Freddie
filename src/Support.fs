[<AutoOpen>]
module ProgNet.Support
open NUnit.Framework

let __YOUR_IMPLEMENTION_HERE__<'T> : 'T = raise <| new System.NotImplementedException("You must implement this to continue") 

let isTrue check =
    if check
    then printfn "Test passed."
    else failwith "Test failed! check is not true"

let assertFirstEqualToSecond x y = 
    if x = y then printfn "Test passed." else failwith (sprintf "Test failed! %A is not equal to %A" x y)
     
let assertFirstGreaterThanSecond x y = 
    if not (x > y)
    then failwith (sprintf "Test failed! %A is not greater than %A" x y)
    else printfn "Test passed."

let assertFirstLessThanSecond x y = 
    if not (x < y) 
    then failwith (sprintf "Test failed! %A is not less than %A" x y)
    else printfn "Test passed."
