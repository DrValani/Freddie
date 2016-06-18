module TransmissionLog

open System.Text.RegularExpressions

type Entry =
  { Temperature : float
    EarthElevation : float
    EarthAzmuth : float
    EarthTime : float
    Success : float }

let split pattern string = Regex.Split(string, pattern)

let Entries =
    @"-20	20	50	14	0
-25	15	60	20	0
-30	10	70	25	0
-15	25	40	10	1
-10	30	30	5	1
-5	40	20	0	1"
    |> split "\r?\n"
    |> Seq.map (split "\s+")
    |> Seq.map (fun logEntry ->
        match logEntry with
        | [|tmp; elv; azm; time; ok|] ->
          { Temperature = float tmp
            EarthElevation = float elv
            EarthAzmuth = float azm
            EarthTime = float time
            Success = if ok = "1" then 1.0 else 0.0 } 
        | _ -> failwith "This shouldn't happen!" )
    |> List.ofSeq
    