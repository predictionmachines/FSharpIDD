module CollectionToHtml

open System.Xml
open System.IO

let toHTML (sampleList: (string*string)list) = 
    let root = XmlDocument()

    let samplesNode = root.CreateElement("div")
    samplesNode.SetAttribute("class", "test-samples-container")
    root.AppendChild(samplesNode) |> ignore

    let arrangeSample sample =
        let (chartName: string, chartSample: string) = sample

        let sampleNode = root.CreateElement("div")
        sampleNode.SetAttribute("class", "test-sample")

        let chartNameDiv = root.CreateElement("div")
        chartNameDiv.SetAttribute("class", "test-sample-name")
        chartNameDiv.InnerText <- chartName
        sampleNode.AppendChild(chartNameDiv) |> ignore

        let chartDiv = root.CreateDocumentFragment()
        chartDiv.InnerXml <- chartSample
        sampleNode.AppendChild(chartDiv) |> ignore

        samplesNode.AppendChild(sampleNode) |> ignore

    sampleList |> Seq.iter arrangeSample
        
    use writer = new StringWriter()
    use xmlWriter = new XmlTextWriter(writer)
    root.WriteContentTo(xmlWriter)
    let res = writer.ToString()
    res