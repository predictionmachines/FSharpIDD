open System.IO
open System.Diagnostics
open FSharpIDD
open CollectionToHtml
open FSharpIDD.Plot
open FSharpIDD.Chart
open FSharpIDD.Plot.Polyline
open FSharpIDD.Colour

[<EntryPoint>]
let main argv =

    let template = File.ReadAllText "template.html"

    let Xseries = Array.init 3 (fun i -> float(i+1))
    let Yseries1 = Xseries |> Array.map (fun x -> sin(2.0*x)/x)

    let Empty : Chart = {
        Width = 400
        Height = 300
        Title = null
        Xlabel = null
        Ylabel = null
        Plots = []
    }
    
    let red5ThickRedColourRoundCurve : Polyline.Plot =
        {
            X = Xseries
            Y = Yseries1
            Name = "Red"
            Colour = Colour.Red
            Thickness = 5.0
            LineCap = LineCap.Round
            LineJoin = LineJoin.Round
        }


    // Empty chart
    let emptyChartStr = Empty |> toHTML
    let emptyChartTest = "1. Empty chart", emptyChartStr

    // Preset chart
    let presetChart = Chart.addPolyline red5ThickRedColourRoundCurve Empty
    let presetChartStr = presetChart |> toHTML
    let presetChartTest = "2. This is a chart which properies are overridden in the next tests", presetChartStr
    

    // Specifying empty set of the polyline options with setOption call
    let noPropertiesSetCurve = Polyline.setOptions (Polyline.Options()) red5ThickRedColourRoundCurve
    let noNewPropertiesSetChart = Chart.addPolyline noPropertiesSetCurve Empty
    let noNewPropertiesSetChartStr = noNewPropertiesSetChart |> toHTML
    let setNoneTest = "3. Setting empty properties list to the initial polyline (2) with:  Polyline.setOptions(Polyline.Options())", noNewPropertiesSetChartStr
    

    // Specifying polyline name and colour with setOption call
    let nameColourCurve = Polyline.setOptions (Polyline.Options(Name = "Blue", Colour = Colour.Blue)) red5ThickRedColourRoundCurve
    let nameColourCurveChart = Chart.addPolyline nameColourCurve Empty
    let nameColourCurveChartStr = nameColourCurveChart |> toHTML
    let nameColourTest = "4. Specifying name and colour of the initial polyline (2) with:  Polyline.setOptions(Polyline.Options(Name = 'Blue', Colour = Colour.Blue))", nameColourCurveChartStr


    // Specifying line cap and line join options with setOption call
    let squareMiterCurve = Polyline.setOptions (Polyline.Options(LineCap = LineCap.Square, LineJoin = LineJoin.Miter, Thickness = 20.0,  Colour = Colour.Green, Name = "Green")) red5ThickRedColourRoundCurve
    let squareMiterCurveChart = Chart.addPolyline squareMiterCurve Empty
    let squareMiterCurveChartStr = squareMiterCurveChart |> toHTML
    let squareMiterTest = "5. Overriding line cap and line join properties of the polyline (2) with:  Polyline.setOptions (Polyline.Options(LineCap = LineCap.Square, LineJoin = LineJoin.Miter, Thickness = 20.0,  Colour = Colour.Green))", squareMiterCurveChartStr


    // Specifying line cap and line join options with setOption call
    let buttBevelCurve = Polyline.setOptions (Polyline.Options(LineCap = LineCap.Butt, LineJoin = LineJoin.Bevel, Thickness = 40.0,  Colour = Colour.Blue, Name = "Blue")) red5ThickRedColourRoundCurve
    let buttBevelCurveChart = Chart.addPolyline buttBevelCurve Empty
    let buttBevelCurveChartStr = buttBevelCurveChart |> toHTML
    let buttBevelTest =
        "6. Overriding line cap and line join properties of the polyline (2) with:  Polyline.setOptions (Polyline.Options(LineCap = LineCap.Butt, LineJoin = LineJoin.Bevel, Thickness = 40.0,  Colour = Colour.Blue))", buttBevelCurveChartStr


    // Specifying full set of the polyline options with setOption call
    let green1ThickGreenColourSquareCurve = Polyline.setOptions (Polyline.Options(Name = "Green", Colour = Colour.Green, Thickness = 1.0, LineCap = LineCap.Square, LineJoin = LineJoin.Miter)) red5ThickRedColourRoundCurve
    let allNewPropertiesSetChart = Chart.addPolyline green1ThickGreenColourSquareCurve Empty
    let allNewPropertiesSetChartStr = allNewPropertiesSetChart |> toHTML
    let setAllTest = "7. Overriding all properties of the polyline (2) with:  Polyline.setOptions (Polyline.Options(Name = 'Green', Colour = Colour.Green, Thickness = 1.0, LineCap = LineCap.Square, LineJoin = LineJoin.Miter))", allNewPropertiesSetChartStr

    
    // Specifying chart title
    let titleCurve = Polyline.setOptions (Polyline.Options(Colour = Colour.Red, Name = "Red")) red5ThickRedColourRoundCurve
    let titleCurveChart = Chart.addPolyline titleCurve Empty
    let titleCurveChartStr = titleCurveChart |> Chart.setTitle "Red chart" |> toHTML
    let titleTest =
        "8. Setting the chart name by:  |> Chart.setTitle 'Red chart'", titleCurveChartStr



    let tests = 
        [
            emptyChartTest
            presetChartTest
            setNoneTest
            nameColourTest
            squareMiterTest
            buttBevelTest
            setAllTest
            titleTest
        ]    


    let generatedDiv = tests |> CollectionToHtml.toHTML 
    
    let html = template.Replace("<%PLACEHOLDER%>", generatedDiv)
    printfn "%s" generatedDiv
    let writer = File.CreateText("visualTests.html")
    writer.Write(html)
    writer.Close()
    Process.Start("visualTests.html") |> ignore

    0