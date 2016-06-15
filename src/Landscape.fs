module Landscape

open Surface

open System
open System.IO
open System.Text

//type Elevation =  x:float -> y:float -> float

let length = Math.PI * 2229.0

let getElevationMap () =

    let random seed =
        let seed =
            match seed with
            | None -> Random().Next()
            | Some seed -> seed
        printfn "Seed: %d" seed
        Random(seed)

    let writeFile name text =
        let dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        let file = Path.Combine(dir, name)
        File.WriteAllText(file, text)

    let printSummary s = 
        printfn "Max: %f @ %A; Min: %f %A" s.MaxElevation s.MaxLocation s.MinElevation s.MinLocation


    let getLandscape exp length min max seed  =
        random seed    
        |> getSurface exp     
        |> scale length min max 


//    let seed = None
//    let rec repeat () =
//        getTerrain 10 seed |> ignore
//        //Console.ReadKey() |> ignore
//        repeat()
//    repeat ()

    let seed = Some 751071074  // Max: 0.573640 @ (768, 256); Min: 0.393827 (0, 512)
    printfn "length: %f" length
    // Max: 3848.000000 @ (5251.957519, 1750.652506); Min: 545.000000 (0.0, 3501.305012)
    let landscape = getLandscape 10 length 545.0 3848.0 seed
    printSummary landscape
    landscape
        |> pointsAsString 254
        |> writeFile "points.txt"
    //landscape.GetElevation
    fun x y -> landscape.GetElevation (x, y)