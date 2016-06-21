module Wake

type Perceptron =
    { InputWeights : float list
      BiasWeight : float }

type TrainingEntry = 
    { Inputs : float list
      Desired : float}

let bias = 1.0stats.stackexchange.com/questions/143996/what-is-the-difference-between-linear-perceptron-regression-and-ls-linear-regres

let activate total =
    if total > 0.0 
    then 1.0 else -1.0


let evaluate perceptron inputs =
    let weightedInput = 
        Seq.fold2
            (fun sum weight input -> sum + weight * input)
            0.0
            perceptron.InputWeights
            inputs
    let total = weightedInput + (perceptron.BiasWeight * bias)
    activate total

let createPerceptron inputCount =
    { InputWeights = List.init inputCount (fun _ -> 0.0)
      BiasWeight = 0.0 }

let trainWithEntry learningRate perceptron trainingEntry =    
    let guess = evaluate perceptron trainingEntry.Inputs
    let error = trainingEntry.Desired - guess

    let updateWeight weight input = 
        weight + input * error * learningRate

    let newInputWeights =
        List.map2
            updateWeight
            perceptron.InputWeights   
            trainingEntry.Inputs
    let newBiasWeight = 
        updateWeight perceptron.BiasWeight bias
    //printfn "bias:%f  - Weight:%f" newBiasWeight (newInputWeights.Head)
    { InputWeights = newInputWeights
      BiasWeight = newBiasWeight } 

let trainWithSeries learningRate trainingData perceptron =
    List.fold
        (trainWithEntry learningRate)
        perceptron
        trainingData

let successStats trainingData perceptron =
    let successCount =
        trainingData
        |> List.filter (fun entry -> 
            entry.Desired = evaluate perceptron entry.Inputs )
        |> List.length
    (successCount, trainingData.Length)
    
let doneTraining trainingData perceptron =
    let successCount, count = successStats trainingData perceptron
    successCount = count

let trainPerceptron trainingData =
    let learningRate = 0.1
    let sample = List.head trainingData
    let guess = createPerceptron sample.Inputs.Length

    Common.improveIncrementally 
        guess 
        (trainWithSeries learningRate trainingData)
        (doneTraining trainingData)
    
