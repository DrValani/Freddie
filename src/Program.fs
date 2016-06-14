open System

[<EntryPoint>]
let main argv = 
    Terrain.go () |> ignore
    Console.ReadKey() |> ignore
    0