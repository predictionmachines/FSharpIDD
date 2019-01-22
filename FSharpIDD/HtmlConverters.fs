namespace FSharpIDD
open WebSharper

[<JavaScript>]
type Placement =
    |   Bottom
    |   Top
    |   Left
    |   Right

namespace FSharpIDD

open FSharpIDD.Plots
open FSharpIDD.Chart
open DOM
open WebSharper

[<JavaScript>]
module internal HtmlConverters =
    let axisPlacementToStr placement =
            match placement with
                |   Top -> "top"
                |   Bottom -> "bottom"
                |   Left -> "left"
                |   Right -> "right"

    let axisToHtmlStructure axis placement =
        let getDataDomWithTicksLabels ticks labels =
                // can't use string builder here as it is not transpilable with WebSharper                
                let ticks_array = Array.ofSeq ticks
                let labels_array = Array.ofSeq labels
                let labels_seq_final =
                    if((Array.length ticks_array) = (Array.length labels_array))
                    then
                        labels
                    else
                        Seq.append labels [""]
                let str = Seq.fold2 (fun state tick label -> state + (sprintf "\t%f\t%s\n" tick label)) "ticks\tlabels\n" ticks labels_seq_final
                str        
        match axis with
            |   Axis.Hidden -> Option.None
            |   Axis.Numeric(scientificNotationEnabled) ->
                let id = Utils.getUniqueId()
                let axisNode =                    
                    createDiv()
                    |> addAttribute "id" id
                    |> addAttribute "data-idd-axis" "numeric"
                    |> addAttribute "data-idd-placement" (axisPlacementToStr placement)
                    |> addAttribute "style" "position: relative;"
                let axisNode = if scientificNotationEnabled then addAttribute "data-idd-scientific-notation" "true" axisNode else axisNode
                
                Some (axisNode, id)
            |   Axis.Labelled labelledAxisRecord ->
                let (ticks, labels, angle) = labelledAxisRecord.Ticks, labelledAxisRecord.Labels, labelledAxisRecord.Angle
                let id = Utils.getUniqueId()
                let tiltString = 
                    if (labelledAxisRecord.Angle = 0.0)
                    then ""
                    else "rotate: true; rotateAngle: " + (sprintf "%f;" labelledAxisRecord.Angle)
                let axisNode =                    
                    createDiv()
                    |> addAttribute "id" id
                    |> addAttribute "data-idd-axis" "labels"
                    |> addAttribute "data-idd-placement" (axisPlacementToStr placement)
                    |> addAttribute "style" "position: relative;"
                    |> addAttribute "data-idd-style" tiltString
                    |> addText (getDataDomWithTicksLabels ticks labels)
                let axisNode = 
                    if (labelledAxisRecord.Angle = 0.0)
                    then axisNode
                    else (axisNode |> addAttribute "data-idd-style" ("rotate: true; rotateAngle: " + (sprintf "%f;" labelledAxisRecord.Angle)))
                Some(axisNode,id)

    let vertAxisLabelToHtmlStructue label placement parent =
        let placementStr = 
            match placement with
            |   Left | Right -> axisPlacementToStr placement
            |   Bottom | Top -> failwith "wrong placement"
        let containerNode =
            let labelNode =
                createDiv()
                |> addAttribute "class" "idd-verticalTitle-inner"
                |> addText label
            createDiv()
            |> addAttribute "class" "idd-verticalTitle"
            |> addAttribute "data-idd-placement" placementStr
            |> addDiv labelNode                                        
        parent |> addDiv containerNode
    
    let horAxisLabelToHtmlStructure label placement parent =
        let placementStr = 
            match placement with
            |   Left | Right -> failwith "wrong placement"
            |   Bottom | Top -> axisPlacementToStr placement
        let labelNode =
            createDiv()
                |> addAttribute "class" "idd-horizontalTitle"
                |> addAttribute "data-idd-placement" placementStr
                |> addText label
        parent |> addDiv labelNode

    let getXYDataDom xSeries ySeries =                
        sprintf "x float64.1D %s\ny float64.1D %s" (Utils.encodeFloat64ArrayBase64 (Array.ofSeq xSeries)) (Utils.encodeFloat64ArrayBase64 (Array.ofSeq ySeries))

    let polylineToDiv (p:Polyline.Plot) =                                    
        let styleEntries = [ sprintf "thickness: %.1f" p.Thickness ]
        let styleEntries = 
            match p.Colour with
            | Colour.Rgb c -> (sprintf "stroke: rgb(%d,%d,%d)" c.R c.G c.B)::styleEntries
            | Colour.Default -> styleEntries                     
        let styleEntries = 
            let joinStr =
                match p.LineJoin with
                | Polyline.LineJoin.Miter -> "miter"
                | Polyline.LineJoin.Bevel -> "bevel"
                | Polyline.LineJoin.Round -> "round"
            (sprintf "lineJoin: %s" joinStr)::styleEntries
        let styleEntries = 
            let capStr =
                match p.LineCap with
                | Polyline.LineCap.Butt -> "butt"
                | Polyline.LineCap.Round -> "round"
                | Polyline.LineCap.Square -> "square"
            (sprintf "lineCap: %s" capStr)::styleEntries
        let styleValue = System.String.Join("; ",styleEntries)

        let resultNode =
            createDiv()
            |> addAttribute "data-idd-plot" "polyline"
            |> addAttribute "data-idd-style" styleValue
            |> addAttribute "data-idd-datasource" "InteractiveDataDisplay.readBase64"
            |> addText (getXYDataDom p.X p.Y)
                        
        let resultNode =
            if System.String.IsNullOrEmpty p.Name then
                resultNode
            else
                resultNode |> addAttribute "data-idd-name" p.Name  

        resultNode

    let markersToDiv (m: Markers.Plot) =          
        // A number is a size in pixels
        let styleEntries = [ sprintf "size: %.1f" m.Size ]

        let styleEntries = 
            match m.FillColour with
            | Colour.Rgb c -> (sprintf "color: rgb(%d,%d,%d)" c.R c.G c.B)::styleEntries
            | Colour.Default -> styleEntries

        let styleEntries = 
            match m.BorderColour with
            | Colour.Rgb c -> (sprintf "border: rgb(%d,%d,%d)" c.R c.G c.B)::styleEntries
            | Colour.Default -> styleEntries
                
        let styleEntries = 
            let shapeStr =
                match m.Shape with
                | Markers.Shape.Box -> "box"
                | Markers.Shape.Circle -> "circle"
                | Markers.Shape.Cross -> "cross"
                | Markers.Shape.Diamond -> "diamond"
                | Markers.Shape.Triangle -> "triangle"
            (sprintf "shape: %s" shapeStr)::styleEntries

        let styleValue = System.String.Join("; ",styleEntries)

        let resultNode =
            createDiv()
            |> addAttribute "data-idd-plot" "markers"
            |> addAttribute "data-idd-style" styleValue
            |> addAttribute "data-idd-datasource" "InteractiveDataDisplay.readBase64"
            |> addText (getXYDataDom m.X m.Y)
                        
        let resultNode =
            if System.String.IsNullOrEmpty m.Name then
                resultNode
            else
                resultNode |> addAttribute "data-idd-name" m.Name  

        resultNode

    let barchartToDiv (b: Bars.Plot) =                                
        // A number is a size in plot coords
        let styleEntries = [ sprintf "barWidth: %f" b.BarWidth ]

        let styleEntries = 
            match b.FillColour with
            | Colour.Rgb c -> (sprintf "color: rgb(%d,%d,%d)" c.R c.G c.B)::styleEntries
            | Colour.Default -> styleEntries

        let styleEntries = 
            match b.BorderColour with
            | Colour.Rgb c -> (sprintf "border: rgb(%d,%d,%d)" c.R c.G c.B)::styleEntries
            | Colour.Default -> styleEntries

        let styleEntries = 
            match b.Shadow with
            | Bars.Shadow.WithShadow c ->
                match c with
                |   Colour.Rgb c -> (sprintf "shadow: rgb(%d,%d,%d)" c.R c.G c.B)::styleEntries
                |   Colour.Default -> "shadow: grey"::styleEntries
            | Bars.Shadow.WithoutShadow -> styleEntries
                
        let styleEntries = (sprintf "shape: bars")::styleEntries

        let styleValue = System.String.Join("; ",styleEntries)

        let resultNode =
            createDiv()
            |> addAttribute "data-idd-plot" "markers"
            |> addAttribute "data-idd-style" styleValue
            |> addAttribute "data-idd-datasource" "InteractiveDataDisplay.readBase64"
            |> addText (getXYDataDom b.BarCenters b.BarHeights)
                        
        let resultNode =
            if System.String.IsNullOrEmpty b.Name then
                resultNode
            else
                resultNode |> addAttribute "data-idd-name" b.Name  

        resultNode
    
    let histogramToDiv (h: Histogram.Plot) =    
        let histogramToBars (h:Histogram.Plot) =
            let hist = HistogramBuilder.buildHistogram h.Samples h.BinCount            
            let bars: Bars.Plot = 
                {
                    Name = h.Name
                    BarCenters = hist.BinCentres |> Array.ofList
                    BarHeights = hist.BinCounters |> Seq.map float |> Array.ofSeq // leaving Seq type here causes WebSharper to produce empty data code
                    BarWidth = hist.BinWidth
                    FillColour = h.Colour
                    BorderColour = h.Colour
                    Shadow = Bars.Shadow.WithoutShadow
                }
            bars            

        let histMarkers = histogramToBars h            
        let div = barchartToDiv histMarkers            
        div
        
    let heatmapToDiv (hm: Heatmap.Plot) =
        let getHeatMapDataDom (xSeries:float array) (ySeries: float array) (valArray: float [,]) =
            let outerDimLen,innerDimLen = Array2D.length1 valArray, Array2D.length2 valArray            
            let flattenedData = 
                Array.ofSeq <|                
                seq {
                    for i in 0 .. (outerDimLen-1) do 
                        for j in 0 .. (innerDimLen-1) do
                            yield valArray.[i,j]
                }
                
            sprintf "x float64.1D %s\ny float64.1D %s\nvalues float64.2D %i %s"
                <| Utils.encodeFloat64ArrayBase64 (Array.ofSeq xSeries)
                <| Utils.encodeFloat64ArrayBase64 (Array.ofSeq ySeries)
                <| innerDimLen
                <| Utils.encodeFloat64ArrayBase64 (Array.ofSeq flattenedData)

        let styleEntries = [ sprintf "opacity: %f" hm.Opacity ]
        let styleEntries =                 
            match hm.Palette with
            | Heatmap.Palette.IddPaletteString(definition) -> (sprintf "colorPalette: %s" definition)::styleEntries
            | Heatmap.Palette.Default -> styleEntries
                
        let styleValue = System.String.Join("; ", styleEntries)

        let resultNode =
            createDiv()
            |> addAttribute "data-idd-plot" "heatmap"
            |> addAttribute "data-idd-style" styleValue
            |> addAttribute "data-idd-datasource" "InteractiveDataDisplay.readBase64"
            |> addText (getHeatMapDataDom hm.X hm.Y hm.Data)
                        
        let resultNode =
            if System.String.IsNullOrEmpty hm.Name then
                resultNode
            else
                resultNode |> addAttribute "data-idd-name" hm.Name  

        resultNode

    let plotToDiv plot =
        match plot with
        |   Polyline p -> polylineToDiv p
        |   Markers m -> markersToDiv m
        |   Bars b -> barchartToDiv b
        |   Histogram h -> histogramToDiv h
        |   Heatmap hm -> heatmapToDiv hm

    let gridLinesToHtmlStructure gridlines xAxisID yAxisID parent =
        match gridlines with
            |   GridLines.Enabled(colour,thickness) ->                
                let gridNode =
                    let styleEntries = [ sprintf "thickness: %.1fpx" thickness ]
                    let styleEntries = 
                        match colour with
                        | Colour.Rgb c -> (sprintf "stroke: rgb(%d,%d,%d)" c.R c.G c.B)::styleEntries
                        | Colour.Default -> styleEntries 
                    let styleValue = System.String.Join("; ",styleEntries)
                    createDiv()
                    |> addAttribute "data-idd-plot" "grid"
                    |> addAttribute "data-idd-placement" "center"
                    |> addAttribute "data-idd-style" styleValue
                let gridNode = 
                    match xAxisID with
                    |   Some xId -> gridNode |> addAttribute "data-idd-xaxis" xId
                    |   Option.None -> gridNode
                let gridNode = 
                    match yAxisID with
                    |   Some yId -> gridNode |> addAttribute "data-idd-yaxis" yId
                    |   Option.None -> gridNode
                parent |> addDiv gridNode
            |   GridLines.Disabled -> parent

    let toHtmlStructure (chart:Chart) =
        let chartNode =
            let addVisibleRegionAttribute = 
                match chart.VisibleRegion with
                |   Autofit padding -> addAttribute "data-idd-padding" (sprintf "%d" padding)
                |   Explicit(xmin,ymin,xmax,ymax) -> addAttribute "data-idd-visible-region" (sprintf "%f %f %f %f" xmin xmax ymin ymax)
            createDiv()
            |> addAttribute "class" "fsharp-idd" 
            |> addAttribute "data-idd-plot" "figure" 
            |> addAttribute "style" (sprintf "width: %dpx; height: %dpx;" chart.Width chart.Height)
            |> addVisibleRegionAttribute        
        
        let yAxis = axisToHtmlStructure chart.Yaxis Left
        let chartNode, yAxisID = 
            match yAxis with
            |   Some(axis,id) -> DOM.addDiv axis chartNode, Some id
            |   None -> chartNode, None
        
        let chartNode = 
            if chart.Ylabel <> null then
                vertAxisLabelToHtmlStructue chart.Ylabel Left chartNode
            else
                chartNode
        
        let xAxis = axisToHtmlStructure chart.Xaxis Bottom
        let chartNode,xAxisID = 
            match xAxis with
            |   Some(axis,id) -> DOM.addDiv axis chartNode, Some id
            |   None -> chartNode, None

        let chartNode = 
            if chart.Xlabel <> null then
                horAxisLabelToHtmlStructure chart.Xlabel Bottom chartNode
            else
                chartNode
        
        let effectiveLegendvisibility =
            match chart.IsLegendEnabled with
            |   Visible -> true
            |   Hidden -> false
            |   Automatic ->
                let isNameDefined plot =
                    match plot with
                    |   Polyline p -> p.Name <> null
                    |   Markers m -> m.Name <> null
                    |   Bars b -> b.Name <> null
                    |   Histogram h -> h.Name <> null
                    |   Heatmap hm -> hm.Name <> null
                List.exists isNameDefined chart.Plots

        let chartNode = chartNode |> addAttribute "data-idd-legend-enabled" (if effectiveLegendvisibility then "true" else "false")            
        let chartNode = chartNode |> addAttribute "data-idd-navigation-enabled" (if chart.IsNavigationEnabled then "true" else "false")            

        let chartNode = 
            if chart.Title <> null then
                let titleNode =
                    createDiv()
                    |> addAttribute "class" "idd-title"
                    |> addAttribute "data-idd-placement" "top"
                    |> addText chart.Title
                chartNode |> addDiv titleNode
            else
                chartNode
        
        let chartNode =
            if chart.IsTooltipPlotCoordsEnabled then
                chartNode
            else
                chartNode |> addAttribute "data-idd-suppress-tooltip-coords" "true"
        
        let chartNode =
            gridLinesToHtmlStructure chart.GridLines xAxisID yAxisID chartNode

        let plotElems = chart.Plots |> Seq.map plotToDiv
        let chartNode = Seq.fold (fun state elem -> addDiv elem state) chartNode plotElems
        
        Div chartNode