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
    
let findHigherPoint current getElevation =
    let stepFactor = 0.66

    let neighbours = 
        let point = current.Point
        let step = current.Step
        [ {point with Y = point.Y - step}
          {point with X = point.X + step}
          {point with Y = point.Y + step}
          {point with X = point.X - step} ]

    let newState point =
        { current with
            Point = point 
            Elevation = getElevation point }

    let bestCandidate = 
        neighbours
        |> List.map newState
        |> List.maxBy (fun s -> s.Elevation)

    if bestCandidate.Elevation > current.Elevation then
      bestCandidate
    else
      { current with Step = current.Step * stepFactor }

let missionComplete minStep state =
    state.Step < minStep

let climbToPeak startPoint startStep getElevation  =
    let minStep = startStep / 10000.0 
    let getElevation {X = x; Y = y} =
        getElevation x y

    let rec climbUntilDone state = seq {
        yield state
        if not (missionComplete minStep state) then
            let improved = findHigherPoint state getElevation
            yield! climbUntilDone improved
    }

    let initialState = pickStart startPoint startStep getElevation
    climbUntilDone initialState
