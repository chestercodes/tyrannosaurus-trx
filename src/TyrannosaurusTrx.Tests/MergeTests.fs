module MergeTests

open System
open Xunit

let inputFile name = sprintf "./InputFiles/%s" name

[<Fact>]
let ``Test that merges pass and skipped nunit ok`` () =
    let files = [
        inputFile "NUnit_AllPass.trx"
        inputFile "NUnit_AllPass_SomeIgnored.trx"
    ]

    match App.mergeTrxFiles files with
    | Error err -> Assert.True(false, (sprintf "Failed to merge files %A" err))
    | Ok testRun -> 
        Assert.Equal("Completed", testRun.ResultSummary.Outcome)
