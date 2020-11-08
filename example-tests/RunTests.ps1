$dir = $PsScriptRoot

$tests = gci "$dir/**/*.Tests.csproj"

function RunTestProj($absPath){
    $name = $absPath.Replace('\ '.Replace(' ', ''), '_').Replace('.', '_')
    write-host ('Running dotnet test for {0} -> {1}' -f $absPath, $name)
    dotnet test $absPath --logger ('trx;LogFileName={0}/{1}.trx' -f "$dir/out", $name)
    # if($LASTEXITCODE -ne 0){
    #     out-file -filepath ('{0}/failed' -f $env:TESTDIR)
    # }
}
gci -Recurse **/*Tests.*proj |% { 
    RunTestProj (resolve-path -relative $_.FullName)
}