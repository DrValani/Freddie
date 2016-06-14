open System

[<EntryPoint>]
let main argv = 
    let elevationMap = Landscape.getElevationMap () 
    //printfn "%f" (elevationMap (1000.0, 1000.0))
    Console.ReadKey() |> ignore
    0