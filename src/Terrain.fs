module Terrain

open System
open System.IO
open System.Text

let printMap (heights : float[,]) =
    let sb = new StringBuilder()
    let upper = heights.GetLength(0) - 1
    for y in [0..upper] do
        for x in [0..upper] do
            sb.Append (sprintf "%f " heights.[x, y]) |> ignore
        sb.AppendLine() |> ignore
    File.WriteAllText(@"c:\stuff\desktop\out.txt", sb.ToString())
    printfn "%s" (sb.ToString())
    
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

let newHeight (heights : float[,]) nudge verticies =
    let max = heights.GetLength(0) - 1
    verticies
    |> List.filter (fun (x, y) -> x >= 0 && y >= 0 && x <= max && y <= max)
    |> List.map (fun (x, y) -> heights.[x, y])
    |> List.average
    |> nudge


type Orientation = Inline | Diagnal

let setHeight (heights : float[,]) nudge halfCount orientation vertexToSet  =
    let max = heights.GetLength(0) - 1
    let x, y = vertexToSet 
    let sourcePoints = 
        match orientation with 
        | Inline -> [(x - halfCount, y); (x, y + halfCount); (x + halfCount, y); (x, y - halfCount)]
        | Diagnal -> [(x - halfCount, y - halfCount); (x - halfCount, y + halfCount); (x + halfCount, y + halfCount); (x + halfCount, y - halfCount)]
    heights.[x, y] <-
        sourcePoints
        |> List.filter (fun (x, y) -> x >= 0 && y >= 0 && x <= max && y <= max)
        |> List.map (fun (x, y) -> heights.[x, y])
        |> List.average
        |> nudge
    
let setCentre (heights : float[,]) nudge halfCount bottomLeft  =
    match halfCount with 
    | 0 -> ()
    | _ -> 
    let x, y = bottomLeft
    let centre = x + halfCount, y + halfCount
    setHeight heights nudge halfCount Diagnal centre
    

let rec fillSquare random (heights : float[,]) nudgeRange tileCount vertex  =
    printMap heights
    Console.ReadKey() |> ignore
    printfn ""

    match tileCount with 
    | 0 -> failwith "This should never happen."
    | 1 -> ()
    | _ -> 
    let halfCount = tileCount / 2
    let halfRange = nudgeRange / 2.0
    //let bigNudge = nudge random nudgeRange
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
    
    //setHeight heights bigNudge halfCount Diagnal centre
    setHeight heights nudge halfCount Inline midLeft
    setHeight heights nudge halfCount Inline midTop
    setHeight heights nudge halfCount Inline midRight
    setHeight heights nudge halfCount Inline midBottom

    setCentre heights nudge (halfCount/2) bottomLeft
    setCentre heights nudge (halfCount/2) midLeft
    setCentre heights nudge (halfCount/2) centre
    setCentre heights nudge (halfCount/2) midBottom

    let fillSubsquare = fillSquare random heights halfRange halfCount
    fillSubsquare bottomLeft
    fillSubsquare midLeft
    fillSubsquare centre
    fillSubsquare midBottom
    ()
    
let createVertexMap random exp =
    let tileCount = pown 2 exp
    let vericiesCount = tileCount + 1
    let heights = Array2D.zeroCreate vericiesCount vericiesCount

    let initialCornerHeight = 0.5
    let nudgeRange = 0.25
    let nudge = nudge random nudgeRange
    let newCornerHeight () = 
        nudge initialCornerHeight

    for x in [0; tileCount] do
        for y in [0; tileCount] do
            heights.[x, y] <- newCornerHeight ()

    setCentre heights nudge (tileCount/2) (0,0)
    fillSquare random heights nudgeRange tileCount (0, 0)
    heights

let getVertexAndPoint vertexCount point =
    let scale = float (vertexCount - 1)
    let x = scale * fst point
    let y = scale * snd point 
    let vertex = int x, int y
    let subPoint = x - float (int x), y - float (int y)
    vertex, subPoint


// triangle (0, 0) (0, 1) (1 0)
let heightInTriangle originHeight topHeight rightHeight point =
    let x, y = point
    originHeight
    + x * (rightHeight - originHeight)
    + y * (topHeight - originHeight)

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
    
    if x >= y then
        heightInTriangle br bl tr (y, (1.0 - x))
    else
        heightInTriangle tl tr bl ((1.0 - y), x)    

let random seed =
    let seed =
        match seed with
        | None -> Random().Next()
        | Some seed -> seed

    printfn "Seed: %d" seed
    Random(seed)

let getTerrain exp seed : (float -> float -> float) =
    let random = random seed
    let vHeights = createVertexMap random exp
    printMap vHeights
    let maxValue, maxCoord = findMax vHeights
    printfn "Max: %f @ %A" maxValue maxCoord
    fun x y -> getHeight vHeights (x, y)


let go () =
    let seed = Some 282857238
    let seed = Some 389150060
    let seed = Some 643083317
     //let seed = None
    //let terrain = getTerrain 2 seed
    let terrain = getTerrain 3 seed
//
//    let count = 50
//    [0..(count-1)]
//    |> List.map float
//    |> List.map (fun x -> x/(float count))
//    |> List.iter(fun x ->
//        printfn "%f" (terrain x 0.333))

    terrain

