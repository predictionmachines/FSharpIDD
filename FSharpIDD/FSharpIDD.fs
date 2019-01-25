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
            
    [<Inline "btoa((new Uint8Array((Float64Array.from($0)).buffer)).reduce(function (data, byte) { return data + String.fromCharCode(byte)}, ''))">]
    let encodeFloat64ArrayBase64 (data:float array) =
        let bytes = data |> Seq.collect (fun (elem:float) -> System.BitConverter.GetBytes(elem)) |> Array.ofSeq
        let base64str = System.Convert.ToBase64String bytes
        base64str

    [<Inline "throw \"not NotImplementedException\"">]
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
        type Palette =
        |   IddPaletteString of string
        |   Default
        /// Heatmap plot settings
        type Plot = {
            /// Specifies how to annotate Heatmap plot in a legend
            Name: string
            /// X coords of grid points. If missing is considered to be sequential integers. Should have at least 2 elements
            X: float[]
            /// Y coords of grid points. If missing is considered to be sequential integers. Should have at least 2 elements
            Y: float[]
            /// Two-dimensional array of values in grid points. If value is NaN, the point is skipped
            Data: float[,]
            /// Colour palette
            Palette: Palette
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
                | Some(palette) -> {heatmap with Palette = IddPaletteString palette}
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
                    Palette = Palette.Default
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
    |   Heatmap of Heatmap.Plot

    type SizeType = int    

[<JavaScript>]
module Chart =        
    open Plots

    type LabelledAxisRecord = {
        Ticks: float seq
        Labels: string seq
        Angle: float
    }

    /// Sets ticks and labels of a labelled axis
    let setTicksLabels ticks labels labelledAxis =  { labelledAxis with Ticks = ticks; Labels = labels }

    /// Sets tilt angle of labels on a labelled axis
    let setLabelsAngle angle labelledAxis =  { labelledAxis with Angle = angle }

    type Axis = 
    /// The axis is disabled (not visible)
    |   Hidden
    /// Numeric axis of automatically calculated and placed numerical ticks with values
    |   Numeric of ScientificNotationEnabled:bool
    /// Labelled axis. Uses array with string labels(lables[]) and array of numerical values (ticks[]), where these labels will be placed. Also has an angle parameter
    |   Labelled of LabelledAxisRecord
    
    /// Creates a labelled axis using the specified ticks and labels arrays, tilt angle
    let createTiltedLabelledAxis ticks labels angle = 
        Labelled {
            Ticks = ticks
            Labels = labels
            Angle = angle
        }
    
    /// Creates a labelled axis using the specified ticks and labels arrays
    let createLabelledAxis ticks labels = 
        Labelled {
            Ticks = ticks
            Labels = labels
            Angle = 0.0
        }
    

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
        /// Whether the plot coordinates of the point under the mouse are shown in the tooltip
        IsTooltipPlotCoordsEnabled: bool
        /// Which visible rectangle is displayed by the chart
        VisibleRegion : VisibleRegion
    }

    let Empty : Chart = {
        Width = 800
        Height = 600
        Title = null // null means not set
        Xlabel = null // null means not set
        Ylabel = null // null means not set
        Xaxis = Axis.Numeric true
        Yaxis = Axis.Numeric true
        GridLines = DefaultGridLines
        IsLegendEnabled = Automatic
        IsNavigationEnabled = true
        Plots = []
        VisibleRegion = VisibleRegion.Autofit 20
        IsTooltipPlotCoordsEnabled = true
    }

    let addPolyline polyline chart = { chart with Plots = Polyline(polyline)::chart.Plots }

    let addMarkers markers chart = { chart with Plots = Markers(markers)::chart.Plots }

    let addBars bars chart = { chart with Plots = Bars(bars)::chart.Plots }

    let addHistogram histogram chart = { chart with Plots = Histogram(histogram)::chart.Plots }

    let addHeatmap heatmap chart = { chart with Plots = Heatmap(heatmap)::chart.Plots }

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
