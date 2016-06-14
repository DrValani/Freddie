module Terrain

open System
open System.IO
open System.Text

let randMinusToPlusOne (random : Random) =
    random.NextDouble() * 2.0 - 1.0
           
let nudge random range initial =
    initial + (range * randMinusToPlusOne random)

type Orientation = Inline | Diagonal

let setHeight (heights : float[,]) nudge halfCount orientation vertexToSet  =
    let max = heights.GetLength(0) - 1
    let x, y = vertexToSet 
    let sourcePoints = 
        match orientation with 
        | Inline -> [(x - halfCount, y); (x, y + halfCount); (x + halfCount, y); (x, y - halfCount)]
        | Diagonal -> [(x - halfCount, y - halfCount); (x - halfCount, y + halfCount); (x + halfCount, y + halfCount); (x + halfCount, y - halfCount)]
    heights.[x, y] <-
        sourcePoints
        |> List.filter (fun (x, y) -> x >= 0 && y >= 0 && x <= max && y <= max)
        |> List.map (fun (x, y) -> heights.[x, y])
        |> List.average
        |> nudge    
    
let createVertexMap random exp =
    let tileCount = pown 2 exp
    let vericiesCount = tileCount + 1
    let heights = Array2D.zeroCreate vericiesCount vericiesCount

    let initialCornerHeight = 0.5
    let nudgeRange = 0.25
    let newCornerHeight () = 
        initialCornerHeight //|> nudge random nudgeRange

    for x in [0; tileCount] do
        for y in [0; tileCount] do
            heights.[x, y] <- newCornerHeight ()

    let enumerateCentres halfCount = seq{
        let mids = [halfCount .. (2 * halfCount) .. tileCount]
        for y in mids do
            for x in mids do
                yield (x, y)}


    let enumerateMidPoints halfCount = seq{
        let mids = [halfCount .. (2 * halfCount) .. tileCount]
        let edges = [0 .. (2 * halfCount) .. tileCount]
        for y in mids do
            for x in edges do
                yield (x, y)
        for y in edges do
            for x in mids do
                yield (x, y)}

    let rec fillSquares nudgeRange halfCount =
        match halfCount with
        | 0 -> ()
        | _ ->
            let nudge = nudge random nudgeRange
            enumerateCentres halfCount
            |> Seq.iter (setHeight heights nudge halfCount Diagonal)  
            enumerateMidPoints halfCount
            |> Seq.iter (setHeight heights nudge halfCount Inline)        
            fillSquares (nudgeRange / 2.0) (halfCount / 2)

    fillSquares (nudgeRange / 2.0) (tileCount / 2)
    heights

       
let getVertexAndPoint vertexCount point =
    let scale = float (vertexCount - 1)
    let x = scale * fst point
    let y = scale * snd point 
    let vertex = int x, int y
    let subPoint = x - float (int x), y - float (int y)
    vertex, subPoint

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


let printMap (heights : float[,]) =
    let sb = new StringBuilder()
    let upper = heights.GetLength(0) - 1
    for y in [0..upper] do
        for x in [0..upper] do
            sb.Append (sprintf "%f " heights.[x, y]) |> ignore
        sb.AppendLine() |> ignore
    File.WriteAllText(@"c:\stuff\desktop\out.txt", sb.ToString())
    //printfn "%s" (sb.ToString())
    
let getStats (heights : float[,])  =
    let mutable maxValue = -1.0
    let mutable maxCoord = (-1, -1)
    let mutable minValue = 2.0
    let mutable minCoord = (-1, -1)

    let upper = heights.GetLength(0) - 1    
    for y in [0..upper] do
        for x in [0..upper] do
            let value = heights.[x, y]
            if value > maxValue then
                maxValue <- value
                maxCoord <- (x, y)
            if value < minValue then
                minValue <- value
                minCoord <- (x, y)
    maxValue, maxCoord, minValue, minCoord

let getTerrain exp seed : (float -> float -> float) =
    let random = random seed
    let vHeights = createVertexMap random exp
    //printMap vHeights 
    let maxValue, maxCoord, minValue, minCoord = getStats vHeights
    printfn "Max: %f @ %A; Min: %f %A" maxValue maxCoord minValue minCoord
    fun x y -> getHeight vHeights (x, y)


let go () =

    let seed = None
    let seed = Some 751071074  //  Seed: 751071074  Max: 0.573640 @ (768, 256); Min: 0.393827 (0, 512)

//    let rec repeat () =
//        getTerrain 10 seed |> ignore
//        //Console.ReadKey() |> ignore
//        repeat()
//    repeat ()

    let terrain = getTerrain 10 seed
    let sb = new StringBuilder()
    let upper = 254
    let divisor = (float upper) + 0.1
    for y in [0..upper] do
        for x in [0..upper] do
            let X = (float x) / divisor
            let Y = (float y) / divisor
            sb.Append (sprintf "%f " (terrain X Y)) |> ignore
        sb.AppendLine() |> ignore
    File.WriteAllText(@"c:\stuff\desktop\point.txt", sb.ToString())
    //printfn "%s" (sb.ToString())


    ()

