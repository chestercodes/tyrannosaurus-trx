module App
open System
open System.IO
open System.Xml.Linq
open TRX_Merger.TrxModel

type CliError =
    | TrxPathsNotSpecified
    | MergeOrReportNotSpecified
    | CouldNotFindAnyTrxPaths
    | MultipleTrxFilesFoundAndMergePathNotSpecified
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
        //|> TRX_Merger.Utilities.TRXSerializationUtils.SerializeTestRun
        |> Ok
    with
        | ex -> 
            sprintf "Error: %A" ex |> log
            FailedToMergeTrxFiles ex |> Error

let runMergeFiles (mergePathOpt: string option) (trxPaths: string list) =
    mergeTrxFiles trxPaths
    |> Result.bind (fun doc ->
        let mergedContents = doc.ToString()
        match mergePathOpt with
        | NotSpecified -> Ok doc
        | ExistingDirectory dir -> 
            log "TODO"
            //let path = Path.Combine(dir, "merged.trx")
            //File.Copy(trxPath, path) 
            Ok doc
        | ExistingFile path ->
            log "TODO"
            //File.Delete path
            //File.Copy(trxPath, path)
            Ok doc
        | NonExistingFile path ->
            log "TODO"
            //let dir = Path.GetDirectoryName path
            //if not(Directory.Exists dir) then
            //    Directory.CreateDirectory dir |> ignore
            //else ()
            //File.Copy(trxPath, path) 
            Ok doc
    )
    
let runGenerateReport (reportPathOpt: string option) (testRun: TestRun) =
    let html =  TRX_Merger.ReportGenerator.TrxReportGenerator.GenerateReport testRun
    File.WriteAllText("C:/temp/Some.html", html)
    match reportPathOpt with
    | None -> Ok ()
    | Some mergePath -> Ok ()
