open System

open ClimbHill

let euler x y = Math.E ** -(x ** 2.0 + y ** 2.0)

let printState state =
    let {Point = {X = x; Y = y}} = state
    printfn "  --->  x:%f, y:%f, step:%f, elevation:%f" 
        x y state.Step state.Elevation 

[<EntryPoint>]
let main argv = 
    let random = new Random()
    let hill = Landscape.getElevationMap ()
    let length = Landscape.length
    
    let startPoint = (random.NextDouble() * length, random.NextDouble() * length)
    climb startPoint length hill
    |> Seq.iter printState

    Console.ReadKey() |> ignore
    0