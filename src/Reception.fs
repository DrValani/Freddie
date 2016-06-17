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

//    let symmetry =
//        let balanceOf a b = 
//            let AandBs  = 
//                design |> List.filter (fun p -> p = a || p = b)                    
//            let total = AandBs |> List.length |> float
//            let aProportion =
//                AandBs 
//                |> List.filter (fun e -> e = a) 
//                |> List.length
//                |> float
//                |> fun aCount -> aCount / total
//            let bProportion = 1.0 - aProportion
//            1.0 - (aProportion - bProportion) ** 2.0        
//        let leftRightBalance = balanceOf L R
//        let upDownBalance = balanceOf U D
//        (leftRightBalance * upDownBalance) ** 2.0

    let symmetry =
        let hEnd, vEnd = points |> List.last
        let total = List.length design |> float
        let dimensionSym endValue = (total - (endValue |> abs |> float)) / total
        ((dimensionSym hEnd)**2.0 * (dimensionSym vEnd)**2.0) ** 3.0
        

//    let spread =
//        let move h v part =
//            match part with
//            | 'L' -> h - 1, v
//            | 'R' -> h + 1, v
//            | 'U' -> h, v + 1
//            | 'D' -> h, v - 1
//            | _ -> failwith "Unexpected character. Expected L, R, U or D"
//
//        let hMin, hMax, vMin, vMax, _, _ = 
//            ((0, 0, 0, 0, 0, 0), design)
//            ||> List.fold 
//                    (fun (hMin, hMax, vMin, vMax, h, v) part ->
//                        let h, v = move h v part
//                        ((min h hMin), (max h hMax), (min v vMin), (max v vMax), h, v))
//
//        let spread = (hMax - hMin) * (vMax - vMin) |> float
//        let maxSpread = 
//            design
//            |> List.length
//            |> float
//            |> fun length -> (length / 4.0) ** 2.0
//        min 1.0 (spread / maxSpread)

    let spread =
        let hVals = points |> List.map (fun (h, v) -> h)
        let vVals = points |> List.map (fun (h, v) -> v)
        let hMin = hVals |> List.min
        let hMax = hVals |> List.max
        let vMin = vVals |> List.min
        let vMax = vVals |> List.max

        let spread = (hMax - hMin) * (vMax - vMin) |> float
        let maxSpread = 
            design
            |> List.length
            |> float
            |> fun length -> (length / 4.0) ** 2.0
        min 1.0 (spread / maxSpread)    

    //printfn "old: %f new: %f" spread newSpread
    100.0 * symmetry * spread

