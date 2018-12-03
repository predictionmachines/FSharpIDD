open FsharpIDD.Chart
open FsharpIDD.Plot
open System.IO
open System.Diagnostics
open FsharpIDD

[<EntryPoint>]
let main argv = 
    // fake data generation
    let Xseries = Array.init 1000 (fun i -> float(i)*0.01)

    let Yseries = Xseries |> Array.map (fun x -> sin(2.0*x)/x)
    let Yseries2 = Xseries |> Array.map (fun x -> sin(5.0*x)/x)
        
    // composing a chart
    let curve1 = 
        createPolyline Xseries Yseries
        |> setLineName "Curve 1"
        |> setLineStrokeColour Colour.Red
        |> setLineThickness 2.0
    
    let curve2 =
        createPolyline Xseries Yseries2
        |> setLineStrokeColour Colour.Blue
        |> setLineThickness 3.0
        |> setLineName "Curve 2"

    let chart =
        Chart.Empty
        |> Chart.addPolyline curve1
        |> Chart.addPolyline curve2

    // getting HTML that represents the chart
    let generatedChart = chart |> toHTML

    let template = File.ReadAllText "template.html"

    // Injecting it into the HTML template
    let html = template.Replace("<%PLACEHOLDER%>",generatedChart)
    printfn "%s" generatedChart
    let writer = File.CreateText("chart.html")
    writer.Write(html)
    writer.Close()
    // showing the result HTML with browser
    Process.Start("chart.html") |> ignore
    0
