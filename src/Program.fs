open System

open ClimbHill
open BatteryLife
open Antenna
open Wake

open NNetTrainer
open MartianSymbols

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

let getBatteryLife currentTime =
    printTitle "Estimating Battery Life"
    let start = DateTime(2016, 6, 1, 0, 0, 0)
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
    

let wake () =
    printTitle "Control Antenna"
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
        
let learnMartian () =
    printTitle "Learning Martian"
    let display trainingSet =
        let printStats title {Total = total; Correct = correct; Percent = percent } =
            Console.WriteLine("{0}: {1:n2}% correct ({2:n0}/{3:n0})", 
                title, percent, correct, total)
        trainingSet.TrainingStats |> printStats "    Training"
        trainingSet.TestStats |> printStats "        Test"
        printfn ""
        

    let sampleCount = 2000        // number of samples to use (max 21,000)
    let learnRate = 0.05          // learns faster but reduces final accuracy (e.g.: 0.001 - 0.1)
    let requiredAccuracy =  70.0  // when to stop

    MartianSymbols.learn  sampleCount learnRate requiredAccuracy
        |> Seq.iter display    



[<EntryPoint>]
let main argv = 
//    climbMap ()
    getBatteryLife 60.0
//    designAntenna ()
//    wake ()
//    learnMartian ()

    printfn ""
    printfn "Done."
    Console.ReadKey() |> ignore
    0