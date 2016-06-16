open System

let euler x y = Math.E ** -(x ** 2.0 + y ** 2.0)

[<EntryPoint>]
let main argv = 
    let random = new Random()
    Landscape.getElevationMap () |> ignore
    let getElevation = Landscape.getElevationMap ()
    let length = Landscape.length

    let peak = 
        Ascend.climbToPeak 
            (random.NextDouble() * length, random.NextDouble() * length)
            5000.0
            getElevation

//    let little = Surface.getSurface 1 random
//    printfn "middle: %f" (little.GetElevation (0.5, 0.5))
//
//    let peak = 
//        Ascend.climbToPeak 
//            (0.35, 0.35)
//            0.5
//            euler
//            (fun x y -> little.GetElevation (x, y))
//
//    printfn "middle: %f" (little.GetElevation (0.5, 0.5))


    Console.ReadKey() |> ignore
    0