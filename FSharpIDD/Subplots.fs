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
        /// The height of the chart in pixels
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
    |   Axis of Chart.Axis * AxisPlacement * string // axis , axis placement (left, bottom, ...) and title
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
            slotGrid.[rowIdx*3 + 1, colIdx*3] <- Axis(chart.Xaxis, Left, chart.Xlabel)

            //bottom slot mat contain bottom axis
            slotGrid.[rowIdx*3 + 1, colIdx*3 + 2] <- Axis(chart.Yaxis, Bottom, chart.Ylabel)
            )
        )
    slotGrid

let slotToHtmlStructure (slot:Slot) : Html.Node =
    match slot with
    |   Empty -> Html.Empty
    |   Axis(axis,placement,title) ->        
        let div = Html.createDiv()
        let div,_ = Chart.axisToHtmlStructure axis placement div
        let div =
            if title <> null then
                horAxisLabelToHtmlStructure title placement div
            else
                div
        div
    |   Plot(bareChart) ->



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