module App
open System
open System.IO

type CliError =
    | TrxPathsNotSpecified
    | MergeOrReportNotSpecified
    | CouldNotFindAnyTrxPaths
    | MultipleTrxFilesFoundAndMergePathNotSpecified

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

let runMergeFiles (mergePathOpt: string option) (trxPaths: string list) =
    match trxPaths with
    | [ trxPath ] ->
        match mergePathOpt with
        | NotSpecified -> Ok trxPath
        | ExistingDirectory dir -> 
            let path = Path.Combine(dir, "merged.trx")
            File.Copy(trxPath, path) 
            Ok trxPath
        | ExistingFile path ->
            File.Delete path
            File.Copy(trxPath, path)
            Ok trxPath
        | NonExistingFile path ->
            let dir = Path.GetDirectoryName path
            if not(Directory.Exists dir) then
                Directory.CreateDirectory dir |> ignore
            else ()
            File.Copy(trxPath, path) 
            Ok trxPath
    | trxPaths -> 
        match mergePathOpt with
        | NotSpecified ->
            log "TODO"
            trxPaths |> List.head |> Ok
        | ExistingDirectory dir ->
            log "TODO"
            trxPaths |> List.head |> Ok
        | ExistingFile path ->
            log "TODO"
            trxPaths |> List.head |> Ok
        | NonExistingFile path ->
            log "TODO"
            trxPaths |> List.head |> Ok
        
let runGenerateReport (reportPathOpt: string option) (trxPath: string) =
    match reportPathOpt with
    | None -> Ok ()
    | Some mergePath -> Ok ()
