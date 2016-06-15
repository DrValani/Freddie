open System

[<EntryPoint>]
let main argv = 
    Ascend.ascendToElevation 3000.0
    Console.ReadKey() |> ignore
    0