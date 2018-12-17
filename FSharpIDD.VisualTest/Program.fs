open System.IO
open System.Diagnostics
open FSharpIDD
open CollectionToHtml
open FSharpIDD.Plots
open FSharpIDD.Chart
open FSharpIDD.Plots.Polyline
open FSharpIDD.Plots.Markers
open FSharpIDD.Plots.Bars
open FSharpIDD.Colour
open PolylineTests
open MarkersTests
open BarChartTest
open HistogramTests

[<EntryPoint>]
let main argv =

    let template = File.ReadAllText "template.html"


    // Empty chart
    let emptyChartStr = Empty |> Chart.setSize 300 200
    let emptyChartTest =
        "Empty chart",
        [
            "|> Chart.setSize 300 200"
            "Chart has axes, grid lines. Is 300px wide, 200px tall"
        ],
        emptyChartStr


    // Legend enabling
    let legendEnabledChartStr = Empty |> Chart.setLegendEnabled LegendVisibility.Visible
    let legendEnabledTest =
        "Legend enabling",
        [
            "|> Chart.setLegendEnabled LegendVisibility.Visible"
            "Empty chart with an empty legend"
        ],
        legendEnabledChartStr


    // Legend disabling
    let legendDisabledChart = Chart.addPolyline blue20RoundRoundCurve Empty
    let legendDisabledChartStr = legendDisabledChart |> Chart.setLegendEnabled LegendVisibility.Hidden
    let legendDisabledTest =
        "Legend enabling",
        [
            "|> Chart.setLegendEnabled LegendVisibility.Visible"
            "Plot without a legend"
        ],
        legendDisabledChartStr


    // Navigation enabling
    let navigationEnabledChart = Chart.addMarkers basicMarkersPlot Empty
    let navigationEnabledChartStr = navigationEnabledChart |> Chart.setNavigationEnabled true
    let navigationEnabledTest =
        "Navigation enabling",
        [
            "|> Chart.setNavigationEnabled true"
            "Markers plot with enabled navigation"
        ],
        navigationEnabledChartStr


    // Hidden Y axis
    let HiddenYAxisChart = Chart.addPolyline blue20RoundRoundCurve Empty
    let HiddenYAxisChartStr = HiddenYAxisChart |> Chart.setYaxis Axis.Hidden
    let HiddenYAxisTest =
        "Hidden Y axis",
        [
            "|> Chart.setYaxis Axis.Hidden"
            "Polyline plot with X axis only"
        ],
        HiddenYAxisChartStr


    // Grid lines disabled
    let gridDisabledLinesChart = Chart.addMarkers basicMarkersPlot Empty
    let gridDisabledLinesChartStr = gridDisabledLinesChart |> Chart.setGridLines GridLines.Disabled
    let gridDisabledLinesTest =
        "Hiding grid lines",
        [
            "|> Chart.setGridLines GridLines.Disabled"
            "Markers plot without grid lines"
        ],
        gridDisabledLinesChartStr


    // Styling grid lines
    let gridStyledLinesChart = Chart.addPolyline blue20RoundRoundCurve Empty
    let gridStyledLinesChartStr = gridStyledLinesChart |> Chart.setGridLines (GridLines.Enabled(Colour.Green, 2.0))
    let gridStyledLinesTest =
        "Styling grid lines",
        [
            "|> Chart.setGridLines (GridLines.Enabled(Colour.Green, 2.0))"
            "Polyline plot with green thin grid lines"
        ],
        gridStyledLinesChartStr
    
    // Removing data visual padding    
    let paddingRemovedChart = Chart.addPolyline {blue20RoundRoundCurve with Thickness = 1.0} Empty
    let paddingRemovedChartStr = paddingRemovedChart |> Chart.setVisibleRegion (Autofit 0)
    let paddingRemovedTest =
        "Visible padding removed",
        [
            "|> Chart.setVisibleRegion (Autofit 0)"
            "The polyline bounds touch the chart bounds"
        ],
        paddingRemovedChartStr

    // Explicit visual region    
    let explicitVisualRegionStr = Chart.addPolyline blue20RoundRoundCurve Empty |> Chart.setVisibleRegion (Explicit(-1.0,-1.0,15.0,5.0))
    let explicitVisualRegionTest =
        "Visible region explicitly set",
        [
            "Chart.setVisibleRegion (Explicit(-1.0,-1.0,15.0,5.0))"
            "Polyline is in lower-left part of the visible region"
        ],
        explicitVisualRegionStr

    let tests = 
        [
            emptyChartTest
            //polyline
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
            //markers
            basicMarkersTest
            emptyOptionsMarkersTest
            nameMarkersTest
            borderFillMarkersTest
            shapeMarkersTest
            sizeMarkersTest
            //other
            legendEnabledTest
            legendDisabledTest
            navigationEnabledTest
            HiddenYAxisTest
            gridDisabledLinesTest
            gridStyledLinesTest
            paddingRemovedTest
            explicitVisualRegionTest
            //bar chart
            basicBarChartTest
            emptyOptionsBarChartTest
            nameBarChartTest
            borderFillBarChartTest
            shadowBarChartTest
            defaultShadowBarChartTest
            withoutShadowBarChartTest
            barWidthBarChartTest
            //histogram
            basicHistogramChartTest
            histogramSetEmptyOptionsTest
            histogramSetBinsTest
            histogramSetColourTest
            histogramSetNameTest
            histogramSetOptionsTest
        ]

    let tests = List.mapi (fun i elem -> let testName, descrList, chartStr = elem in (sprintf "%d. %s" (i+1) testName), descrList, (chartStr |> toHTML)) tests

    let generatedDiv = tests |> CollectionToHtml.toHTML 
    
    let html = template.Replace("<%PLACEHOLDER%>", generatedDiv)
    printfn "%s" generatedDiv
    let writer = File.CreateText("visualTests.html")
    writer.Write(html)
    writer.Close()
    Process.Start("visualTests.html") |> ignore

    0