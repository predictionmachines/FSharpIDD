[<WebSharper.JavaScript>]
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

let corrMapVals = Array2D.init 6 6 (fun x y -> let y = 5-y in if x >= y then nan else (1.0 - 0.4 * float(abs(y - x))))

let ticks = Array.init 7 (fun idx -> float(idx))
let labels = ["one"; "two"; "three"; "four"; "five"; "six"]
let labelsRev = List.rev labels
    

let corrMap =
    Heatmap.createHeatmap corrMapX corrMapX corrMapVals
    |> Heatmap.setName "Correlation"
    |> Heatmap.setPalette (Heatmap.Palette.IddPaletteString "-1=red,#ff3333=-0.8=#ff3333,#ff6666=-0.6=#ff6666,#ff9999=-0.4=#ff9999,#ffcccc=-0.2=#ffcccc,white=0.0=white,#ccccff=0.2=#ccccff,#9999ff=0.4=#9999ff,#6666ff=0.6=#6666ff,#3333ff=0.8=#3333ff,blue=1")

let corrMapTest =
    "Correlation map between 6 species",
    [    
        "|> Heatmap.setPalette (Heatmap.Palette.IddPaletteString '-1.0=blue,white,red=1.0')"
        "Correlation map"
    ],
     {
        Empty with            
            Width = 300
            Height = 200
            Xaxis = createTiltedLabelledAxis ticks labels 90.0
            Yaxis = createLabelledAxis ticks labelsRev
            IsLegendEnabled = LegendVisibility.Hidden
            IsTooltipPlotCoordsEnabled=false
            Title = "Correlation"
            TooltipDelay = Some 0.0
     }    
    |> Chart.addHeatmap corrMap