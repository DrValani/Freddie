module ClimbHill

open System

type Point =
  { X : float 
    Y : float}

type State =
  { Point : Point
    Step : float 
    Height: float }
  
let findHigherPoint current getElevation =
    let stepFactor = 0.8333

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
            Height = getElevation point }

    let bestCandidate = 
        neighbours
        |> List.map newState
        |> List.maxBy (fun s -> s.Height)

    if bestCandidate.Height > current.Height then
      bestCandidate
    else
      { current with Step = current.Step * stepFactor }

let missionComplete minStep state =
    state.Step < minStep

let climb startPoint initialStep getHeight  =
    let initialState = 
       { Point = startPoint
         Step = initialStep 
         Height = getHeight startPoint }
    
    let minimumStep = initialStep / 10000.0

    let rec climbUntilDone state = seq {
        yield state
        if not (missionComplete minimumStep state) then
            let improved = findHigherPoint state getHeight
            yield! climbUntilDone improved }

    climbUntilDone initialState
