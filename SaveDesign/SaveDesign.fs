﻿open System

[<EntryPoint>]
let main argv = 
    let initialDesign = "DULDUDRURULLURLRRURRULDLULDLUDURRUULRRURLLDRRLRUDLRUUULUDLLRDLLUDUUDDDDDDLRURUDUDRRUULDDUDRRLRRDLRRUURDDLRUULRRRRURUDRDUDURDURUU"
    let finalDesign = "DRDRDRRRDDDRDDDDRDDLRRLDLLLDLLLDLRLDLLDDLULULUULLLUULLLDULLULLLLULDLUUDLURURUUDURUUUUDDRRDRURDRURRUUUUDDRRRURRULRUURRURRURDLDRDD"

    Png.saveImage initialDesign finalDesign
    printfn "Done. Look for '%s' on your Desktop." Png.filename
    Console.ReadKey() |> ignore
    0 
