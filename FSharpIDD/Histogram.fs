namespace FSharpIDD

open WebSharper

[<JavaScript>]
module internal Histogram =

    open System

    type Histogram = {
        BinCentres : float list
        BinCounters: int list
        BinWidth: float
    }

    let buildHistogram (samples:float seq) binCount =
        let eps = 1e-6
        let left = Seq.min samples - eps
        let right = Seq.max samples + eps //adding and subtracting eps ensures that the boundary bins will contain boundary points
        let width = (right-left)/float(binCount)
        let centres = List.init binCount (fun idx -> left + width*0.5 + float(idx)*width)
        let boundaries = Array.init binCount (fun idx -> left + float(idx)*width)
        let foundIndices = samples |> Seq.map (fun s -> Array.BinarySearch(boundaries,s))
        let binIndices = foundIndices |> Seq.map (fun idx -> if idx<0 then ~~~idx - 1 else idx)   
        let binCounts = Array.zeroCreate<int> binCount
        binIndices |> Seq.iter (fun (elem:int) -> binCounts.[elem] <- binCounts.[elem] + 1)
        {
            BinCentres = centres
            BinCounters = List.ofArray binCounts
            BinWidth = width
        }

