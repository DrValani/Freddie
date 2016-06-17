module Antenna

let L, R, U, D = 'L', 'R', 'U', 'D'
let Parts = [L; R; U; D;]
let random = System.Random()

type Design =
  { Parts : char list
    Reception : float }

let reception parts = Reception.test parts

let createDesigns designCount =
    let partCount = 128

    let createDesign () = 
        let randomPart () = Parts.[random.Next(Parts.Length)]
        let parts = 
            partCount
            |>  List.unfold (fun length -> 
                if length = 0 then None
                else Some (randomPart (), length - 1))
        { Parts = parts; Reception = reception parts }

    designCount
    |>  List.unfold (fun length -> 
            if length = 0 then None
            else  Some(createDesign (), length - 1))
    
let missionComplete [first::rest] =
    let targetReception = 80.0
    first.Reception > targetReception
    

let designs = createDesigns 40
