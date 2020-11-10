$dir = $PsScriptRoot

$packageVersion = "0.9.4"

dotnet pack "$dir/TyrannosaurusTrx.fsproj" `
    -p:PackageVersion=$packageVersion `
    -p:Authors=chestercodes `
    -p:Description="TyrannosaurusTrx is a dotnet tool fork of the TRX_Merger tool. It allows you to combine multiple TRX files in a single TRX file containing all the information from the TRX files passed to it and also to generate an html report from the TRX." `
    -p:PackageTags="trx test" `
    -p:LicenseUrl="https://github.com/chestercodes/tyrannosaurus-trx/blob/master/LICENSE.txt" `
    -p:ProjectUrl="https://github.com/chestercodes/tyrannosaurus-trx"

$publishPath = "$dir/out/TyrannosaurusTrx.$packageVersion.nupkg"
if(-not($publishPath))
{
    write-error "Cannot find package at $publishPath"
    exit 1
}

$apiKey = $env:NUGET_API_KEY

if($apiKey -eq $null)
{
    write-error "Cannot find api key in env"
    exit 1
}

dotnet nuget push $publishPath -k $apiKey -s https://api.nuget.org/v3/index.json
