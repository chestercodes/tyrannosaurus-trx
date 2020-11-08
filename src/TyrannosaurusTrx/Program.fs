open System
open Argu
open App

type CmdArgs =
    | [<AltCommandLine("-p")>] Trx_Paths of paths:string list
    | Recurse
    | [<AltCommandLine("-m")>] Merge_Out of path:string
    | [<AltCommandLine("-r")>] Report_Out of path:string
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Trx_Paths _ -> "Specify trx paths or directory"
            | Recurse -> "Recurse through directories looking for .trx files"
            | Merge_Out _ -> "Merge output path or directory"
            | Report_Out _ -> "Html report output path or directory"

let getExitCode result =
    match result with
    | Ok () -> 0
    | Error err ->
        match err with
        | TrxPathsNotSpecified -> 1
        | MergeOrReportNotSpecified -> 2
        | CouldNotFindAnyTrxPaths -> 3
        | FailedToMergeTrxFiles _ -> 4
        | FailedToGenerateReport _ -> 5
    
let printResult result =
    match result with
    | Ok () -> "Finished"
    | Error err ->
        match err with
        | TrxPathsNotSpecified -> "Need to specify trx path(s)"
        | MergeOrReportNotSpecified -> "Need to specify either merge or report"
        | CouldNotFindAnyTrxPaths -> "Could not find any .trx files"
        | FailedToMergeTrxFiles ex -> sprintf "Failed to merge trx files: %A" ex
        | FailedToGenerateReport ex -> sprintf "Failed to generate report: %A" ex

let runProgram argv =
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)
    let parser = ArgumentParser.Create<CmdArgs>(programName = "t-trx", errorHandler = errorHandler)
    
    let argz = parser.ParseCommandLine argv
    if not(argz.Contains(Trx_Paths)) then
        log (parser.PrintUsage())
        Error TrxPathsNotSpecified
    else
        if not(argz.Contains(Report_Out)) && not(argz.Contains(Merge_Out)) then
            log (parser.PrintUsage())
            Error MergeOrReportNotSpecified
        else
            let mergePathOpt = if argz.Contains(Merge_Out) then Some (argz.GetResult(Merge_Out)) else None
            let reportPathOpt = if argz.Contains(Report_Out) then Some (argz.GetResult(Report_Out)) else None
            let mergeFiles = runMergeFiles mergePathOpt
            let generateReport = runGenerateReport reportPathOpt
            
            resolveTrxPaths (argz.GetResult(Trx_Paths)) (argz.Contains(Recurse))
            |> Result.bind mergeFiles
            |> Result.bind generateReport
    |> fun res -> 
        printResult res |> log
        res
    |> getExitCode

[<EntryPoint>]
let main argv = 
    //runProgram [| "-p"; "C:/temp/trx-files"; "-m"; "C:/temp/"; "-r"; "C:/temp/" |]
    runProgram argv