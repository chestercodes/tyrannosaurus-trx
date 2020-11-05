open System
open System.IO
open Argu

type CliError =
    | TrxPathsNotSpecified
    | MergeOrReportNotSpecified
    | CouldNotFindAnyTrxPaths

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
        | MergeOrReportNotSpecified -> 1
        | CouldNotFindAnyTrxPaths -> 1

let log (line: string) =
    Console.WriteLine line
    ()

let resolveTrxPath trxPath recurse = 
    let fullPath = Path.GetFullPath trxPath
    let logFile path = log (sprintf "File path: '%s'" path)
    let logDir path = log (sprintf "Directory path: '%s'" path)
    
    if Directory.Exists fullPath then
        logDir fullPath
        let searchOpts = if recurse then SearchOption.AllDirectories else SearchOption.TopDirectoryOnly
        let files = Directory.GetFiles(fullPath, "*.trx", searchOpts) |> Array.toList
        for p in files do logFile p
        files
    elif File.Exists fullPath then
        logFile fullPath
        [ fullPath ]
    else []

let resolveTrxPaths trxPaths recurse = 
    trxPaths |> List.collect (fun x -> resolveTrxPath x recurse)    
    
[<EntryPoint>]
let main argv = 
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)
    let parser = ArgumentParser.Create<CmdArgs>(programName = "t-trx", errorHandler = errorHandler)
    
    let argz = parser.ParseCommandLine argv
    if not(argz.Contains(Trx_Paths)) then
        Error TrxPathsNotSpecified
    else
        if not(argz.Contains(Report_Out)) && not(argz.Contains(Merge_Out)) then
            Error MergeOrReportNotSpecified
        else 
            match resolveTrxPaths (argz.GetResult(Trx_Paths)) (argz.Contains(Recurse)) with
            | [] -> Error CouldNotFindAnyTrxPaths
            | trxFullPaths -> 
                Ok ()
    |> getExitCode