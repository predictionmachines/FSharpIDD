namespace FSharpIDD

open WebSharper

[<AutoOpen>]
module Conversions =
    [<Inline "$0 % 256">]
    let inline byte x = byte x

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
        


        /// Creates a basic polyline using the specifies set of X and Y coords
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

    type Plot =
    |   Polyline of Polyline.Plot

    type SizeType = int

[<JavaScript>]
module Chart =        
    open Plots    

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
        /// The text which describes the Y axis
        Ylabel: string
        /// A collection of plots (polyline, markers, etc) to draw
        Plots: Plot list
    }

    let Empty : Chart = {
        Width = 800
        Height = 600
        Title = null // null means not set
        Xlabel = null // null means not set
        Ylabel = null // null means not set
        Plots = []
    }    

    let addPolyline polyline chart = { chart with Plots = Polyline(polyline)::chart.Plots }

    /// Sets the textual title that will be placed above the charting area
    let setTitle title chart =  { chart with Title = title}

    /// Sets the size of the chart in pixels
    let setSize width height chart = { chart with Width = width; Height = height}

    /// Sets the X axis textual  label (placed below the X axis)
    let setXlabel label chart = { chart with Xlabel = label}

    /// Set the Y axis textual label (placed to the left of Y axis)
    let setYlabel label chart = { chart with Ylabel = label}
    
    open Html

    let toHTML (chart:Chart) =
        let chartNode =
            createDiv()
            |> addAttribute "class" "fsharp-idd" 
            |> addAttribute "data-idd-plot" "chart" 
            |> addAttribute "style" (sprintf "width: %dpx; height: %dpx;" chart.Width chart.Height)
        
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
            if chart.Xlabel <> null then
                let labelNode =
                    createDiv()
                    |> addAttribute "class" "idd-horizontalTitle"
                    |> addAttribute "data-idd-placement" "bottom"
                    |> addText chart.Xlabel
                chartNode |> addDiv labelNode
            else
                chartNode

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

        let polylineToDiv (p:Polyline.Plot) =
            let getDataDom xSeries ySeries =
                // can't use string builder here as it is not transpilable with WebSharper
                let str = Seq.fold2 (fun state x y -> state + (sprintf "\t%f\t%f\n" x y)) "\tx\ty\n" xSeries ySeries                                
                str
                        
            let styleEntries = [ sprintf "thickness: %.1f" p.Thickness ]
            let styleEntries = 
                match p.Colour with
                | Colour.Rgb c -> (sprintf "stroke: rgb(%d,%d,%d)" c.R c.G c.B)::styleEntries
                | Colour.Default -> styleEntries                     
            let styleEntries = 
                let joinStr =
                    match p.LineJoin with
                    | Polyline.LineJoin.Miter -> "miter"
                    | Polyline.LineJoin.Bevel -> "bavel"
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
    
        let plotToDiv plot =
            match plot with
            |   Polyline p -> polylineToDiv p

        let plotElems = chart.Plots |> Seq.map plotToDiv
        let chartNode = Seq.fold (fun state elem -> addDiv elem state) chartNode plotElems
        
        divToStr chartNode

    
