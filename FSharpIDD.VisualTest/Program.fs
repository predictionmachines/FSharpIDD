open System.IO
open System.Diagnostics
open FSharpIDD
open CollectionToHtml
open FSharpIDD.Plots
open FSharpIDD.Chart
open FSharpIDD.Plots.Polyline
open FSharpIDD.Plots.Markers
open FSharpIDD.Colour
open PolylineTests
open MarkersTests

[<EntryPoint>]
let main argv =

    let template = File.ReadAllText "template.html"


    // Empty chart
    let emptyChartStr = Empty |> Chart.setSize 300 200 |> toHTML
    let emptyChartTest =
        "Empty chart",
        [
            "|> Chart.setSize 300 200"
            "Chart has axes, grid lines. Is 300px wide, 200px tall"
        ],
        emptyChartStr


    // Legend enabling
    let legendEnabledChartStr = Empty |> Chart.setLegendEnabled LegendVisibility.Visible |> toHTML
    let legendEnabledTest =
        "Legend enabling",
        [
            "|> Chart.setLegendEnabled LegendVisibility.Visible"
            "Empty chart with an empty legend"
        ],
        legendEnabledChartStr


    // Legend disabling
    let legendDisabledChart = Chart.addPolyline blue20RoundRoundCurve Empty
    let legendDisabledChartStr = legendDisabledChart |> Chart.setLegendEnabled LegendVisibility.Hidden |> toHTML
    let legendDisabledTest =
        "Legend enabling",
        [
            "|> Chart.setLegendEnabled LegendVisibility.Visible"
            "Plot without a legend"
        ],
        legendDisabledChartStr


    // Navigation enabling
    let navigationEnabledChart = Chart.addMarkers basicMarkersPlot Empty
    let navigationEnabledChartStr = navigationEnabledChart |> Chart.setNavigationEnabled true |> toHTML
    let navigationEnabledTest =
        "Navigation enabling",
        [
            "|> Chart.setNavigationEnabled true"
            "Markers plot with enabled navigation"
        ],
        navigationEnabledChartStr


    let tests = 
        [
            emptyChartTest
            presetPolylineTest
            setNonePolylineTest
            nameColourTest
            thicknessTest
            squareMiterTest
            buttBevelTest
            setAllTest
            setNameSetStrokeTest
            setThicknessSetLineCapSetLineJoinTest
            titleTest
            axisTest
            basicPolylineTest
            basicMarkersTest
            emptyOptionsMarkersTest
            nameMarkersTest
            borderFillMarkersTest
            shapeMarkersTest
            sizeMarkersTest
            legendEnabledTest
            legendDisabledTest
            navigationEnabledTest
        ]

    let tests = List.mapi (fun i elem -> let testName, descrList, chartStr = elem in (sprintf "%d. %s" (i+1) testName), descrList, chartStr) tests

    let generatedDiv = tests |> CollectionToHtml.toHTML 
    
    let html = template.Replace("<%PLACEHOLDER%>", generatedDiv)
    printfn "%s" generatedDiv
    let writer = File.CreateText("visualTests.html")
    writer.Write(html)
    writer.Close()
    Process.Start("visualTests.html") |> ignore

    0