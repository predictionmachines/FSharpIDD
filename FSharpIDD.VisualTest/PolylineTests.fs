module PolylineTests

open FSharpIDD.Plots
open FSharpIDD.Chart
open FSharpIDD
open FSharpIDD.Plots.Polyline

let Xseries = Array.init 3 (fun i -> float(i+1))
let Yseries = Xseries |> Array.map (fun x -> sin(2.0*x)/x)

let Empty : Chart =
    Chart.Empty
    |> Chart.setSize 300 200
    |> Chart.setNavigationEnabled false

let blue20RoundRoundCurve : Polyline.Plot =
    {
        X = Xseries
        Y = Yseries
        Name = "Original"
        Colour = Colour.Blue
        Thickness = 20.0
        LineCap = LineCap.Round
        LineJoin = LineJoin.Round
    }

// Preset chart
let presetChart = Chart.addPolyline blue20RoundRoundCurve Empty
let presetChartStr = presetChart |> toHTML
let presetPolylineTest =
    "This is a chart which properies are overridden in all of the next tests",
    [
        "{ Name = 'Original'; X = Xseries; Y = Yseries; Colour = Colour.Blue; Thickness = 20.0; LineCap = LineCap.Round; LineJoin = LineJoin.Round }"
        "A blue polyline 20px thick with round cap, round join and a legend"
    ],
    presetChartStr
    

// Specifying empty set of polyline options with setOptions call
let noPropertiesSetCurve = Polyline.setOptions (Polyline.Options()) blue20RoundRoundCurve
let noNewPropertiesSetChart = Chart.addPolyline noPropertiesSetCurve Empty
let noNewPropertiesSetChartStr = noNewPropertiesSetChart |> toHTML
let setNonePolylineTest =
    "Setting empty properties list",
    [
        "Polyline.setOptions(Polyline.Options())"
        "Chart is the same as previous"
    ],
    noNewPropertiesSetChartStr
    

// Specifying polyline name and colour with setOptions call
let nameColourCurve = Polyline.setOptions (Polyline.Options(Name = "New Name", Colour = Colour.Green)) blue20RoundRoundCurve
let nameColourCurveChart = Chart.addPolyline nameColourCurve Empty
let nameColourCurveChartStr = nameColourCurveChart |> toHTML
let nameColourTest =
    "Specifying name and colour",
    [
        "Polyline.setOptions(Polyline.Options(Name = 'New Name', Colour = Colour.Green))"
        "The Green polyline with a name in a legend"
    ],
    nameColourCurveChartStr
    

// Specifying polyline thickness with setOptions call
let thicknessCurve = Polyline.setOptions (Polyline.Options(Thickness = 5.0)) blue20RoundRoundCurve
let thicknessCurveChart = Chart.addPolyline thicknessCurve Empty
let thicknessCurveChartStr = thicknessCurveChart |> toHTML
let thicknessTest =
    "Specifying polyline thickness",
    [
        "Polyline.setOptions(Polyline.Options(Thickness = 5.0))"
        "The polyline with a 5px thick line"
    ],
    thicknessCurveChartStr


// Specifying line cap and line join options with setOptions call
let squareMiterCurve = Polyline.setOptions (Polyline.Options(LineCap = LineCap.Square, LineJoin = LineJoin.Miter)) blue20RoundRoundCurve
let squareMiterCurveChart = Chart.addPolyline squareMiterCurve Empty
let squareMiterCurveChartStr = squareMiterCurveChart |> toHTML
let squareMiterTest =
    "Overriding line cap and line join properties",
    [
        "Polyline.setOptions (Polyline.Options(LineCap = LineCap.Square, LineJoin = LineJoin.Miter))"
        "The polyline with a square line cap and miter line join"
    ],
    squareMiterCurveChartStr


// Specifying line cap and line join options with setOptions call
let buttBevelCurve = Polyline.setOptions (Polyline.Options(LineCap = LineCap.Butt, LineJoin = LineJoin.Bevel)) blue20RoundRoundCurve
let buttBevelCurveChart = Chart.addPolyline buttBevelCurve Empty
let buttBevelCurveChartStr = buttBevelCurveChart |> toHTML
let buttBevelTest =
    "Overriding line cap and line join properties",
    [
        "Polyline.setOptions (Polyline.Options(LineCap = LineCap.Butt, LineJoin = LineJoin.Bevel))"
        "The polyline with a butt line cap and bevel line join"
    ],
    buttBevelCurveChartStr


// Specifying the full set of polyline options with setOptions call
let allSetCurve = Polyline.setOptions (Polyline.Options(Name = "All Set Name", Colour = Colour.Red, Thickness = 1.0, LineCap = LineCap.Square, LineJoin = LineJoin.Miter)) blue20RoundRoundCurve
let allSetChart = Chart.addPolyline allSetCurve Empty
let allSetChartStr = allSetChart |> toHTML
let setAllTest =
    "Overriding all properties",
    [
        "Polyline.setOptions (Polyline.Options (Name='All Set Name', Colour=Colour.Red, Thickness=1.0, LineCap=LineCap.Square, LineJoin=LineJoin.Miter) )"
        "A red polyline 1px thick with a square line cap, miter line join and a legend with its name"
    ],
    allSetChartStr


// Specifying polyline name and stroke colour with set[Property] call
let setNameSetStrokeCurve =
    blue20RoundRoundCurve
    |> Polyline.setName "Green polyline name"
    |> Polyline.setStrokeColour Colour.Green
let setNameSetStrokeCurveChart = Chart.addPolyline setNameSetStrokeCurve Empty
let setNameSetStrokeCurveChartStr = setNameSetStrokeCurveChart |> toHTML
let setNameSetStrokeTest =
    "Setting polyline name and stroke colour",
    [
        "|> Polyline.setName 'Green polyline name' |> Polyline.setStrokeColour Colour.Green"
        "A green polyline and a legend with its name"
    ],
    setNameSetStrokeCurveChartStr


// Specifying polyline thickness, line cap and join with set[Property] call
let setThicknessSetLineCapSetLineJoinCurve =
    blue20RoundRoundCurve
    |> Polyline.setThickness 50.0
    |> Polyline.setLineCap LineCap.Square
    |> Polyline.setLineJoin LineJoin.Miter
let setThicknessSetLineCapSetLineJoinCurveChart = Chart.addPolyline setThicknessSetLineCapSetLineJoinCurve Empty
let setThicknessSetLineCapSetLineJoinCurveChartStr = setThicknessSetLineCapSetLineJoinCurveChart |> toHTML
let setThicknessSetLineCapSetLineJoinTest =
    "Setting line thickness, cap and join",
    [
        "|> Polyline.setThickness 50.0 |> Polyline.setLineCap LineCap.Square |> Polyline.setLineJoin LineJoin.Miter"
        "A polyline 50px thick with a square line cap, miter line join"
    ],
    setThicknessSetLineCapSetLineJoinCurveChartStr

    
// Specifying chart title
let titleCurveChart = Chart.addPolyline blue20RoundRoundCurve Empty
let titleCurveChartStr = titleCurveChart |> Chart.setTitle "Chart title" |> toHTML
let titleTest =
    "Setting the chart name",
    [
        "|> Chart.setTitle 'Chart title'"
        "Chart has a title above it"
    ],
    titleCurveChartStr

    
// Specifying axes names
let axisCurveChart = Chart.addPolyline blue20RoundRoundCurve Empty
let axisCurveChartStr = axisCurveChart |> Chart.setXlabel "Horizontal axis" |> Chart.setYlabel "Vertical axis" |> toHTML
let axisTest =
    "Setting axes names",
    [
        "|> Chart.setXlabel 'Horizontal axis' |> Chart.setYlabel 'Vertical axis'"
        "X and Y axes have titles"
    ],
    axisCurveChartStr


// Default polyline via createPolyline
let basicPolylinePlot : Polyline.Plot = createPolyline Xseries Yseries
let basicPolylineChart = Chart.addPolyline basicPolylinePlot Empty
let basicPolylineChartStr = basicPolylineChart |> toHTML
let basicPolylineTest =
    "Default polyline via createPolyline",
    [
        "createPolyline Xseries Yseries"
        "Blue polyline 1px thick with no name specified in a legend"
    ],
    basicPolylineChartStr