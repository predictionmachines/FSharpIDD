namespace FsharpIDD

/// This module is to reduce the dependencies count and to ease possible WebSharper compilation
module Colour =    
    type Colour = {
        R: byte
        G: byte
        B: byte
    }

    let createColour red green blue = {R=red; G=green;B=blue}

    let Red = createColour (byte 255) (byte 0) (byte 0)
    let Green = createColour (byte 0) (byte 255) (byte 0)
    let Blue = createColour (byte 0) (byte 0) (byte 255)

module Plot =            
    type DataSeries = float seq

    module Polyline =
        /// The single polyline settings
        type Plot = {
            /// Specifies how to annotate the polyline in the legend
            Name: string option
            /// Series of X coords of the points that form the polyline
            X: DataSeries
            /// Series of Y coords of the points that form the polyline
            Y: DataSeries
            /// The fill colour of the polyline
            Colour: Colour.Colour option
            /// The line thickness of the polyline
            Thickness: float option
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
                | Some(name) -> {polyline with Name = Some(name)}
            let polyline =
                match options.SpecifiedColour with
                | None -> polyline
                | Some(color) -> {polyline with Colour = Some(color)}
            let polyline =
                match options.SpecifiedThickness with
                | None -> polyline
                | Some(thickness) -> {polyline with Thickness = Some(thickness)}
            polyline
        


        /// Creates a basic polyline using the specifies set of X and Y coords
        let createPolyline x y =    
                {
                    X = x
                    Y = y
                    Colour = None
                    Thickness = None
                    Name = None
                }
        /// Changes the name of the polyline (how it is depicted in the legend)
        let setName name polyline =
            {
                polyline with
                    Name = Some(name)
            }

        /// Changes the colour of the polyline
        let setStrokeColour colour polyline =
            {
                polyline with
                    Colour = Some(colour)
            }

        let setThickness thickness polyline =
            {
                polyline with
                    Thickness = Some(thickness)
            }


    type Plot =
    |   Polyline of Polyline.Plot

    type SizeType = int

module Chart =
    open System.Xml
    open System.IO
    open System.Text

    open Plot

    /// Represents single chart that can be transformed later into the HTML IDD Chart
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
    

    let toHTML (chart:Chart) = 
        let root = XmlDocument()

        let chartNode = root.CreateElement("div")
        root.AppendChild(chartNode) |> ignore

        let chartClassAttribure = root.CreateAttribute("class")
        chartClassAttribure.AppendChild(root.CreateTextNode("fsharp-idd")) |> ignore
        chartNode.Attributes.Append(chartClassAttribure) |> ignore

        let chartPlotAttr = root.CreateAttribute("data-idd-plot")
        chartPlotAttr.AppendChild(root.CreateTextNode("chart")) |> ignore
        chartNode.Attributes.Append(chartPlotAttr) |> ignore
    
        let styleAttribute = root.CreateAttribute("style")
        styleAttribute.AppendChild(root.CreateTextNode("width: 800px; height: 600px;")) |> ignore
        chartNode.Attributes.Append(styleAttribute) |> ignore

        let polylineToDom (p:Polyline.Plot) =
            let getDataDom xSeries ySeries =
                let builder = StringBuilder()
                builder.AppendLine("\tx\ty") |> ignore
                Seq.iter2 (fun x y -> builder.AppendLine(sprintf "\t%f\t%f" x y) |> ignore) xSeries ySeries
                builder.ToString()

            let resultNode = root.CreateElement("div")

            let plotAttribute = root.CreateAttribute("data-idd-plot")
            plotAttribute.AppendChild(root.CreateTextNode("polyline")) |> ignore
            resultNode.Attributes.Append(plotAttribute) |> ignore

            let plotStyleAttribute = root.CreateAttribute("data-idd-style")
            let styleEntries = []
            let styleEntries = 
                match p.Thickness with
                | Some(thickness) -> (sprintf "thickness: %.1f" thickness)::styleEntries
                | None -> styleEntries
            let styleEntries = 
                match p.Colour with
                | Some(c) -> (sprintf "stroke: rgb(%d,%d,%d)" c.R c.G c.B)::styleEntries
                | None -> styleEntries            
            plotStyleAttribute.AppendChild(root.CreateTextNode(System.String.Join("; ",styleEntries))) |> ignore
            resultNode.Attributes.Append(plotStyleAttribute) |> ignore

            match p.Name with
            | Some(name) ->
                let nameAttribute = root.CreateAttribute("data-idd-name")
                nameAttribute.AppendChild(root.CreateTextNode(name)) |> ignore
                resultNode.Attributes.Append(nameAttribute) |> ignore
            | None -> ()

            resultNode.InnerText <- (getDataDom p.X p.Y)

            resultNode
    
        let plotToDom plot =
            match plot with
            |   Polyline p -> polylineToDom p

        let plotElems = chart.Plots |> Seq.map plotToDom
        plotElems |> Seq.iter (fun elem -> chartNode.AppendChild(elem) |> ignore)
        use writer = new StringWriter()
        use xmlWriter = new XmlTextWriter(writer)
        root.WriteContentTo(xmlWriter)
        let res = writer.ToString()
        res

    
