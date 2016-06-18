#r "packages/NUnit.2.6.4/lib/nunit.framework.dll"
#load "Support.fs"
#load "Surface.fs"
#load "Landscape.fs"
#load "ClimbHill.fs"

open ProgNet
open ProgNet.ClimbHill
open ProgNet.Landscape

let elevationMap = getElevationMap()
let getNewAltitude point = elevationMap point.X point.Y 
let length = Landscape.length / 2.
let startPoint = 
    { X = 0.5 * length 
      Y = 0.5 * length }
let initialState = 
    { Point = startPoint
      Step = length 
      Elevation = getNewAltitude startPoint }

let ``Freddie will locate 4 point around his location`` () : unit =
    let expectedNeighboursCount = 4

    let neighboursFound = neighbours initialState |> List.length

    assertFirstEqualToSecond 4 neighboursFound


let ``Should return an elevation with a higher altitude`` () =
    let neighboursFound = neighbours initialState

    printfn "%A" <| getNewAltitude initialState.Point

    let higherPoint = neighboursFound |> Seq.map getNewAltitude |> Seq.max

    assertFirstEqualToSecond (float 2636.410473) (float higherPoint)

//``Freddie will locate 4 point around his location`` () 
``Should return an elevation with a higher altitude`` ()