module Ascend

open System

type Point =
  { X : float 
    Y : float}

type State =
  { Point : Point
    Step : float }

let pickStart startPoint initialStep =
    let x, y = startPoint 
    let point = { X = x; Y = y }
    { Point = point
      Step = initialStep }
    
let findHigherPoint state getElevation =
    let { Point = point; Step = step } = state

    let candidates = 
      [ {point with Y = point.Y - step}
        {point with X = point.X + step}
        {point with Y = point.Y + step}
        {point with X = point.X - step} ]

    let bestPoint = 
        List.maxBy getElevation candidates
    
    let bestElevation = getElevation bestPoint

    match bestElevation > (getElevation point) with
    | true -> Some {state with Point = bestPoint}
    | false -> None

let missionComplete minStep state =
    state.Step < minStep

let climbToPeak startPoint startStep getElevation  =
    let minStep = startStep / 10000.0 
    let getElevation (point : Point) =
        getElevation point.X point.Y
    let stepFactor = 0.5
    let describe state =
        let {Point = point; Step = step } = state
        let {X = x; Y = y} = point
        sprintf "x:%f, y:%f, elevation:%f" x y (getElevation point)

    let rec climbUntilDone state =
        printfn "   -->  %s" (describe state)
        match missionComplete minStep state with
        | true -> state.Point
        | false -> 
            match findHigherPoint state getElevation with
            | None -> climbUntilDone { state with Step = (state.Step * stepFactor) }
            | Some better -> climbUntilDone better

    climbUntilDone (pickStart startPoint startStep)


    

