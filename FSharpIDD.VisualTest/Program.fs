open System.IO
open System.Diagnostics
open FSharpIDD
open CollectionToHtml
open FSharpIDD.Plot
open FSharpIDD.Chart

[<EntryPoint>]
let main argv =

    let template = File.ReadAllText "template.html"

    let Xseries = Array.init 3 (fun i -> float(i+1))
    let Yseries1 = Xseries |> Array.map (fun x -> sin(2.0*x)/x)
    
    let redColour2ThickRedNameCurve : Polyline.Plot =
        {
            X = Xseries
            Y = Yseries1
            Name = "Red"
            Colour = Colour.Red
            Thickness = 2.0
        }



    // Empty chart
    let emptyChartStr = Chart.Empty |> toHTML
    let emptyChartTest = "1. Empty chart", emptyChartStr

    // Preset chart
    let presetChart = Chart.addPolyline redColour2ThickRedNameCurve Chart.Empty
    let presetChartStr = presetChart |> toHTML
    let presetChartTest = "2. This is a chart which properies are overridden in the next tests", presetChartStr
    

    // Specifying empty set of the polyline options with setOption call
    let noPropertiesSetCurve = Polyline.setOptions (Polyline.Options()) redColour2ThickRedNameCurve
    let noNewPropertiesSetChart = Chart.addPolyline noPropertiesSetCurve Chart.Empty
    let noNewPropertiesSetChartStr = noNewPropertiesSetChart |> toHTML
    let setNoneTest = "3. Setting empty properties list to the initial polyline (2) with: Polyline.setOptions (Polyline.Options())", noNewPropertiesSetChartStr


    // Specifying full set of the polyline options with setOption call
    let greenColour6ThickGreenNameCurve = Polyline.setOptions (Polyline.Options(Name = "Green", Colour = Colour.Green, Thickness = 6.0)) redColour2ThickRedNameCurve
    let allNewPropertiesSetChart = Chart.addPolyline greenColour6ThickGreenNameCurve Chart.Empty
    let allNewPropertiesSetChartStr = allNewPropertiesSetChart |> toHTML
    let setAllTest = "4. Overriding all properties of the polyline (2) with: Polyline.setOptions (Polyline.Options(Name = 'Green', Colour = Colour.Green, Thickness = 6.0))", allNewPropertiesSetChartStr

    //let setStrokeTest = "Set stroke", Chart.Empty
    //let setNameTest = "Set polyline name", Chart.Empty
    //let setThicknessTest = "Set polyline thickness", Chart.Empty
    

    let tests = 
        [
            emptyChartTest
            presetChartTest
            setNoneTest
            setAllTest
        ]    


    let generatedDiv = tests |> CollectionToHtml.toHTML 
    
    let html = template.Replace("<%PLACEHOLDER%>", generatedDiv)
    printfn "%s" generatedDiv
    let writer = File.CreateText("visualTests.html")
    writer.Write(html)
    writer.Close()
    Process.Start("visualTests.html") |> ignore

    0