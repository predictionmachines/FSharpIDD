[<WebSharper.JavaScript>]
module BarsTest

open FSharpIDD.Plots
open FSharpIDD.Chart
open FSharpIDD
open FSharpIDD.Plots.Bars

let Xseries = Array.init 3 (fun i -> float(i+1))
let Yseries = Xseries |> Array.map (fun x -> sin(2.0*x)/x)

let Empty : Chart =
    Chart.Empty
    |> Chart.setSize 300 200
    |> Chart.setNavigationEnabled false
    

// Basic bar chart sample
let basicBarsPlot : Bars.Plot = createBars Xseries Yseries

// Basic bar chart test
let basicBarsChart = Chart.addBars basicBarsPlot Empty
let basicBarsTest =
    "Simple bar chart sample",
    [
        "createBars Xseries Yseries"
        "Blue bar charts"
    ],
    basicBarsChart
    

// Specifying empty set of bar chart options with setOptions call
let emptyOptionsBars = Bars.setOptions(Bars.Options()) basicBarsPlot
let emptyOptionsBarsChart = Chart.addBars emptyOptionsBars Empty
let emptyOptionsBarsTest =
    "Setting empty properties list",
    [
        "Bars.setOptions(Bars.Options())"
        "Same bar chart as on the previous chart"
    ],
    emptyOptionsBarsChart
    

// Specifying bar chart name with setOptions call
let nameBarsPlot = Bars.setOptions (Bars.Options(Name = "Bars")) basicBarsPlot
let nameBarsChart = Chart.addBars nameBarsPlot Empty
let nameBarsTest =
    "Specifying name of the bar chart",
    [
        "Bars.setOptions (Bars.Options(Name = 'Bars'))"
        "Same bar chart as on the previous chart with a legend "
    ],
    nameBarsChart
    

// Specifying borders and fill colours of the bar chart with setOptions call
let borderFillBarsPlot = Bars.setOptions (Bars.Options(BorderColour = Colour.Red, FillColour = Colour.Gray)) basicBarsPlot
let borderFillBarsPlotChart = Chart.addBars borderFillBarsPlot Empty
let borderFillBarsTest =
    "Specifying borders and fill colours of the bar chart",
    [
        "Bars.setOptions (Bars.Options(BorderColour = Colour.Red, FillColour = Colour.Gray))"
        "Gray bars with red borders"
    ],
    borderFillBarsPlotChart
    

// Specifying shadow colour of a bar with setOptions call
let shadowBarsPlot = Bars.setOptions (Bars.Options(Shadow = Shadow.WithShadow Colour.Turquoise)) basicBarsPlot
let shadowBarsPlotChart = Chart.addBars shadowBarsPlot Empty
let shadowBarsTest =
    "Specifying shadow color of a bar",
    [
        "Bars.setOptions (Bars.Options(Shadow = Shadow.WithShadow Colour.Turquoise))"
        "Bars have turquoise shadows"
    ],
    shadowBarsPlotChart
    

// Bars with default shadow color. Setting with setOptions call
let defaultShadowBarsPlot = Bars.setOptions (Bars.Options(Shadow = Shadow.WithShadow Colour.Default)) basicBarsPlot
let defaultShadowBarsPlotChart = Chart.addBars defaultShadowBarsPlot Empty
let defaultShadowBarsTest =
    "Default shadow colour of bars",
    [
        "Bars.setOptions (Bars.Options(Shadow = Shadow.WithShadow Colour.Default))"
        "Bars have a grey shadow"
    ],
    defaultShadowBarsPlotChart
    

// Specifying shadow colour of a bar with setOptions call
let withoutShadowBarsPlot = Bars.setOptions (Bars.Options(Shadow = Shadow.WithoutShadow)) basicBarsPlot
let withoutShadowBarsPlotChart = Chart.addBars withoutShadowBarsPlot Empty
let withoutShadowBarsTest =
    "Bar chart without shadow",
    [
        "Bars.setOptions (Bars.Options(Shadow = Shadow.WithoutShadow))"
        "Bars without shadow"
    ],
    withoutShadowBarsPlotChart
    

// Specifying width of a bar with setOptions call
let barWidthBarsPlot = Bars.setOptions (Bars.Options(BarWidth = 0.3)) basicBarsPlot
let barWidthBarsPlotChart = Chart.addBars barWidthBarsPlot Empty
let barWidthBarsTest =
    "Specifying width of a bar",
    [
        "Bars.setOptions (Bars.Options(BarWidth = 0.3))"
        "Bars are 0.3 (in plot coordinates) wide"
    ],
    barWidthBarsPlotChart