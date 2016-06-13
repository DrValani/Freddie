open System

[<EntryPoint>]
let main argv = 
    Terrain.go ()
    Console.ReadKey() |> ignore
    0