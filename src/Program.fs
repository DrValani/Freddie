open System

open ClimbHill
open EstimateLife
open Antenna

let climbHill () = 
    let printState state =
        let {Point = {X = x; Y = y}} = state
        printfn "  ---> %sm (%f, %f) step:%f"
           (state.Height.ToString("n0")) x y state.Step 

    let length = Landscape.length
    let elevationMap = Landscape.getElevationMap ()
    let getHeight point = elevationMap point.X point.Y             
    
    let startPoint = 
        { X = 0.5 * length 
          Y = 0.5 * length }

    climb startPoint length getHeight
    |> Seq.iter printState

let getRemainingTime currentTime =
    printfn ""
    printfn ""
    printfn "Linear Regression"
    printfn "================="
    printfn ""
    let start = DateTime(2016, 6, 16, 19, 0, 0)
    //let start = DateTime(2016, 6, 23, 9, 0, 0)
    let elapsedMinutes = DateTime.Now.Subtract(start).TotalMinutes
    let remainingMinutes = EstimateLife.remainingTime Fuel.Readings elapsedMinutes
    printfn "%s minutes remaining." (remainingMinutes.ToString("n1"))

[<EntryPoint>]
let main argv = 
    //climbHill ()
    //getRemainingTime 60.0
    
    let designs = Antenna.createDesigns 10
    for ant in designs do
            printfn "reception: %f" ant.Reception 

    //printf "max: %f" (designs |> List.map (fun a -> a.Reception) |> List.max)

    Console.ReadKey() |> ignore
    0