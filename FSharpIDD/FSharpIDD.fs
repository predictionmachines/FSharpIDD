namespace FSharpIDD

open WebSharper

[<AutoOpen>]
module Conversions =
    [<Inline "$0 % 256">]
    let inline byte x = byte x

module Utils =
    [<Inline "Math.floor(new Date().valueOf() * Math.random()).toString()">]
    let getUniqueId(): string = 
        System.Guid.NewGuid().ToString()
    
    [<Inline "throw \"not implemented\"">]
    let encodeFloat64SeqBase64 data =
        let bytes = data |> Seq.collect (fun (elem:float) -> System.BitConverter.GetBytes(elem)) |> Array.ofSeq
        let base64str = System.Convert.ToBase64String bytes
        base64str

    let encodeStringSeqBase64 (data:string) =
        let bytes = data |> System.Text.Encoding.ASCII.GetBytes
        let base64str = System.Convert.ToBase64String bytes
        base64str


[<JavaScript>]
module Plots =        
    type DataSeries = float seq
    
    module Polyline =
        /// The cap (shape of ending) of the line
        type LineCap =
        |   Butt
        |   Round
        |   Square

        /// The shape of joins of polyline's strait segments
        type LineJoin =
        |   Bevel
        |   Round
        |   Miter

        /// The single polyline settings
        type Plot = {
            /// Specifies how to annotate the polyline in the legend. Null means that name is not set
            Name: string
            /// Series of X coords of the points that form the polyline
            X: DataSeries
            /// Series of Y coords of the points that form the polyline
            Y: DataSeries
            /// The fill colour of the polyline
            Colour: Colour.Colour
            /// The line thickness of the polyline in pixels
            Thickness: float
            /// The cap (shape of ending) of the line
            LineCap: LineCap
            /// The shape of joins of polyline's strait segments
            LineJoin: LineJoin
            }
        
        type Options() =
            let mutable name: string option = None
            let mutable colour: Colour.Colour option = None
            let mutable thickness: float option = None
            let mutable lineCap: LineCap option = None
            let mutable lineJoin: LineJoin option = None

            member s.Name with set v = name <- Some(v)                
            member s.SpecifiedName with internal get() = name
            member s.Colour with set v = colour <- Some(v)
            member s.SpecifiedColour with internal get() = colour
            member s.Thickness with set v = thickness <- Some(v)
            member s.SpecifiedThickness with internal get() = thickness
            member s.LineCap with set v = lineCap <- Some(v)
            member s.SpecifiedLineCap with internal get() = lineCap
            member s.LineJoin with set v = lineJoin <- Some(v)
            member s.SpecifiedLineJoin with internal get() = lineJoin
    
        /// sets several polyline options at once
        let setOptions (options:Options) polyline =
            let polyline =
                match options.SpecifiedName with
                | None -> polyline
                | Some(name) -> {polyline with Name = name}
            let polyline =
                match options.SpecifiedColour with
                | None -> polyline
                | Some(color) -> {polyline with Colour = color}
            let polyline =
                match options.SpecifiedThickness with
                | None -> polyline
                | Some(thickness) -> {polyline with Thickness = thickness}
            let polyline =
                match options.SpecifiedLineCap with
                | None -> polyline
                | Some(cap) -> {polyline with LineCap = cap}
            let polyline =
                match options.SpecifiedLineJoin with
                | None -> polyline
                | Some(join) -> {polyline with LineJoin = join}
            polyline
        
        /// Creates a basic polyline using the specified set of X and Y coords
        let createPolyline x y = 
                {
                    X = x
                    Y = y
                    Colour = Colour.Default
                    Thickness = 1.0
                    Name = null
                    LineCap = LineCap.Butt
                    LineJoin = LineJoin.Miter
                }
        
        /// Changes the name of the polyline (how it is depicted in the legend)
        let setName name polyline =
            {
                polyline with
                    Name = name
            }

        /// Changes the colour of the polyline
        let setStrokeColour colour polyline =
            {
                polyline with
                    Colour = colour
            }
        
        /// Sets the line thickness in pixels
        let setThickness thickness polyline =
            {
                polyline with
                    Thickness = thickness
            }
        
        /// Sets the shape of polyline caps (line endings)
        let setLineCap capType polyline =
            {
                polyline with
                    LineCap = capType
            }

        /// Sets the shape of polyline strait segment joins
        let setLineJoin joinType polyline =
            {
                polyline with
                    LineJoin = joinType
            }

    module Markers =
        /// Marker primitives
        type Shape =
        |   Box
        |   Circle
        |   Diamond
        |   Cross
        |   Triangle

        /// The single marker plot settings
        type Plot = {
            /// Specifies how to annotate markers in a legend. Null means that name is not set
            Name: string
            /// Series of X coords of markers
            X: DataSeries
            /// Series of Y coords of markers
            Y: DataSeries
            /// Specifies the size of one marker in pixels
            Size: float
            /// The colour with which the colour will be filled
            FillColour: Colour.Colour
            /// The colour of a marker border
            BorderColour: Colour.Colour
            /// The shape of a marker
            Shape: Shape
        }
        
        type Options() =
            let mutable name: string option = None
            let mutable fillcolour: Colour.Colour option = None
            let mutable bordercolour: Colour.Colour option = None
            let mutable shape: Shape option = None
            let mutable size: float option = None

            member s.Name with set v = name <- Some(v)                
            member s.SpecifiedName with internal get() = name
            member s.FillColour with set v = fillcolour <- Some(v)
            member s.SpecifiedFillColour with internal get() = fillcolour
            member s.BorderColour with set v = bordercolour <- Some(v)
            member s.SpecifiedBorderColour with internal get() = bordercolour
            member s.Shape with set v = shape <- Some(v)
            member s.SpecifiedShape with internal get() = shape
            member s.Size with set v = size <- Some(v)
            member s.SpecifiedSize with internal get() = size
    
        /// sets several markers options at once
        let setOptions (options:Options) markers =
            let markers =
                match options.SpecifiedName with
                | None -> markers
                | Some(name) -> {markers with Name = name}
            let markers =
                match options.SpecifiedFillColour with
                | None -> markers
                | Some(fillcolour) -> {markers with FillColour = fillcolour}
            let markers =
                match options.SpecifiedBorderColour with
                | None -> markers
                | Some(bordercolour) -> {markers with BorderColour = bordercolour}
            let markers =
                match options.SpecifiedShape with
                | None -> markers
                | Some(shape) -> {markers with Shape = shape}
            let markers =
                match options.SpecifiedSize with
                | None -> markers
                | Some(size) -> {markers with Size = size}
            markers
        
        /// Creates markers plot using the specified set of X and Y coords with default settings
        let createMarkers x y = 
                {
                    X = x
                    Y = y
                    FillColour = Colour.Default
                    BorderColour = Colour.Default
                    Name = null
                    Shape = Shape.Box
                    Size = 10.0
                }
        
        /// Changes the name of markers (how are they depicted in the legend)
        let setName name markers =
            {
                markers with
                    Name = name
            }
        
        /// Changes the colour of a marker's border
        let setBorderColour bordercolour markers =
            {
                markers with
                    BorderColour = bordercolour
            }
        
        /// Changes the colour with which a marker is filled (how are they depicted in the legend)
        let setFillColour fillcolour markers =
            {
                markers with
                    FillColour = fillcolour
            }

        /// Sets the shape of a marker
        let setShape shapeType markers =
            {
                markers with
                    Shape = shapeType
            }

        /// Sets the size of a marker
        let setSize size markers =
            {
                markers with
                    Size = size
            }

    module Bars =
        type Shadow =
        |   WithoutShadow
        |   WithShadow of Colour.Colour

        /// Bars plot settings
        type Plot = {
            /// Specifies how to annotate Bars plot in a legend
            Name: string
            /// Series of bar centers horizontal coordinates. Length of the series equals the number of bars
            BarCenters: DataSeries
            /// Series of bar heights. Length of the series equals the number of bars
            BarHeights: DataSeries
            /// The width in plot coordinates of one bar in a bar chart plot
            BarWidth: float
            /// The colour with which a bar will be filled
            FillColour: Colour.Colour
            /// The colour of a bar border
            BorderColour: Colour.Colour
            /// Shadow mode: with or without shadow, shadow colour
            Shadow: Shadow
        }
        
        type Options() =
            let mutable name: string option = None
            let mutable fillcolour: Colour.Colour option = None
            let mutable bordercolour: Colour.Colour option = None
            let mutable shadow: Shadow option = None
            let mutable barwidth: float option = None

            member s.Name with set v = name <- Some(v)                
            member s.SpecifiedName with internal get() = name
            member s.FillColour with set v = fillcolour <- Some(v)
            member s.SpecifiedFillColour with internal get() = fillcolour
            member s.BorderColour with set v = bordercolour <- Some(v)
            member s.SpecifiedBorderColour with internal get() = bordercolour
            member s.Shadow with set v = shadow <- Some(v)
            member s.SpecifiedShadow with internal get() = shadow
            member s.BarWidth with set v = barwidth <- Some(v)
            member s.SpecifiedBarWidth with internal get() = barwidth
    
        /// sets several bar chart options at once
        let setOptions (options:Options) barchart =
            let barchart =
                match options.SpecifiedName with
                | None -> barchart
                | Some(name) -> {barchart with Name = name}
            let barchart =
                match options.SpecifiedFillColour with
                | None -> barchart
                | Some(fillcolour) -> {barchart with FillColour = fillcolour}
            let barchart =
                match options.SpecifiedBorderColour with
                | None -> barchart
                | Some(bordercolour) -> {barchart with BorderColour = bordercolour}
            let barchart =
                match options.SpecifiedBarWidth with
                | None -> barchart
                | Some(barwidth) -> {barchart with BarWidth = barwidth}
            let barchart =
                match options.SpecifiedShadow with
                | None -> barchart
                | Some(shadow) -> {barchart with Shadow = shadow}
            barchart
        
        /// Creates bar chart plot using the specified set of BarCenters and BarHeights with default settings
        let createBars barcenters barheights = 
                {
                    BarCenters = barcenters
                    BarHeights = barheights
                    Name = null
                    FillColour = Colour.Default
                    BorderColour = Colour.Default
                    Shadow = Shadow.WithoutShadow
                    BarWidth = 1.0
                }
        
        /// Changes a colour of bar's borders
        let setBorderColour bordercolour barchart =
            {
                barchart with
                    BorderColour = bordercolour
            }
        
        /// Changes a colour with which a bar is filled (how are they depicted in the legend)
        let setFillColour fillcolour barchart =
            {
                barchart with
                    FillColour = fillcolour
            }

        /// Sets whether a bar has a shadow and what colour is it
        let setShadow shadow barchart =
            {
                barchart with
                    Shadow = shadow
            }

        /// Sets bar width (in plot coords) of a bar
        let setBarWidth barwidth barchart =
            {
                barchart with
                    BarWidth = barwidth
            }
                
        /// Changes the name of bar chart plot (how it is depicted in the legend)
        let setName name barChart =
            {
                barChart with
                    Name = name
            }
    
    module Histogram =
        /// Histogram plot
        type Plot = {        
            /// Specifies how to annotate the histogram in a legend
            Name: string
            /// Samples to calculate the histogram for
            Samples: DataSeries        
            /// The colour of the histogram
            Colour: Colour.Colour
            /// Number of bins in the histogram
            BinCount: int
        }

        /// Creates a histogram plot for the passed data
        let createHistogram samples =
            {
                Name = null
                Samples = samples
                Colour = Colour.Default
                BinCount = 30
            }       

        let setName name hist = {hist with Name = name}
        let setColour colour hist = {hist with Colour = colour}
        let setBinCount count hist = {hist with BinCount = count}
        
        type Options() =
            let mutable name: string option = None
            let mutable colour: Colour.Colour option = None
            let mutable binCount: int option = None

            member s.Name with set v = name <- Some(v)                
            member s.SpecifiedName with internal get() = name
            member s.Colour with set v = colour <- Some(v)
            member s.SpecifiedColour with internal get() = colour
            member s.BinCount with set v = binCount <- Some(v)
            member s.SpecifiedBinCount with internal get() = binCount
    
        /// sets several histogram options at once
        let setOptions (options:Options) histogram =
            let histogram =
                match options.SpecifiedName with
                | None -> histogram
                | Some(name) -> {histogram with Name = name}
            let histogram =
                match options.SpecifiedColour with
                | None -> histogram
                | Some(colour) -> {histogram with Colour = colour}
            let histogram =
                match options.SpecifiedBinCount with
                | None -> histogram
                | Some(binCount) -> {histogram with BinCount = binCount}
            histogram
    
    module Heatmap =
        /// Heatmap plot settings
        type Plot = {
            /// Specifies how to annotate Heatmap plot in a legend
            Name: string
            /// X coords of grid points. If missing is considered to be sequential integers. Should have at least 2 elements
            X: DataSeries
            /// Y coords of grid points. If missing is considered to be sequential integers. Should have at least 2 elements
            Y: DataSeries
            /// Two-dimensional array of values in grid points. If value is NaN, the point is skipped
            Data: float[,]
            /// Colour palette
            Palette: string
            /// Heatmap transparency in [0, 1] domain, where 0 - transparent, 1 - opaque
            Opacity: float
        }

        type Options() =
            let mutable name: string option = None
            let mutable palette: string option = None
            let mutable opacity: float option = None

            member s.Name with set v = name <- Some(v)                
            member s.SpecifiedName with internal get() = name
            member s.Palette with set v = palette <- Some(v)
            member s.SpecifiedPalette with internal get() = palette
            member s.Opacity with set v = opacity <- Some(v)
            member s.SpecifiedOpacity with internal get() = opacity
    
        /// sets several heatmap options at once
        let setOptions (options:Options) heatmap =
            let heatmap =
                match options.SpecifiedName with
                | None -> heatmap
                | Some(name) -> {heatmap with Name = name}
            let heatmap =
                match options.SpecifiedPalette with
                | None -> heatmap
                | Some(palette) -> {heatmap with Palette = palette}
            let heatmap =
                match options.SpecifiedOpacity with
                | None -> heatmap
                | Some(opacity) -> {heatmap with Opacity = opacity}
            heatmap
        
        /// Creates a basic heatmap using the specified set of X, Y coords and data array
        let createHeatmap x y data = 
                {
                    Name = null
                    X = x
                    Y = y
                    Data = data
                    Palette = null
                    Opacity = 1.0
                }
        
        /// Changes name of the heatmap (how it is depicted in the legend)
        let setName name heatmap =
            {
                heatmap with
                    Name = name
            }
        
        /// Changes palette of the heatmap
        let setPalette palette heatmap =
            {
                heatmap with
                    Palette = palette
            }
        
        /// Changes opacity of the heatmap
        let setOpacity opacity heatmap =
            {
                heatmap with
                    Opacity = opacity
            }

    type Plot =
    |   Polyline of Polyline.Plot
    |   Markers of Markers.Plot
    |   Bars of Bars.Plot
    |   Histogram of Histogram.Plot

    type SizeType = int

open Plots.Bars

[<JavaScript>]
module Chart =        
    open Plots

    type Axis = 
    /// The axis is disabled (not visible)
    |   Hidden
    /// Numeric axis of automatically calculated and placed numerical ticks with values
    |   Numeric
    /// Labelled axis. Uses array with string labels(lables[]) and array of numerical values (ticks[]), where these labels will be placed
    |   Labelled of ticks: float seq * labels: string seq

    type GridLines =
    |   Disabled
    |   Enabled of strokeColour: Colour.Colour * lineWidthPX: float

    let DefaultGridLines = Enabled(Colour.DarkGrey, 1.0)

    type LegendVisibility =
    /// The legend is always visible (even if all of the plots are without names)
    |   Visible
    /// The legend is visible if some of the plots have their name set
    |   Automatic
    /// The legend is not visible
    |   Hidden

    type VisibleRegion =
    /// Fits the visible region so that all of the data is visible adding additional padding (blank visible area) in pixels
    |   Autofit of dataPaddingPx:int
    /// Explicit visible region in data coordinates
    |   Explicit of xmin:float * ymin:float * xmax:float * ymax:float

    /// Represents single chart that can be transformed later into the HTML IDD Chart    
    type Chart = {
        /// The width of the chart in pixels
        Width: int 
        /// The height of the chart in pixels
        Height: int
        /// The text that is centered and placed above the chart
        Title: string
        /// The text which describes the X axis
        Xlabel: string
        /// Which X axis to draw
        Xaxis: Axis
        /// The text which describes the Y axis        
        Ylabel: string
        /// Which Y axis to draw
        Yaxis: Axis
        /// The appearance of grid lines
        GridLines : GridLines
        /// A collection of plots (polyline, markers, bar charts, etc) to draw
        Plots: Plot list
        /// Whether the legend (list of plot names and their icons) is visible in the top-right part of the chart
        IsLegendEnabled: LegendVisibility
        /// Whether the chart visible area can be navigated with a mouse or touch gestures
        IsNavigationEnabled: bool
        /// Which visible rectangle is displayed by the chart
        VisibleRegion : VisibleRegion
    }

    let Empty : Chart = {
        Width = 800
        Height = 600
        Title = null // null means not set
        Xlabel = null // null means not set
        Ylabel = null // null means not set
        Xaxis = Axis.Numeric
        Yaxis = Axis.Numeric
        GridLines = DefaultGridLines
        IsLegendEnabled = Automatic
        IsNavigationEnabled = true
        Plots = []
        VisibleRegion = VisibleRegion.Autofit 20
    }

    let addPolyline polyline chart = { chart with Plots = Polyline(polyline)::chart.Plots }

    let addMarkers markers chart = { chart with Plots = Markers(markers)::chart.Plots }

    let addBars bars chart = { chart with Plots = Bars(bars)::chart.Plots }

    let addHistogram histogram chart = { chart with Plots = Histogram(histogram)::chart.Plots }

    /// Sets the textual title that will be placed above the charting area
    let setTitle title chart =  { chart with Title = title}

    /// Sets the size of the chart in pixels
    let setSize width height chart = { chart with Width = width; Height = height}

    /// Sets the X axis textual  label (placed below the X axis)
    let setXlabel label chart = { chart with Xlabel = label}

    /// Sets the mode of X axis
    let setXaxis axisMode chart = {chart with Xaxis = axisMode}

    /// Sets the visible region that is displayed by the chart
    let setVisibleRegion region chart = { chart with VisibleRegion = region}

    /// Set the Y axis textual label (placed to the left of Y axis)
    let setYlabel label chart = { chart with Ylabel = label}

    /// Sets the mode of Y axis
    let setYaxis axisMode chart = { chart with Yaxis = axisMode}

    /// Set the mode of grid lines appearance
    let setGridLines gridLines chart = {chart with GridLines = gridLines}
    
    /// Set the visibility of the plot legend floating in the top-right region of the chart
    let setLegendEnabled legendVisibility chart = {chart with IsLegendEnabled = legendVisibility}

    /// Set whether the chart can be navigated with a mouse or touch gestures
    let setNavigationEnabled isEnabled chart = {chart with IsNavigationEnabled = isEnabled}

    open Html

    let toHTML (chart:Chart) =
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
                
        let chartNode = 
            if chart.Ylabel <> null then
                let containerNode =
                    let labelNode =
                        createDiv()
                        |> addAttribute "class" "idd-verticalTitle-inner"
                        |> addText chart.Ylabel
                    createDiv()
                    |> addAttribute "class" "idd-verticalTitle"
                    |> addAttribute "data-idd-placement" "left"
                    |> addDiv labelNode                                        
                chartNode |> addDiv containerNode
            else
                chartNode
            

        let getDataDomWithTicksLabels ticks labels =
                // can't use string builder here as it is not transpilable with WebSharper
                let str = Seq.fold2 (fun state tick label -> state + (sprintf "\t%f\t%s\n" tick label)) "ticks\tlabels\n" ticks labels
                str

        let chartNode,yAxisID = 
            match chart.Yaxis with
            |   Axis.Hidden -> chartNode, Option.None
            |   Axis.Numeric ->
                let id = Utils.getUniqueId()
                let axisNode =                    
                    createDiv()
                    |> addAttribute "id" id
                    |> addAttribute "data-idd-axis" "numeric"
                    |> addAttribute "data-idd-placement" "left"
                    |> addAttribute "style" "position: relative;"                    
                (chartNode |> addDiv axisNode),(Some id)
            |   Axis.Labelled (ticks, labels) ->
                let id = Utils.getUniqueId()
                let axisNode =                    
                    createDiv()
                    |> addAttribute "id" id
                    |> addAttribute "data-idd-axis" "labels"
                    |> addAttribute "data-idd-placement" "left"
                    |> addAttribute "style" "position: relative;"
                    |> addText (getDataDomWithTicksLabels ticks labels)
                (chartNode |> addDiv axisNode),(Some id)
        
        let chartNode = 
            if chart.Xlabel <> null then
                let labelNode =
                    createDiv()
                    |> addAttribute "class" "idd-horizontalTitle"
                    |> addAttribute "data-idd-placement" "bottom"
                    |> addText chart.Xlabel
                chartNode |> addDiv labelNode
            else
                chartNode

        let chartNode,xAxisID = 
            match chart.Xaxis with
            |   Axis.Hidden -> chartNode, Option.None
            |   Axis.Numeric ->
                let id = Utils.getUniqueId()
                let axisNode =                    
                    createDiv()
                    |> addAttribute "id" id
                    |> addAttribute "data-idd-axis" "numeric"
                    |> addAttribute "data-idd-placement" "bottom"
                    |> addAttribute "style" "position: relative;"                    
                (chartNode |> addDiv axisNode),(Some id)  
            |   Axis.Labelled (ticks, labels)->
                let id = Utils.getUniqueId()
                let axisNode =                    
                    createDiv()
                    |> addAttribute "id" id
                    |> addAttribute "data-idd-axis" "labels"
                    |> addAttribute "data-idd-placement" "bottom"
                    |> addAttribute "style" "position: relative;"
                    |> addText (getDataDomWithTicksLabels ticks labels)
                (chartNode |> addDiv axisNode),(Some id)             
        
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

        let getXYDataDom xSeries ySeries =                
                sprintf "x float64.1D %s\ny float64.1D %s" (Utils.encodeFloat64SeqBase64 xSeries) (Utils.encodeFloat64SeqBase64 ySeries)

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
                <| Utils.encodeFloat64SeqBase64 xSeries
                <| Utils.encodeFloat64SeqBase64 ySeries
                <| innerDimLen
                <| Utils.encodeFloat64SeqBase64 flattenedData

        let histogramToBars (h:Histogram.Plot) =
            let hist = Histogram.buildHistogram h.Samples h.BinCount
            let bars: Bars.Plot = 
                {
                    Name = h.Name
                    BarCenters = hist.BinCentres
                    BarHeights = hist.BinCounters |> Seq.map float
                    BarWidth = hist.BinWidth
                    FillColour = h.Colour
                    BorderColour = h.Colour
                    Shadow = Shadow.WithoutShadow
                }
            bars
            

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
    
        let histogranToDiv (h: Histogram.Plot) =
            histogramToBars h |> barchartToDiv

        let plotToDiv plot =
            match plot with
            |   Polyline p -> polylineToDiv p
            |   Markers m -> markersToDiv m
            |   Bars b -> barchartToDiv b
            |   Histogram h -> histogranToDiv h

        let plotElems = chart.Plots |> Seq.map plotToDiv
        let chartNode = Seq.fold (fun state elem -> addDiv elem state) chartNode plotElems
        
        let chartNode =
            match chart.GridLines with
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
                chartNode |> addDiv gridNode
            |   GridLines.Disabled -> chartNode

        divToStr chartNode

    
