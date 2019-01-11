module CollectionToHtml

open FSharpIDD.Html
open WebSharper

[<JavaScript>]
let toHTML (sampleList: (string*(string list)*string)list) = 
    let testsContainer = addAttribute "class" "test-samples-container" (createDiv())

    let arrangeSample oldNode sample =
        let (chartName: string, chartDescrs: string list, chartSample: string) = sample
        
        let sampleNode = addAttribute "class" "test-sample" (createDiv())
        
        let chartNameDiv = addAttribute "class" "test-sample-name" (createDiv())
        let sampleNode = addDiv (addInnerHtml chartSample (createDiv())) sampleNode

        let addDescrDiv oldNode descr =
            addDiv (addText descr (addAttribute "class" "test-sample-desc" (createDiv()))) oldNode
                
        let sampleNode = chartDescrs |> Seq.fold addDescrDiv sampleNode

        let sampleNode = addDiv (addText chartName chartNameDiv) sampleNode
        
        addDiv sampleNode oldNode

    let testsContainer = sampleList |> Seq.fold arrangeSample testsContainer

    let revertedTests = {
        testsContainer with
            Children = List.rev testsContainer.Children
        }
    
    divToStr (addDiv revertedTests (createDiv()))