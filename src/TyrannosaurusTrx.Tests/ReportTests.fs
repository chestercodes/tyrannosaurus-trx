module ReportTests


open System
open Xunit

let inputFile name = sprintf "./InputFiles/%s" name

[<Fact>]
let ``Generate output of the report and inspect manually`` () =
    let files = [
        inputFile "XUnit_AllPass.trx"
    ]

    match App.mergeTrxFiles files with
    | Error err -> Assert.True(false, (sprintf "Failed to merge files %A" err))
    | Ok testRun ->
        // writes output to Tyran.Test/bin/Debug/net5/out
        let output = "./out/InspectOutput.html"
        App.runGenerateReport (Some output) testRun |> ignore
