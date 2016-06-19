module TransmissionLog

let trainingEntries = 
 @"0.2 -20	20	50	14
0.3 -25	15	60	20
0.4 -30	10	70	25
0.7 -15	25	40	10
0.9 -10	30	30	5
0.8 -5	40	20	0"


open System.Text.RegularExpressions

type Entry =
  { Success : float
    Temperature : float
    EarthElevation : float
    EarthAzmuth : float
    EarthTime : float }

let split pattern string = 
    Regex.Split(string, pattern)
    |> List.ofArray

let entriesAsLists =
    trainingEntries
    |> split "\r?\n"
    |> List.map (split "\s+")
    |> List.map (List.map float)


let entriesAsRecords =
    entriesAsLists
    |> Seq.map (fun logEntry ->
        match logEntry with
        | [success; tmp; elv; azm; time] ->
          { Success = if success > 0.5 then 1.0 else 0.0
            Temperature = tmp
            EarthElevation = elv
            EarthAzmuth = azm
            EarthTime =  time } 
        | _ -> failwith "This shouldn't happen!" )
    |> List.ofSeq
    