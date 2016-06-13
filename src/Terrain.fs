module Terrain

open System

let randMinusToPlusOne (random : Random) =
    random.NextDouble() * 2.0 - 1.0
    
       
let nudge random range initial =
    initial + (range * randMinusToPlusOne random)


let printMap (heights : float[,]) =
    let upper = heights.GetLength(0) - 1
    for y in [0..upper] do
        for x in [0..upper] do
            printf "%f " heights.[x, y]
        printfn ""
    
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

    heights


let random =
    let seed = Random().Next()
    printfn "Seed: %d" seed
    Random(seed)

let go () =
    createVerticiesMap random 3
    |> printMap

