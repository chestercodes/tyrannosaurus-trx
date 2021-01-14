$dir = $PsScriptRoot

$tests = gci "$dir/**/*.Tests.csproj"

if($env:TRX_TESTSDIR -ne $null){
    $testsDir = $env:TRX_TESTSDIR
} else {
    $testsDir = "$dir/out"
}

if($env:TRX_TESTSFAILED -ne $null){
    $testsFailed = $env:TRX_TESTSFAILED
} else {
    $testsFailed = "$testsDir/failed"
}

function RunTestProj($absPath)
{
    $name = $absPath.Replace('\ '.Replace(' ', ''), '').Replace('.', '_').Replace('/', '_')
    write-host ('Running dotnet test for {0} -> {1}' -f $absPath, $name)
    $fileName = "$name.trx"
    $filePath = "$testsDir/$fileName"
    dotnet test $absPath --logger ('trx;LogFileName={0}' -f $filePath)
    # $dest = ('{0}/{1}' -f $testsDir, $fileName)
    # cp $fileName $dest
    if($LASTEXITCODE -ne 0)
    {
        out-file -filepath $testsFailed
    }
}
gci -Recurse *.csproj |% { 
    RunTestProj (resolve-path -relative $_.FullName)
}