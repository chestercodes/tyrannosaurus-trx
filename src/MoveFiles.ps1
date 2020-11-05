$dir = $PsScriptRoot

function CopyFolder {
    param ($source, $destination)
    cp "$dir/$source" "$dir/$destination" -recurse
}

function CopyFile {
    param ($source, $destination)
    cp "$dir/$source" "$dir/$destination"
}

# CopyFolder "trx-merger/TRX_Merger/TrxModel"        "TyrannosaurusTrx.TrxMerger/TrxModel"
# CopyFolder "trx-merger/TRX_Merger/Utilities"       "TyrannosaurusTrx.TrxMerger/Utilities"

# CopyFile "trxer/TrxerConsole/functions.js"       "TyrannosaurusTrx.Trxer"
# CopyFile "trxer/TrxerConsole/ResourceReader.cs"  "TyrannosaurusTrx.Trxer"
# CopyFile "trxer/TrxerConsole/Test.xml.htm"       "TyrannosaurusTrx.Trxer"
# CopyFile "trxer/TrxerConsole/Trxer.css"           "TyrannosaurusTrx.Trxer"
# CopyFile "trxer/TrxerConsole/Trxer.xslt"         "TyrannosaurusTrx.Trxer"
# CopyFile "trxer/TrxerConsole/TrxerTable.css"     "TyrannosaurusTrx.Trxer"
