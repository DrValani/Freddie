module Ascend

open System

open Landscape
let elevationMap = getElevationMap ()

let pickStart () =
    let random = Random()
    let pickRandom () = random.NextDouble() * Landscape.length 
    (pickRandom (), pickRandom())
    
let findHigherPoint point step =
    let x, y = point
    let candidates = 
      [ (x - step, y); (x, y + step); (x + step, y); (x, y - step); ]

    let best = 
        candidates
        |> List.map (fun point -> (point, elevationMap point))
        |> List.maxBy (fun (point, elevation) -> elevation)
    
    let bestPoint, bestElevation = best
    match bestElevation > (elevationMap point) with
    | true -> Some bestPoint
    | false -> None

let missionCompleted targetHeight point =
    elevationMap point > targetHeight

let ascendToElevation targetElevation =
    let step = 500.0
    let completed = missionCompleted targetElevation
    let describe point =
        let x, y = point
        sprintf "x:%f, y:%f, elevation:%f" x y (elevationMap point)

    let rec climbUntilDone point =
        printfn "   -->  %s" (describe point)
        match completed point with
        | true -> printfn "Success :-)  Found %s." (describe point)
        | false -> 
            match findHigherPoint point step with
            | None -> printfn "Failed :-(  Stalled at %s." (describe point)
            | Some better -> climbUntilDone better

    climbUntilDone (pickStart ())


    

