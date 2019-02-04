namespace FSharpIDD

open WebSharper

[<AutoOpen>]
module Conversions =
    [<Inline "$0 % 256">]
    let inline byte x = byte x

module Utils =

    [<Inline "Math.floor(new Date().valueOf() * Math.random()).toString()">]
    let getUniqueId(): string = 
        System.Guid.NewGuid().ToString()
            
    [<Inline "btoa((new Uint8Array((Float64Array.from($0)).buffer)).reduce(function (data, byte) { return data + String.fromCharCode(byte)}, ''))">]
    let encodeFloat64ArrayBase64 (data:float array) =
        let bytes = data |> Seq.collect (fun (elem:float) -> System.BitConverter.GetBytes(elem)) |> Array.ofSeq
        let base64str = System.Convert.ToBase64String bytes
        base64str

    [<Inline "throw \"not NotImplementedException\"">]
    let encodeStringSeqBase64 (data:string) =
        let bytes = data |> System.Text.Encoding.ASCII.GetBytes
        let base64str = System.Convert.ToBase64String bytes
        base64str

// Common parts for different plots

namespace FSharpIDD.Plots

open WebSharper

[<JavaScript>]
type DataSeries = float seq

type SizeType = int