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
module Plot =    
    type DataSeries = float seq
    
    module Polyline =
        /// The single polyline settings
        type Plot = {
            /// Specifies how to annotate the polyline in the legend
            Name: string
            /// Series of X coords of the points that form the polyline
            X: DataSeries
            /// Series of Y coords of the points that form the polyline
            Y: DataSeries
            /// The fill colour of the polyline
            Colour: Colour.Colour
            /// The line thickness of the polyline
            Thickness: float
        }
        
        type Options() =
            let mutable name: string option = None
            let mutable colour: Colour.Colour option = None
            let mutable thickness: float option = None

            member s.Name with set v = name <- Some(v)                
            member s.SpecifiedName with internal get() = name
            member s.Colour with set v = colour <- Some(v)
            member s.SpecifiedColour with internal get() = colour
            member s.Thickness with set v = thickness <- Some(v)
            member s.SpecifiedThickness with internal get() = thickness
                
    
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
            polyline
        


        /// Creates a basic polyline using the specifies set of X and Y coords
        let createPolyline x y =    
                {
                    X = x
                    Y = y
                    Colour = Colour.Default
                    Thickness = 1.0
                    Name = null
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

        let setThickness thickness polyline =
            {
                polyline with
                    Thickness = thickness
            }

    type Plot =
    |   Polyline of Polyline.Plot

    type SizeType = int

[<JavaScript>]
module Chart =        
    open Plot    

    /// Represents single chart that can be transformed later into the HTML IDD Chart
    [<ReflectedDefinition>]
    type Chart = {
        /// The width and height of the chart in pixels
        Size: (SizeType * SizeType) option // width * Height
        /// The text that is centered and placed above the chart
        Title: string option
        Xlabel: string option
        Ylabel: string Option
        Plots: Plot list
    }

    let Empty : Chart = {
        Size = None
        Title = None
        Xlabel = None
        Ylabel = None
        Plots = []
    }    

    let addPolyline polyline chart =
        {
            chart with
                Plots = Polyline(polyline)::chart.Plots
        }
    
    open Html

    let toHTML (chart:Chart) =
        let chartNode =
            createDiv()
            |> addAttribute "class" "fsharp-idd" 
            |> addAttribute "data-idd-plot" "chart" 
            |> addAttribute "style" "width: 800px; height: 600px;"

        let polylineToDiv (p:Polyline.Plot) =
            let getDataDom xSeries ySeries =
                // can't use string builder here as it is not transpilable with WebSharper
                let str = Seq.fold2 (fun state x y -> state + (sprintf "\t%f\t%f\n" x y)) "\tx\ty\n" xSeries ySeries                                
                str
            
            let styleEntries = []
            let styleEntries = (sprintf "thickness: %.1f" p.Thickness)::styleEntries
            let styleEntries = 
                match p.Colour with
                | Colour.Rgb c -> (sprintf "stroke: rgb(%d,%d,%d)" c.R c.G c.B)::styleEntries
                | Colour.Default -> styleEntries                     
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

    
