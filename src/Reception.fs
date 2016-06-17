module Reception

let test design =    
    let L, R, U, D = 'L', 'R', 'U', 'D'

    let addPart endPoint part =
        let h, v = endPoint
        match part with
        | 'L' -> h - 1, v
        | 'R' -> h + 1, v
        | 'U' -> h, v + 1
        | 'D' -> h, v - 1
        | _ -> failwith "Unexpected character. Expected L, R, U or D"

    let points = design |> List.scan addPart (0, 0)

    let symmetry =
        let hEnd, vEnd = points |> List.last
        let total = List.length design |> float
        let dimensionSym endValue = (total - (endValue |> abs |> float)) / total
        ((dimensionSym hEnd)**2.0 * (dimensionSym vEnd)**2.0) ** 3.0

    let spread =
        let hMin, hMax, vMin, vMax =
            ((0, 0, 0, 0), points)
            ||> List.fold (fun (hMin, hMax, vMin, vMax) (h, v) ->              
                (min hMin h, max hMax h, min vMin v, max vMax v))                
        let spread = (hMax - hMin) * (vMax - vMin) |> float
        let maxSpread = ((design |> List.length |> float) / 4.0) ** 2.0
        min 1.0 (spread / maxSpread)    

    100.0 * symmetry * spread

