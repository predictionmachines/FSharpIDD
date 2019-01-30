module FSharpIDD.PlotGrid

open WebSharper
open FSharpIDD.Html
open FSharpIDD.Chart
open HtmlConverters

type Subplots = 
    {
        Title: string
        Charts: Chart.Chart[,]
        Width: int 
        /// The height of each chart in pixels
        Height: int
    }

let createPlotGrid nrow ncol initializer =    
    {
        Title= "Test plot grid"
        Charts = Array2D.init nrow ncol initializer
        /// The width of the chart in pixels
        Width = 300
        Height = 200
    }

/// the chart without axis and titles
type BareChart = {
    /// The appearance of grid lines
    GridLines : Chart.GridLines
    /// A collection of plots (polyline, markers, bar charts, etc) to draw
    Plots: Plots.Plot list    
    }

let chartToBareChart (chart:Chart.Chart) : BareChart=
    {
        GridLines = chart.GridLines
        Plots = chart.Plots
    }

type Slot =
    |   Plot of BareChart
    |   Axis of Chart.Axis * AxisPlacement * title:string
    |   PlotTitle of string
    |   Empty

let chartsGridToSlotGrid charts = 
    let ncol = Array2D.length2 charts
    let nrow = Array2D.length1 charts    
    let slotGrid : Slot[,]  = Array2D.create (nrow*3) (ncol*3) Empty
    
    seq { 0..(nrow-1) } |> Seq.iter (fun rowIdx ->
         seq { 0..(ncol-1) } |> Seq.iter (fun colIdx ->
            let chart : Chart.Chart = downcast charts.GetValue(rowIdx,colIdx)
            // chart itself
            slotGrid.[rowIdx*3 + 1, colIdx*3 + 1] <- Plot(chartToBareChart chart)
            
            // upper slot can contain Chart title
            if chart.Title <> null then slotGrid.[rowIdx*3, colIdx*3 + 1] <- PlotTitle(chart.Title)

            // left slot may contain left axis
            slotGrid.[rowIdx*3 + 1, colIdx*3] <- Axis(chart.Yaxis, Left, chart.Ylabel)

            //bottom slot mat contain bottom axis
            slotGrid.[rowIdx*3 + 2, colIdx*3 + 1] <- Axis(chart.Xaxis, Bottom, chart.Xlabel)
            )
        )
    slotGrid

let slotToHtmlStructure (slot:Slot) : Html.Node =
    match slot with
    |   Empty -> Html.Empty
    |   Axis(axis,placement,title) ->        
        let div = Html.createDiv()
        let axisNode = HtmlConverters.axisToHtmlStructure axis placement
        let div =
            match axisNode with
            |   Some(axis,_) ->
                let axis = 
                    match placement with
                    |   Left| Right -> axis |> Html.addAttribute "style" "height: 100px;"
                    |   Top | Bottom -> axis |> Html.addAttribute "style" "width: 200px;"
                Html.addDiv axis div
            |   None -> div
        let div =
            if title <> null then
                match placement with
                |   Left| Right -> vertAxisLabelToHtmlStructue title placement div 
                |   Top | Bottom -> horAxisLabelToHtmlStructure title placement div 
            else
                div
        Div div
    |   Plot(bareChart) ->       
        let div =
            Html.createDiv()
            |> Html.addAttribute "data-idd-plot" "plot"
            |> Html.addAttribute "style" "height: 100px; width: 200px;"
            |> Html.addAttribute "data-idd-padding" "1"

        let plotTrap =
            let plotDiv = createDiv()
            Html.addAttribute "data-idd-plot" "subplots-trap" plotDiv

        let div = Html.addDiv plotTrap div

        let div = HtmlConverters.gridLinesToHtmlStructure bareChart.GridLines None None div
        Div (Seq.fold (fun state t -> let plotDiv = HtmlConverters.plotToDiv t in Html.addDiv plotDiv state) div bareChart.Plots)
    |   PlotTitle title ->
        let div =
            Html.createDiv()
            |> Html.addText title
            |> Html.addAttribute "class" "title"
        Div div

let slotGridToHtmlStructure (slots: Slot[,]) =
    let rows = Array2D.length1 slots
    let cols = Array2D.length2 slots

    let getRow i =
        Cells(seq { 0 .. (cols-1)} |> Seq.map (fun col -> TD(slots.[i,col] |> slotToHtmlStructure)) |> Seq.rev |> List.ofSeq)
    let rows = 
        Rows(seq { 0 .. (rows - 1) } |> Seq.map getRow |> Seq.rev |> List.ofSeq)
    Html.Table rows

(*
let toHtmlStructure subplots =
    let root = createDiv()
    let root = addDiv  (createDiv() |> Html.addText plotGrid.Title) root
    
    let charts = subplots.Charts

    let ncol = Array2D.length2 charts
    let nrow = Array2D.length1 charts       

    let plotTrap =
        let plotDiv = createDiv()
        Html.addAttribute "data-idd-plot" "subplots-trap" plotDiv

    let rec appendChart div charts =
        let folder state elem =
            let chart, row, col = elem

            // trap plot injection is required by IDD for subplots operation
            
            let iddDiv =
                Chart.toHtmlStructure chart                
            let outerDiv =
                createDiv()
                |> addAttribute "style" "display: inline-block;"
                |> addDiv iddDiv
            Html.addDiv outerDiv state
        Seq.fold folder div charts

    let getRow idx charts =
        let rowDiv = createDiv()        
        let rowDiv = seq { 0..(ncol-1) } |> Seq.map (fun col -> (Array2D.get charts idx col), idx, col) |> appendChart rowDiv        
        rowDiv
        
    let root = Seq.fold (fun state idx -> Html.addDiv (getRow idx charts) state) root (seq{ 0 .. (nrow-1)})

    root
    *)
let commonAxes grid =
    let lastRow = (Array2D.length1 grid.Charts)-1
    let replaceAxes r c chart =
        let chart = if r = lastRow then chart else Chart.setXaxis Chart.Axis.Hidden chart |> Chart.setXlabel null
        let chart = if c = 0 then chart else Chart.setYaxis Chart.Axis.Hidden chart |> Chart.setYlabel null
        chart
    {
        grid with
            Charts = Array2D.init (Array2D.length1 grid.Charts) (Array2D.length2 grid.Charts) (fun r c -> replaceAxes r c grid.Charts.[r,c])
    }