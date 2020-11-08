module App

open System
open System.IO
open TRX_Merger.TrxModel

type CliError =
    | TrxPathsNotSpecified
    | MergeOrReportNotSpecified
    | CouldNotFindAnyTrxPaths
    | FailedToMergeTrxFiles of exn

let log (line: string) = Console.WriteLine line ; ()

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
    match trxPaths |> List.collect (fun x -> resolveTrxPath x recurse) with
    | []           -> Error CouldNotFindAnyTrxPaths
    | trxFullPaths -> Ok trxFullPaths 

let (|NotSpecified|ExistingDirectory|ExistingFile|NonExistingFile|) p =
    match p with
    | None -> NotSpecified
    | Some path ->
        match path with
        | x when File.Exists x -> ExistingFile path
        | x when Directory.Exists x -> ExistingDirectory path
        | _ -> NonExistingFile path

let mergeTrxFiles trxPaths =
    try
        trxPaths
        |> TRX_Merger.TestRunMerger.MergeTrxFiles
        |> Ok
    with
        | ex -> FailedToMergeTrxFiles ex |> Error

let getFileName pathOpt dirDefaultName =
    match pathOpt with
    | NotSpecified -> None
    | ExistingDirectory dir -> Path.Combine(dir, dirDefaultName) |> Some
    | ExistingFile path -> Some path
    | NonExistingFile path ->
        let dir = Path.GetDirectoryName path
        if not(Directory.Exists dir) then
            Directory.CreateDirectory dir |> ignore
        else ()
        Some path

let runMergeFiles (mergePathOpt: string option) (trxPaths: string list) =
    mergeTrxFiles trxPaths
    |> Result.bind (fun testRun ->
        match getFileName mergePathOpt "merged.trx" with
        | None -> ()
        | Some path ->
            let mergedContents = 
                testRun
                |> TRX_Merger.Utilities.TRXSerializationUtils.SerializeTestRun
                |> fun x -> x.ToString()
            File.WriteAllText(path, mergedContents) 
        
        Ok testRun
    )
    
let runGenerateReport (reportPathOpt: string option) (testRun: TestRun) =
    match getFileName reportPathOpt "report.html" with
    | None -> Ok ()
    | Some mergePath ->
        let html =  TRX_Merger.ReportGenerator.TrxReportGenerator.GenerateReport testRun
        File.WriteAllText(mergePath, html)
        Ok ()
