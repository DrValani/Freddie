module Wake

type Perceptron =
    { Weights : float list
      Bias : float }

let evaluate perceptron inputs =
    let weightedInput = 
        (perceptron.Weights, inputs)
        ||> Seq.map2 (*)
        |> Seq.sum
    if weightedInput > perceptron.Bias then
        1.0
    else
        0.0

let createPerceptron inputCount =
    { Weights = List.init inputCount (fun _ -> 0.0)
      Bias = 0.0 }

let trainPerceptron trainingData perceptron =
    perceptron

