module Terrain

open System


let printMap (heights : float[,]) =
    let upper = heights.GetLength(0) - 1
    for y in [0..upper] do
        for x in [0..upper] do
            printf "%f " heights.[x, y]
        printfn ""

let findMax (heights : float[,])  =
    let mutable maxValue = -1.0
    let mutable maxCoord = (-1, -1)

    let upper = heights.GetLength(0) - 1    
    for y in [0..upper] do
        for x in [0..upper] do
            let value = heights.[x, y]
            if value > maxValue then
                maxValue <- value
                maxCoord <- (x, y)
    maxValue, maxCoord


let randMinusToPlusOne (random : Random) =
    random.NextDouble() * 2.0 - 1.0
           
let nudge random range initial =
    initial + (range * randMinusToPlusOne random)

let newHeight (array : float[,]) nudge verticies =
    verticies
    |> List.map (fun (x, y) -> array.[x, y])
    |> List.average
    |> nudge

let rec fillSquare random (heights : float[,]) nudgeRange tileCount vertex  =
    match tileCount with 
    | 0 -> failwith "This should never happen."
    | 1 -> ()
    | tileCount -> 
    let halfCount = tileCount / 2
    let halfRange = nudgeRange / 2.0
    let bigNudge = nudge random nudgeRange
    let nudge = nudge random halfRange
    let x, y = vertex
    
    let bottomLeft = x, y
    let topLeft = x, y + tileCount
    let topRight = x + tileCount, y + tileCount
    let bottomRight = x + tileCount, y

    let centre = x + halfCount, y + halfCount
    let midLeft = x, y + halfCount
    let midTop = x + halfCount, y + tileCount
    let midRight = x + tileCount, y + halfCount
    let midBottom = x + halfCount, y
    
    heights.[fst centre, snd centre] <-
        newHeight heights bigNudge  [bottomLeft; topLeft; topRight; bottomRight]

    heights.[fst midLeft, snd midLeft] <-
        newHeight heights nudge  [centre; bottomLeft; topLeft; ]

    heights.[fst midTop, snd midTop] <-
        newHeight heights nudge  [centre; topLeft; topRight; ]

    heights.[fst midRight, snd midRight] <-
        newHeight heights nudge  [centre; topRight; bottomRight; ]

    heights.[fst bottomRight, snd bottomRight] <-
        newHeight heights nudge  [centre; bottomRight; bottomLeft; ]

    let fillSubsquare = fillSquare random heights halfRange halfCount
    fillSubsquare bottomLeft
    fillSubsquare midLeft
    fillSubsquare centre
    fillSubsquare midBottom
    
    
let createVerticiesMap random exp =
    let tileCount = pown 2 exp
    let vericiesCount = tileCount + 1
    let heights = Array2D.zeroCreate vericiesCount vericiesCount

    let initialCornerHeight = 0.5
    let nudgeRange = 0.25
    let newCornerHeight () = 
        nudge random nudgeRange initialCornerHeight

    for x in [0; tileCount] do
        for y in [0; tileCount] do
            heights.[x, y] <- newCornerHeight ()

    fillSquare random heights nudgeRange tileCount (0, 0)
    heights

let getVertexAndPoint vertexCount point =
    let scale = float (vertexCount - 1)
    let x = scale * fst point
    let y = scale * snd point 
    let vertex = int x, int y
    let subPoint = x - float (int x), y - float (int y)
    vertex, subPoint

let getHeight (vHeights : float[,]) point =
    match point with
    | x, _ when x < 0.0 || x >= 1.0 -> 0.0
    | _, y when y < 0.0 || y >= 1.0 -> 0.0
    | x, y ->
    let vertexCount = vHeights.GetLength(0)
    let (vx, vy), (px, py) = getVertexAndPoint vertexCount point
    let bl = vHeights.[vx, vy]
    let tl = vHeights.[vx, vy + 1]
    let tr = vHeights.[vx + 1, vy + 1]
    let br = vHeights.[vx + 1, vy]
    (bl * (1. - px) * (1. - py))
    + (tl  * (1. - px) * py)
    + (tr * px * py)
    + (br * px * (1. - py))


let random =
    let seed = Random().Next()
    // let seed = 282857238
    // let seed = 389150060
    printfn "Seed: %d" seed
    Random(seed)

let go () =
//    let vHeights = createVerticiesMap random 10
//    //printMap vHeights
//    let maxValue, maxCoord = findMax vHeights
//    printfn "Max: %f @ %A" maxValue maxCoord
    let vh = array2D [|  [| 1.0; 0.0 |] ; [| 0.0; 1.0 |] |]
    let gh x y = getHeight vh (x, y)
    printfn "bl: %f" (gh 0.001 0.001)
    printfn "tl: %f" (gh 0.001 0.999)
    printfn "tr: %f" (gh 0.999 0.999)
    printfn "br: %f" (gh 0.999 0.001)
    let p = 0.1, 0.1
    //let h = getHeight vh p
    printfn "%A: %f" p (gh (fst p) (snd p))
