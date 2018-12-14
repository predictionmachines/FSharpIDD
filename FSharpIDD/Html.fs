namespace FSharpIDD

open WebSharper

/// The module provides basic HTML dom composing needed for IDD declarative chart creation without any external dependencies
[<JavaScript>]
module Html=

    /// This is guard type holding validated string
    type HtmlString = Valid of string

    type Attribute = {
        Key: HtmlString
        Value: HtmlString
    }

    type Node =
        |   Text of HtmlString
        |   Div of Div
    and Div = {
            Attributes: Attribute list
            Children:  Node list
        }

    let internal guardAttrName (str:string) : HtmlString = 
        let validChars = [|'-';|]
        if str.ToCharArray() |> Array.exists (fun c -> not(System.Char.IsLetterOrDigit(c) || (Array.contains c validChars)) ) then
            raise(System.ArgumentException(sprintf "the string must contain only letters, digits or %A, but %s was passed" validChars str))
        Valid str

    let internal guardAttrValue (str:string): HtmlString =
        // https://html.spec.whatwg.org/multipage/syntax.html#syntax-attribute-value
        // first escaping escape
        let str = str.Replace("\\","\\\\")
        // then escaping double quotes
        let str = str.Replace("\"","\\\"")
        Valid str


    let internal guardText (str:string) : HtmlString = 
        let validChars = [|'-';'['; ']'; '('; ')';':'; ';'; '.'; ','; '%'; '_'|]
        if str.ToCharArray() |> Array.exists (fun c -> not(System.Char.IsWhiteSpace(c) || System.Char.IsLetterOrDigit(c) || (Array.contains c validChars)) ) then
            raise(System.ArgumentException(sprintf "the string must contain only letters, digits or %A, but %s was passed" validChars str))
        Valid str

        

    let createDiv() =
        {
            Attributes = []
            Children = []
        }

    /// Adds the text inside the div (inner content)
    let addText text div = 
        {
            div with
                Children = (Text (guardText text)):: div.Children
        }

    let addDiv child parent = 
        {
            parent with
                Children = (Div child)::parent.Children
        }

    let addAttribute key value div =    
        {
            div with
                Attributes = {Key = guardAttrName key; Value = guardAttrValue value} :: div.Attributes
        }

    let rec divToStr div =
        let attributeToStr attr = 
            let key,value = 
                match attr.Key,attr.Value with
                | Valid key,Valid value -> key,value
            sprintf "%s=\"%s\"" key value
        let nodeToStr node = 
            match node with
            |   Text htmlText ->
                match htmlText with
                | Valid text -> text
            |   Div div -> divToStr div
        let attributesStr = 
            if List.length div.Attributes = 0 then ""
            else
                " "+System.String.Join(" ", div.Attributes |> Seq.map attributeToStr)
        let contents = System.String.Join("\n", div.Children |> Seq.map nodeToStr)
        sprintf "<div%s>%s</div>" attributesStr contents