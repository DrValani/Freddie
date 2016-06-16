module Ascend

open System

type Point =
  { X : float 
    Y : float}

type State =
  { Point : Point
    Step : float 
    Elevation: float }

let pickStart startPoint initialStep  getElevation =
    let x, y = startPoint 
    let point = { X = x; Y = y }
    { Point = point
      Step = initialStep 
      Elevation = getElevation point }
    
let findHigherPoint state getElevation =
    let stepFactor = 0.66
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
    | true -> {state with Point = bestPoint}
    | false -> {state with Step = state.Step * stepFactor}

let missionComplete minStep state =
    state.Step < minStep

let climbToPeak startPoint startStep getElevation  =
    let minStep = startStep / 10000.0 
    let getElevation (point : Point) =
        getElevation point.X point.Y
    let stepFactor = 0.66
    let describe state =
        let {Point = point; Step = step } = state
        let {X = x; Y = y} = point
        sprintf "x:%f, y:%f, elevation:%f" x y (getElevation point)

    let rec climbUntilDone state =
        printfn "   -->  %s" (describe state)
        match missionComplete minStep state with
        | true -> state.Point
        | false -> climbUntilDone (findHigherPoint state getElevation)

    let initialState = pickStart startPoint startStep getElevation
    climbUntilDone initialState


    

