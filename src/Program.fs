open System

open ClimbHill
open BatteryLife
open Antenna
open Wake

let printTitle title =
    printfn ""
    printfn ""
    printfn "%s" title
    printfn "%s" (String('=', title.Length))
    printfn ""
    

let climbMap () = 
    printTitle "Climbing Map"

    let printState state =
        let {Point = {X = x; Y = y}} = state
        printfn "%sm (%f, %f) step:%f"
           (state.Height.ToString("n0")) x y state.Step 

    climbMap ()
    |> Seq.iter printState
    //|> Seq.last |> printState

let getRemainingTime currentTime =
    printTitle "Estimating Battery Life"
    //let start = DateTime(2016, 6, 16, 19, 0, 0)
    let start = DateTime(2016, 6, 23, 9, 0, 0)
    let elapsedMinutes = DateTime.Now.Subtract(start).TotalMinutes
    let remainingMinutes = BatteryLife.remainingTime Fuel.Readings elapsedMinutes
    printfn "%s minutes remaining." (remainingMinutes.ToString("n1"))

let designAntenna () =
    printTitle "Designing Antenna"
    let display design = 
        printf "%s%% - " (design.Reception.ToString("n2"))
        printfn "%s" (design.Parts |> Array.ofList |> String)
//        for (x, y) in Reception.toPoints design.Parts do
//            printfn "%d\t%d" x y
               
    Antenna.design ()
    |> Seq.map Seq.head
    |> Seq.iter display
    //|> Seq.last |> display
    
//    let results = Antenna.design () |> List.ofSeq
//    results |> List.head |> List.head |> display 
//    results |> List.last |> List.head |> display 
    
let wake () =
    printTitle "Wake"
//    TransmissionLog.entriesAsRecords
//    |> List.iter (printfn "%A")

    let x = TransmissionLog.entriesAsLists

    let trainingData =
        TransmissionLog.entriesAsLists
        |> List.map (fun (reception :: inputs) ->
            { Inputs = inputs
              Desired = if reception > 0.5 then 1.0 else -1.0 } )

    let display perceptron =          
        let success, count = successStats trainingData perceptron
        printfn "%d/%d - Bias:%f  Weigths:%A" success count perceptron.BiasWeight perceptron.InputWeights
                    
    Wake.trainPerceptron trainingData
    |> Seq.iter display
    //|> Seq.last |> display
        


[<EntryPoint>]
let main argv = 
//    climbMap ()
//    getRemainingTime 60.0
//    designAntenna ()
    wake ()


    Console.ReadKey() |> ignore
    0