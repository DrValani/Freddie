module Fuel

type Reading = {Time : float; Value : float }

let Readings = 

    let times = [
        0.0
        5.0
        10.0
        15.0
        20.0
        25.0
        30.0
        35.0
        40.0
        45.0
        50.0
        55.0
        60.0 ]

    let values = [
        55.21
        46.03
        49.81
        51.9
        48.48
        48.15
        40.24
        44.3
        42.21
        41.44
        35.19
        36.39
        36.22 ]

    (times, values)
    ||> List.map2 (fun t v -> {Time = t; Value = v})