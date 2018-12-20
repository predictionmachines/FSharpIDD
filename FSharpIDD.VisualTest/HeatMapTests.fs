module HeatMapTests

open FSharpIDD.Plots
open FSharpIDD.Chart
open FSharpIDD
open FSharpIDD.Plots.Markers

let y = Array.init<float> 51 (fun i -> float(i)*0.1-2.5)
let x = Array.init<float> 26 (fun i -> float(i)*0.2-2.5)

let gradientVals = Array2D.init 26 51 (fun i j -> let x,y = x.[i],y.[j] in sin(sqrt((y*y+x*x))/2.5 * System.Math.PI))

let descreteVals = Array2D.init 25 50 (fun i j -> let x,y = x.[i]+0.1, y.[j]+0.05 in sin(sqrt((y*y+x*x))/2.5 * System.Math.PI))

let Empty : Chart =
    Chart.Empty
    |> Chart.setSize 300 200
    |> Chart.setVisibleRegion (Autofit 3)
//    |> Chart.setNavigationEnabled false

let gradient = Heatmap.createHeatmap x y gradientVals
let descrete = Heatmap.createHeatmap x y descreteVals


let basicGradientHeatmapTest =
    "Simple gradient heatmap",
    [
        "Heatmap.createHeatmap x y gradientVals"
        "Grey scale gradient heatmap with 0.0 based symmetry"
    ],
    Chart.addHeatmap gradient Empty

let basicDescreteHeatmapTest =
    "Simple descrete heatmap",
    [
        "Heatmap.createHeatmap x y descreteVals"
        "Grey scale descrete heatmap with 0.0 based symmetry"
    ],
    Chart.addHeatmap descrete Empty


    
let corrMapX = Array.init<float> 7 (fun i -> float(i))
let corrMapY = Array.init<float> 7 (fun i -> float(i))

let random = System.Random(1)

let corrMapVals = Array2D.init 6 6 (fun x y -> let x = 5-x in if y>=x then nan else random.NextDouble()*2.0-1.0)

let ticks = Array.init 6 (fun idx -> 0.5+float(idx))
let labels = ["a"; "b"; "c"; "d"; "e"; "f"]
    

let corrMap =
    Heatmap.createHeatmap corrMapX corrMapY corrMapVals
    |> Heatmap.setPalette (Heatmap.Palette.IddPaletteString "-1.0=blue,white,red=1.0")

let corrMapTest =
    "Correlation map between 6 species",
    [    
        ""
    ],
    Empty
    |> Chart.setSize 600 400
    |> Chart.addHeatmap corrMap 
    |> Chart.setXaxis (Labelled(ticks,labels))
    |> Chart.setYaxis (Labelled(ticks,labels))