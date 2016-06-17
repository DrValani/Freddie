module Antenna

let L, R, U, D = 'L', 'R', 'U', 'D'
let Parts = [L; R; U; D;]
let random = System.Random()

type Design =
  { Parts : char list
    Reception : float }

let reception parts = Reception.test parts

let generateList getMember length =
    length
    |>  List.unfold (fun length ->
            if length = 0 then None
            else Some (getMember (), length - 1))

let createDesign () = 
    let partCount = 128
    let randomPart () = Parts.[random.Next(Parts.Length)]
    let parts = generateList randomPart partCount
    { Parts = parts; Reception = reception parts }

let createDesigns = generateList createDesign 

        
let missionComplete designs =
    let targetReception = 80.0
    match designs with
    | first::_ -> first.Reception > targetReception
    | _ -> failwith "Well, this shouldn't happen"

let shuffle list =
    let array = Array.ofList list
    let len = Array.length array
    for i in [0 .. (len - 1)] do
        let j = random.Next(len)
        let jItem = array.[j]
        array.[j] <- array.[i]
        array.[i] <- jItem    
    List.ofArray array

   
let evolve designs =
    let designCount = List.length designs

    let selectPairs designs =
        List.zip designs (shuffle designs)

    let crossover (design1, design2) =
        let split = random.Next(design1.Parts.Length)
        let newParts = 
            List.concat  [
                design1.Parts |> List.take split
                design2.Parts |> List.skip split ]
        {Parts = newParts; Reception = reception newParts }

    let createCrossovers designs count =
        designs
        |> selectPairs
        |> List.take count
        |> List.map crossover

    let cull designs =
        let cullRate = 0.1
        designs
        |> Seq.mapi (fun i design -> (i, design))
        |> Seq.filter (fun (i, design) -> 
           i > random.Next(designCount) && random.NextDouble() < (cullRate * 2.0)) 
        |> Seq.map snd
        |> List.ofSeq

    let survivors = cull designs
    let replacements = createCrossovers designs (designCount - (survivors.Length))
           
    [survivors; replacements]
    |> Seq.concat
    |> Seq.sortBy (fun design -> -1.0 * design.Reception)
    |> List.ofSeq


