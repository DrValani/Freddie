#load "Support.fs"
#load "Surface.fs"
#load "Landscape.fs"
#load "ClimbHill.fs"

open ProgNet
open ProgNet.ClimbHill
open ProgNet.Landscape

let elevationMap = getElevationMap()
let getNewAltitude point = elevationMap point.X point.Y 

let ``Should return an elevation with a higher altitude`` () =
    let current =
        { Point     = { X = 13.2; Y = 43.5 }
          Step      = 25.0
          Elevation = 120.0 }

    let originalAltitude = 1.0

    let newPoint = findHigherPoint current getNewAltitude

    assertFirstLessThanSecond originalAltitude newPoint.Elevation
