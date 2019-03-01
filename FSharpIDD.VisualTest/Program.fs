module Tests

open FSharpIDD
open WebSharper
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

#if JAVASCRIPT
[<assembly: WebSharper.JavaScriptExport("FSharpIDD.WS")>]
do ()
#endif

[<JavaScript>]
let getTestText() =
    // Empty chart
    let emptyChartStr = Empty |> Chart.setSize 300 200
    let emptyChartTest =
        "Empty chart",
        [
            "|> Chart.setSize 300 200"
            "Chart has axes, grid lines. Is 300px wide, 200px tall"
        ],
        emptyChartStr
    
    let scientificNotationTest =
        "Scientific notation on numeric axis",
        [
            "Empty |> Chart.setVisibleRegion (VisibleRegion.Explicit(-4.0e5,-1e3,6.0e5,3e3))"
            "scientific notation in axis tick labels"
        ],
        Empty |> Chart.setVisibleRegion (VisibleRegion.Explicit(-4.0e5,-1e3,6.0e5,3e3))

    let scientificNotationTest2 =
        "Scientific notation on numeric axis",
        [
            "Empty |> Chart.setVisibleRegion (VisibleRegion.Explicit(-4.0e-5,-1e-3,6.0e-5,3e-3))"
            "scientific notation in axis tick labels"
        ],
        Empty |> Chart.setVisibleRegion (VisibleRegion.Explicit(-4.0e-5,-1e-3,6.0e-5,3e-3))

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
    
    let paddingAddedChart = Chart.addPolyline {blue20RoundRoundCurve with Thickness = 1.0} Empty
    let paddingAddedChartStr = paddingAddedChart |> Chart.setVisibleRegion (Autofit 20)
    let paddingAddedTest =
        "Visible padding added",
        [
            "|> Chart.setVisibleRegion (Autofit 20)"
            "The polyline bounds don't touch the chart bounds"
        ],
        paddingAddedChartStr

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

    // Tooltip delay 0
    let ChartDelay0 : Chart =
        Chart.Empty
        |> Chart.setSize 300 200
        |> Chart.setNavigationEnabled false
        |> Chart.setTooltipDelay 0.0

    let ChartDelay0Polyline = Chart.addPolyline (Polyline.setOptions (Polyline.Options(Colour = Colour.Red)) blue20RoundRoundCurve) ChartDelay0
    let tooltipDelay0Test =
        "Tooltip delay 0 seconds",
        [
            "|> Chart.setTooltipDelay 0.0"
            "A chart with a polyline. A tooltip should appear immidiately"
        ],
        ChartDelay0Polyline

    // Tooltip delay 1
    let ChartDelay1 : Chart =
        Chart.Empty
        |> Chart.setSize 300 200
        |> Chart.setNavigationEnabled false
        |> Chart.setTooltipDelay 1.0
        
    let ChartDelay1Polyline = Chart.addPolyline blue20RoundRoundCurve ChartDelay1
    let tooltipDelay1Test =
        "Tooltip delay 1 second",
        [
            "|> Chart.setTooltipDelay 1.0"
            "A chart with a polyline. A tooltip should appear after 1 second"
        ],
        ChartDelay1Polyline

    let iddIssue161 =
        let xs = Array.init 50 (fun i -> float(i))
        let means = Array.map (fun x -> x*300.0 % 3000.0) xs
        let xticks  = Array.map (fun i -> sprintf "EtOH = %1.6f" i ) xs

        Chart.Empty
        |> Chart.addMarkers (Markers.createMarkers xs means)
        |> Chart.setXaxis (Chart.createTiltedLabelledAxis xs xticks 90.0)
    
    let chartTests = 
        [
            emptyChartTest
            scientificNotationTest
            scientificNotationTest2
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
            paddingAddedTest
            explicitVisualRegionTest
            labelledAxisTest
            labelledAxisTiltedTest
            tooltipDelay0Test
            tooltipDelay1Test
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
            // issues
            ("Idd issue #161",[ "label axis must present" ], iddIssue161)
        ]

    let tests1 = List.mapi (fun i elem -> let testName, descrList, chart = elem in (sprintf "%d. %s" (i+1) testName), descrList, HTML.ofChart chart) chartTests

    let subplotsTests = 
        [
            SubplotsTests.subplots1
            SubplotsTests.setSubplotTest
            SubplotsTests.setSubplotSizeTest
            SubplotsTests.setSubplotExtLegendRightTest
            SubplotsTests.setSubplotExtLegendBottomTest
            SubplotsTests.setTitleNullTest
            SubplotsTests.setSubplotTestZeroMargin
            SubplotsTests.setSubplotTest30Margin
            SubplotsTests.setSubplotsCommonVisibilityTest
            SubplotsTests.setSubplotsCommonVisibilityExternalLegendTest
            SubplotsTests.setSubplotTestAxisBinding
            SubplotsTests.setSubplotTestAxisHorizontalBinding
        ]
    
    let tests2 = List.mapi (fun i elem -> let testName, descrList, subplots = elem in (sprintf "%d. %s" (i+1) testName), descrList, HTML.ofSubplots subplots) subplotsTests

    let tests = 
        [
            tests1;
            tests2
        ] |> List.concat

    let generatedDiv = tests |> CollectionToHtml.toHTML 
    
    generatedDiv

#if JAVASCRIPT
#else
[<EntryPoint>]
let main argv =
    let template = System.IO.File.ReadAllText "template.html"
    let generatedDiv = getTestText()
    let html = template.Replace("<%PLACEHOLDER%>", generatedDiv)
    printfn "Spawning the browser..."
    // printfn "%s" generatedDiv
    let writer = System.IO.File.CreateText("visualTests.html")
    writer.Write(html)
    writer.Close()
    System.Diagnostics.Process.Start("visualTests.html") |> ignore

    0
#endif