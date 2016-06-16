module EstimateLife

open Fuel
open ClimbHill

type Line(initialFuel:float, slope:float) =
    member this.InitialFuel = initialFuel
    member this.Slope = slope
    member this.CalcFuel time = initialFuel + time * slope

    
let remainingTime readings currentTime =

    let distance readings (line:Line) =  
        readings
        |> List.averageBy (fun r ->
            (r.Value - line.CalcFuel r.Time) ** 2.0)

    let findBestLine readings =    
        let getHeight {X = initialFuel; Y = slope} = 
            let line = Line(initialFuel, slope)
            -1.0 * distance readings line
        
        let {Point = {X = initialFuel; Y = slope}} = 
            ClimbHill.climb {X = 0.0; Y = 0.0} 1.0 getHeight
            |> Seq.last
        Line(initialFuel, slope)

    let line = findBestLine Fuel.Readings
    let totalTime = line.InitialFuel /  - line.Slope           
    totalTime - currentTime

            
        
        