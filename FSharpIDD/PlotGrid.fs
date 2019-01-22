module FSharpIDD.PlotGrid

open WebSharper
open FSharpIDD.Html

type PlotGrid = 
    {
        Title: string
        Charts: Chart.Chart[,]
    }

let createPlotGrid nrow ncol initializer =    
    {
        Title= "Test plot grid"
        Charts = Array2D.init nrow ncol initializer
    }

let toHtmlStructure plotGrid =
    let root = createDiv()
    let root = addDiv  (createDiv() |> Html.addText plotGrid.Title) root
    
    let charts = plotGrid.Charts

    let ncol = Array2D.length2 charts
    let nrow = Array2D.length1 charts       

    let rec appendChart div charts =
        let folder state elem =
            let chart, row, col = elem
            let iddDiv =
                Chart.toHtmlStructure chart
                |> addAttribute "id" (sprintf "plot-%d-%d" row col)
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