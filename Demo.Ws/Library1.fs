module Demo

open FSharpIDD
open FSharpIDD.Chart
open FSharpIDD.Plot.Polyline

open WebSharper

[<assembly: WebSharper.JavaScriptExport("FSharpIDD")>]
do ()

[<JavaScript>]
let getChart() =
    // fake data generation
    let Xseries = Array.init 1000 (fun i -> float(i+1)*0.01)

    let Yseries1 = Xseries |> Array.map (fun x -> sin(2.0*x)/x)
    let Yseries2 = Xseries |> Array.map (fun x -> sin(5.0*x)/x)
    let Yseries3 = Xseries |> Array.map (fun x -> sin(5.0*x+1.0)/x)           

    // Specifying some of the polyline options with setOption call
    let curve1 =
        createPolyline Xseries Yseries1
        |> setOptions (Options(Name = "Curve 1"))
    
    // Specifying some of the polyline options with a series of set... calls
    let curve2 = 
        createPolyline Xseries Yseries2
        |> setName "Curve 2"
        |> setStrokeColour Colour.Green
        |> setThickness 2.0
    
    // Specifying some of the polyline options with a record recreation
    let curve3 = createPolyline Xseries Yseries3
    let curve3 =
        {
             curve3 with
                Name = Some("Curve 3")
                Thickness = Some(1.0)
        }

    let chart =
        Chart.Empty
        |> Chart.addPolyline curve1
        |> Chart.addPolyline curve2
        |> Chart.addPolyline curve3

    // getting HTML that represents the chart
    let generatedChart = chart |> toHTML
    generatedChart