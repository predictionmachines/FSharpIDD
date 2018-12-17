module BarChartTest

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
let basicBarChartPlot : Bars.Plot = createBars Xseries Yseries

// Basic bar chart test
let basicBarsChart = Chart.addBars basicBarChartPlot Empty
let basicBarChartTest =
    "Simple bar chart sample",
    [
        "createBarChart Xseries Yseries"
        "Blue bar charts"
    ],
    basicBarsChart
    

// Specifying empty set of bar chart options with setOptions call
let emptyOptionsBarChart = Bars.setOptions(Bars.Options()) basicBarChartPlot
let emptyOptionsBarsChart = Chart.addBars emptyOptionsBarChart Empty
let emptyOptionsBarChartTest =
    "Setting empty properties list",
    [
        "BarChart.setOptions(Bars.Options())"
        "Same bar chart as on the previous chart"
    ],
    emptyOptionsBarsChart
    

// Specifying bar chart name with setOptions call
let nameBarChartPlot = Bars.setOptions (Bars.Options(Name = "BarChart")) basicBarChartPlot
let nameBarsChart = Chart.addBars nameBarChartPlot Empty
let nameBarChartTest =
    "Specifying name of the bar chart",
    [
        "BarChart.setOptions (Bars.Options(Name = 'BarChart'))"
        "Same bar chart as on the previous chart with a legend "
    ],
    nameBarsChart
    

// Specifying borders and fill colours of the bar chart with setOptions call
let borderFillBarChartPlot = Bars.setOptions (Bars.Options(BorderColour = Colour.Red, FillColour = Colour.Gray)) basicBarChartPlot
let borderFillBarChartPlotChart = Chart.addBars borderFillBarChartPlot Empty
let borderFillBarChartTest =
    "Specifying borders and fill colours of the bar chart",
    [
        "BarChart.setOptions (Bars.Options(BorderColour = Colour.Red, FillColour = Colour.Gray))"
        "BarChart are green with blue borders now "
    ],
    borderFillBarChartPlotChart
    

// Specifying shadow colour of a bar with setOptions call
let shadowBarChartPlot = Bars.setOptions (Bars.Options(Shadow = Shadow.WithShadow Colour.Turquoise)) basicBarChartPlot
let shadowBarChartPlotChart = Chart.addBars shadowBarChartPlot Empty
let shadowBarChartTest =
    "Specifying shadow color of a bar",
    [
        "BarChart.setOptions (Bars.Options(Shadow = Shadow.WithShadow Colour.Turquoise))"
        "Bars have a green shadow"
    ],
    shadowBarChartPlotChart
    

// Bars with default shadow color. Setting with setOptions call
let defaultShadowBarChartPlot = Bars.setOptions (Bars.Options(Shadow = Shadow.WithShadow Colour.Default)) basicBarChartPlot
let defaultShadowBarChartPlotChart = Chart.addBars defaultShadowBarChartPlot Empty
let defaultShadowBarChartTest =
    "Default shadow colour of bars",
    [
        "BarChart.setOptions (Bars.Options(Shadow = Shadow.WithShadow Colour.Default))"
        "Bars have a grey shadow"
    ],
    defaultShadowBarChartPlotChart
    

// Specifying shadow colour of a bar with setOptions call
let withoutShadowBarChartPlot = Bars.setOptions (Bars.Options(Shadow = Shadow.WithoutShadow)) basicBarChartPlot
let withoutShadowBarChartPlotChart = Chart.addBars withoutShadowBarChartPlot Empty
let withoutShadowBarChartTest =
    "Bar chart without shadow",
    [
        "BarChart.setOptions (BarChart.Options(Shadow = Shadow.WithoutShadow))"
        "Bars without shadow"
    ],
    withoutShadowBarChartPlotChart
    

// Specifying width of a bar with setOptions call
let barWidthBarChartPlot = Bars.setOptions (Bars.Options(BarWidth = 0.3)) basicBarChartPlot
let barWidthBarChartPlotChart = Chart.addBars barWidthBarChartPlot Empty
let barWidthBarChartTest =
    "Specifying width of a bar",
    [
        "BarChart.setOptions (BarChart.Options(BarWidth = 0.3))"
        "Bars are 0.3 (in plot coordinates) wide"
    ],
    barWidthBarChartPlotChart