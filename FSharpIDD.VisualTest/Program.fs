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
open BarsTest
open HistogramTests
open HeatMapTests

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
    let basicPolylinePlot : Polyline.Plot = createPolyline Xseries Yseries
    let basicPolylineChart = Chart.addPolyline basicPolylinePlot Empty
    let legendEnabledChartStr = basicPolylineChart |> Chart.setLegendEnabled LegendVisibility.Visible
    let legendEnabledTest =
        "Legend enabling",
        [
            "|> Chart.setLegendEnabled LegendVisibility.Visible"
            "Polyline (no name) with its legend"
        ],
        legendEnabledChartStr


    // Legend disabling
    let blue20RoundRoundCurveChart = Chart.addPolyline blue20RoundRoundCurve Empty
    let legendDisabledChartStr = blue20RoundRoundCurveChart |> Chart.setLegendEnabled LegendVisibility.Hidden
    let legendDisabledTest =
        "Legend disabling",
        [
            "|> Chart.setLegendEnabled LegendVisibility.Hidden"
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
    let HiddenYAxisChartStr = blue20RoundRoundCurveChart |> Chart.setYaxis Axis.Hidden
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
    let gridStyledLinesChartStr = blue20RoundRoundCurveChart |> Chart.setGridLines (GridLines.Enabled(Colour.Green, 2.0))
    let gridStyledLinesTest =
        "Styling grid lines",
        [
            "|> Chart.setGridLines (GridLines.Enabled(Colour.Green, 2.0))"
            "Polyline plot with thick green grid lines"
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
    let explicitVisualRegionStr = blue20RoundRoundCurveChart |> Chart.setVisibleRegion (Explicit(-1.0,-1.0,15.0,5.0))
    let explicitVisualRegionTest =
        "Visible region explicitly set",
        [
            "Chart.setVisibleRegion (Explicit(-1.0,-1.0,15.0,5.0))"
            "Polyline is in lower-left part of the visible region"
        ],
        explicitVisualRegionStr

    // Labelled axis
    let ticks = [1.0; 3.0; 5.0; 7.0; 9.0; 2.0; 4.0; 6.0; 8.0; 10.0]
    let labels = ["a"; "b"; "c"; "d"; "e"; "f"; "g"; "h"; "i"; "j"]
    let labelledAxisStr = blue20RoundRoundCurveChart |> Chart.setXaxis (createLabelledAxis ticks labels)
    let labelledAxisTest =
        "Labelled axis: ticks and labels",
        [
            "|> Chart.setXaxis (createLabelledAxis ticks labels)"
            "Polyline with a labelled horizontal axis"
        ],
        labelledAxisStr

    // Labelled axis with tilted labels
    let ticks = [1.0; 2.0; 3.0; 4.0; 5.0; 6.0]
    let labels = ["one"; "two"; "three"; "four"; "five"]
    let labelledAxisStr = blue20RoundRoundCurveChart |> Chart.setXaxis (createTiltedLabelledAxis ticks labels 30.0)
    let labelledAxisTiltedTest =
        "Labelled axis: ticks and labels with tilted labels",
        [
            "|> Chart.setXaxis (createTiltedLabelledAxis ticks labels 30.0)"
            "Polyline with a horizontal axis, where labels are tilted"
        ],
        labelledAxisStr

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
            labelledAxisTest
            labelledAxisTiltedTest
            //bar chart
            basicBarsTest
            emptyOptionsBarsTest
            nameBarsTest
            borderFillBarsTest
            shadowBarsTest
            defaultShadowBarsTest
            withoutShadowBarsTest
            barWidthBarsTest
            //histogram
            basicHistogramChartTest
            histogramSetEmptyOptionsTest
            histogramSetBinsTest
            histogramSetColourTest
            histogramSetNameTest
            histogramSetOptionsTest
            //heatmaps
            basicGradientHeatmapTest
            basicDescreteHeatmapTest
            corrMapTest
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