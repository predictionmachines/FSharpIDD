namespace FSharpIDD

open WebSharper

[<JavaScript>]
module Subplots =

    open FSharpIDD.DOM
    open FSharpIDD.Chart
    open HtmlConverters

    /// A collection of charts organized into rectangular grid
    type Subplots = 
        {
            /// Common title
            Title: string
            /// 0-based rowIdx,colIdx -> chart
            /// The absence of the index pair indicates the blank slot
            Charts: Map<(int*int),Chart option>
            /// The width of each chart in pixels
            PlotWidth: int 
            /// The height of each chart in pixels
            PlotHeight: int
            /// The number of rows in subplots
            RowsCount: int
            /// The number of columns in subplots
            ColumnsCount: int
            /// Which plot to use for externally placed legend
            /// None indicates that external legend is not
            ExternalLegendSource: (int*int*Placement) option                
        }

    /// Constructs subplots instance with nrow rows and ncol columns, filling up with the charts provided by initializer function
    let createSubplots nrow ncol initializer =    
        let colIndices = seq { 0 .. (ncol-1) }
        let idxPairs = seq { 0 .. (nrow-1) } |> Seq.map (fun rowIdx -> Seq.map (fun colIdx -> (rowIdx,colIdx)) colIndices) |> Seq.concat
        let folder state elem =
            let rowIdx,colIdx = elem
            let chart = initializer rowIdx colIdx
            Map.add elem chart state    
        {
            Title = null
            Charts = Seq.fold folder Map.empty idxPairs
            /// The width of each subplots
            PlotWidth = 300
            /// The hight of each subplots
            PlotHeight = 200
            RowsCount = nrow
            ColumnsCount = ncol
            ExternalLegendSource = None
        }
    
    /// Adds/Replaces the particular chart in subplots
    let setSubplot rowIdx colIdx chart subplots =
        {
            subplots with
                Charts = Map.add (rowIdx,colIdx) chart subplots.Charts
        }
    
    /// Sets the size of each subplot within subplots grid
    let setSubplotSize width height subplots =
        {
            subplots with
                PlotWidth = width;
                PlotHeight = height
        }
    
    /// Sets the title for the whole subplots grid
    let setTitle title subplots :Subplots =
        {
            subplots with
                Title = title
        }

    /// the chart without axis and titles
    /// Used to be placed into the slot of subplots
    type internal BareChart = {
        /// The appearance of grid lines
        GridLines : Chart.GridLines
        /// A collection of plots (polyline, markers, bar charts, etc) to draw
        Plots: Plots.Plot list
        /// Whether the legend (list of plot names and their icons) is visible in the top-right part of the chart
        IsLegendEnabled: LegendVisibility
        /// Whether the chart visible area can be navigated with a mouse or touch gestures
        IsNavigationEnabled: bool
        /// Whether the plot coordinates of the point under the mouse are shown in the tooltip
        IsTooltipPlotCoordsEnabled: bool
        /// Which visible rectangle is displayed by the chart
        VisibleRegion : VisibleRegion
        }

    [<JavaScript>]
    let internal chartToBareChart (chart:Chart.Chart) : BareChart=
        {
            GridLines = chart.GridLines
            Plots = chart.Plots
            IsLegendEnabled = chart.IsLegendEnabled
            IsNavigationEnabled = chart.IsNavigationEnabled
            IsTooltipPlotCoordsEnabled = chart.IsTooltipPlotCoordsEnabled
            VisibleRegion = chart.VisibleRegion
        }

    type internal Slot =
        |   Plot of BareChart
        |   Axis of Chart.Axis * Placement * title:string
        |   PlotTitle of string
        |   Empty

    let internal subplotsToSlotGrid subplots = 
        let nrow,ncol = subplots.RowsCount, subplots.ColumnsCount
        let slotGrid : Slot[,]  = Array2D.create (nrow*3) (ncol*3) Empty
    
        seq { 0..(nrow-1) } |> Seq.iter (fun rowIdx ->
             seq { 0..(ncol-1) } |> Seq.iter (fun colIdx ->
                  match Map.find (rowIdx,colIdx) subplots.Charts with
                  |   Some chart ->                
                      // chart itself
                      slotGrid.[rowIdx*3 + 1, colIdx*3 + 1] <- Plot(chartToBareChart chart)
              
                      // upper slot can contain Chart title
                      if chart.Title <> null then slotGrid.[rowIdx*3, colIdx*3 + 1] <- PlotTitle(chart.Title)

                      // left slot may contain left axis
                      slotGrid.[rowIdx*3 + 1, colIdx*3] <- Axis(chart.Yaxis, Left, chart.Ylabel)

                      //bottom slot mat contain bottom axis
                      slotGrid.[rowIdx*3 + 2, colIdx*3 + 1] <- Axis(chart.Xaxis, Bottom, chart.Xlabel)
                  |   None -> ()
                  )
            )
        slotGrid

    /// Width and height is in pixels
    let internal slotToHtmlStructure plotWidth plotHeight (slot:Slot) : DOM.Node =
        match slot with
        |   Empty -> DOM.Empty
        |   Axis(axis,placement,title) ->                    
            let slotContent = DOM.createDiv()                
            let tryAddAxisTitle slotContent =
                if title <> null then
                    let axisTitle =
                        match placement with
                        |   Left| Right ->
                            vertAxisLabelToHtmlStructue title placement 
                            |>   addAttribute "style" (sprintf "height: %dpx; position: relative;" plotHeight)
                        |   Top | Bottom ->
                            horAxisLabelToHtmlStructure title placement
                            |>   addAttribute "style" (sprintf "width: %dpx;" plotWidth)
                    DOM.addDiv axisTitle slotContent
                else
                    slotContent
            let tryAddAxis slotContent =
                let axisNode = HtmlConverters.axisToHtmlStructure axis placement                
                match axisNode with
                |   Some(axis,_) ->                                           
                    // fixing sizes
                    let axis = 
                        match placement with
                        |   Left| Right -> axis |> DOM.addAttribute "style" (sprintf "height: %dpx;" plotHeight)
                        |   Top | Bottom -> axis |> DOM.addAttribute "style" (sprintf "width: %dpx;" plotWidth)
                    // setting the styles that control the alignment
                    let axis = 
                        match placement with
                        | Left -> axis |> DOM.addAttribute "class" "idd-subplots-slot-left"
                        | Right -> axis |> DOM.addAttribute "class" "idd-subplots-slot-right"
                        | Top -> axis |> DOM.addAttribute "class" "idd-subplots-slot-top"
                        | Bottom -> axis |> DOM.addAttribute "class" "idd-subplots-slot-bottom"
                    DOM.addDiv axis slotContent
                |   None -> slotContent
            let slotContent =
                match placement with
                |   Left ->
                    slotContent
                    |> addAttribute "style" (sprintf "height: %dpx; display: flex; justify-content: flex-end;" plotHeight)
                    |> tryAddAxisTitle
                    |> tryAddAxis
                |   Bottom ->
                    slotContent
                    |> addAttribute "style" (sprintf "width: %dpx; display: flex; flex-direction: column; justify-content: flex-start; margin-left: auto; margin-right: auto;" plotWidth)                    
                    |> tryAddAxis
                    |> tryAddAxisTitle
                |   _   -> failwith "Not supported exception"
            Div slotContent
        |   Plot(bareChart) ->
            let div =
                DOM.createDiv()
                |> DOM.addAttribute "data-idd-plot" "plot"
                |> DOM.addAttribute "class" "idd-subplot"                
                |> DOM.addAttribute "style" (sprintf "height: %dpx; width: %dpx;" plotHeight plotWidth)
                |> addVisibleRegionAttribute bareChart.VisibleRegion

            let plotTrap =
                let plotDiv = createDiv()
                DOM.addAttribute "data-idd-plot" "subplots-trap" plotDiv

            let div = DOM.addDiv plotTrap div

            let div = HtmlConverters.gridLinesToHtmlStructure bareChart.GridLines None None div
            let div = Seq.fold (fun state t -> let plotDiv = HtmlConverters.plotToDiv t in DOM.addDiv plotDiv state) div bareChart.Plots
            let effectiveLegendvisibility = HtmlConverters.getEffectiveLegendvisibility bareChart.IsLegendEnabled bareChart.Plots
            let div = div |> addAttribute "data-idd-legend-enabled" (if effectiveLegendvisibility then "true" else "false")            
            let div = div |> DOM.addAttribute "data-idd-navigation-enabled" (if bareChart.IsNavigationEnabled then "true" else "false")
            Div div
        |   PlotTitle title ->
            let div =
                DOM.createDiv()
                |> DOM.addText title
                |> DOM.addAttribute "class" "idd-subplot-title"
            Div div

    let internal slotGridToHtmlStructure plotWidth plotHeight (slots: Slot[,]) =
        let rows = Array2D.length1 slots
        let cols = Array2D.length2 slots

        let getRow i =
            Cells(seq { 0 .. (cols-1)} |> Seq.map (fun col -> TD(slots.[i,col] |> slotToHtmlStructure plotWidth plotHeight)) |> Seq.rev |> List.ofSeq)
        let rows = 
            Rows(seq { 0 .. (rows - 1) } |> Seq.map getRow |> Seq.rev |> List.ofSeq)
        DOM.Table rows

    /// Coverts the subplots object into corresponding HTML structure
    let internal subplotsToHtmlStructure subplots =
        let slots = subplotsToSlotGrid subplots
        let htmlStructure = slotGridToHtmlStructure subplots.PlotWidth subplots.PlotHeight slots
        htmlStructure    

    let commonAxes subplots =
        let lastRow = subplots.RowsCount-1
        let replaceAxes r c chart =
            match chart with
            |   Some chart ->
                let chart = if r = lastRow then chart else Chart.setXaxis Chart.Axis.Hidden chart |> Chart.setXlabel null
                let chart = if c = 0 then chart else Chart.setYaxis Chart.Axis.Hidden chart |> Chart.setYlabel null
                Some chart
            |   None ->
                chart
        {
            subplots with
                Charts = Map.map (fun key chart-> let r,c = key in replaceAxes r c chart) subplots.Charts
        }