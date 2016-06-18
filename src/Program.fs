open System

open ClimbHill
open EstimateLife
open Antenna

let printTitle title =
    printfn ""
    printfn ""
    printfn "%s" title
    printfn "%s" (String('=', title.Length))
    printfn ""
    

let climbHill () = 
    printTitle "Finding High Point"

    let printState state =
        let {Point = {X = x; Y = y}} = state
        printfn "%sm (%f, %f) step:%f"
           (state.Height.ToString("n0")) x y state.Step 

    let length = Landscape.length
    let elevationMap = Landscape.getElevationMap ()
    let getHeight point = elevationMap point.X point.Y             
    
    let startPoint = 
        { X = 0.5 * length 
          Y = 0.5 * length }

    climb startPoint length getHeight
    //|> Seq.iter printState
    |> Seq.last |> printState

let getRemainingTime currentTime =
    printTitle "Estimating Battery Life"
    //let start = DateTime(2016, 6, 16, 19, 0, 0)
    let start = DateTime(2016, 6, 23, 9, 0, 0)
    let elapsedMinutes = DateTime.Now.Subtract(start).TotalMinutes
    let remainingMinutes = EstimateLife.remainingTime Fuel.Readings elapsedMinutes
    printfn "%s minutes remaining." (remainingMinutes.ToString("n1"))

let designAntenna () =
    printTitle "Designing Antenna"
    let display design = 
        printfn "%s%%" (design.Reception.ToString("n2"))
        printfn "%A" (design.Parts |> Array.ofList |> String)
//        for (x, y) in Reception.toPoints design.Parts do
//            printfn "%d\t%d" x y
    
    Antenna.design ()
    |> Seq.map List.head
    |> Seq.iter display
    //|> Seq.last |> display
    

[<EntryPoint>]
let main argv = 
    climbHill ()
    getRemainingTime 60.0
    designAntenna ()
        


    Console.ReadKey() |> ignore
    0