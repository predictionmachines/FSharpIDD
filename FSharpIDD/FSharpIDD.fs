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


/// This module is to reduce the dependencies count and to ease possible WebSharper compilation
[<JavaScript>]
module Colour =    
    type RgbColour = {
        R: byte
        G: byte
        B: byte
    }

    type Colour = 
    | Default
    | Rgb of RgbColour

    let createColour red green blue = Rgb {R=red; G=green;B=blue}

    let Red = createColour (byte 255) (byte 0) (byte 0)
    let Green = createColour (byte 0) (byte 255) (byte 0)
    let Blue = createColour (byte 0) (byte 0) (byte 255)
    let DarkGrey = createColour (byte 0xA9) (byte 0xA9) (byte 0xA9)

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
            /// Specifies how to annotate markers in the legend. Null means that name is not set
            Name: string
            /// Series of X coords of the markers
            X: DataSeries
            /// Series of Y coords of the markers
            Y: DataSeries
            /// Specifies the size of one marker in pixels
            Size: float
            /// The markers fill colour
            FillColour: Colour.Colour
            /// The colour of the markers border
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
        
        /// Changes the colour of a marker's border (how are they depicted in the legend)
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

    type Plot =
    |   Polyline of Polyline.Plot
    |   Markers of Markers.Plot

    type SizeType = int

[<JavaScript>]
module Chart =        
    open Plots    
    open System

    type Axis = 
    /// The axis is disabled (not visible)
    |   Hidden
    /// Numeric axis of automatically calculated and placed numerical ticks with values
    |   Numeric

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
        /// A collection of plots (polyline, markers, etc) to draw
        Plots: Plot list
        /// Whether the legend (list of plot names and their icons) is visible in the top-right part of the chart
        IsLegendEnabled: LegendVisibility
        /// Whether the chart visible area can be navigated with a mouse or touch gestures
        IsNavigationEnabled: bool
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
    }

    let addPolyline polyline chart = { chart with Plots = Polyline(polyline)::chart.Plots }

    let addMarkers markers chart = { chart with Plots = Markers(markers)::chart.Plots }

    /// Sets the textual title that will be placed above the charting area
    let setTitle title chart =  { chart with Title = title}

    /// Sets the size of the chart in pixels
    let setSize width height chart = { chart with Width = width; Height = height}

    /// Sets the X axis textual  label (placed below the X axis)
    let setXlabel label chart = { chart with Xlabel = label}

    /// Set the Y axis textual label (placed to the left of Y axis)
    let setYlabel label chart = { chart with Ylabel = label}

    /// Set the mode of grid lines appearance
    let setGridLines gridLines chart = {chart with GridLines = gridLines}
    
    /// Set the visibility of the plot legend floating in the top-right region of the chart
    let setLegendEnabled legendVisibility chart = {chart with IsLegendEnabled = legendVisibility}

    /// Set whether the chart can be navigated with a mouse or touch gestures
    let setNavigationEnabled isEnabled chart = {chart with IsNavigationEnabled = isEnabled}

    open Html

    let toHTML (chart:Chart) =
        let chartNode =
            createDiv()
            |> addAttribute "class" "fsharp-idd" 
            |> addAttribute "data-idd-plot" "figure" 
            |> addAttribute "style" (sprintf "width: %dpx; height: %dpx;" chart.Width chart.Height)
                
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
        
        let effectiveLegendvisibility =
            match chart.IsLegendEnabled with
            |   Visible -> true
            |   Hidden -> false
            |   Automatic ->
                let isNameDefined plot =
                    match plot with
                    |   Polyline p -> p.Name <> null
                    |   Markers m -> m.Name <> null
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

        let getDataDom xSeries ySeries =
                // can't use string builder here as it is not transpilable with WebSharper
                let str = Seq.fold2 (fun state x y -> state + (sprintf "\t%f\t%f\n" x y)) "\tx\ty\n" xSeries ySeries                                
                str

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
                |> addText (getDataDom p.X p.Y)
                        
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
                |> addText (getDataDom m.X m.Y)
                        
            let resultNode =
                if System.String.IsNullOrEmpty m.Name then
                    resultNode
                else
                    resultNode |> addAttribute "data-idd-name" m.Name  

            resultNode
    
        let plotToDiv plot =
            match plot with
            |   Polyline p -> polylineToDiv p
            |   Markers m -> markersToDiv m

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

    
