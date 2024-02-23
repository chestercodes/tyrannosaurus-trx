FROM mcr.microsoft.com/dotnet/sdk:8.0 as base
SHELL ["pwsh", "-command"]

WORKDIR /home/app

COPY src/TyrannosaurusTrx/TyrannosaurusTrx.fsproj src/TyrannosaurusTrx/TyrannosaurusTrx.fsproj
COPY src/TyrannosaurusTrx.Tests/TyrannosaurusTrx.Tests.fsproj src/TyrannosaurusTrx.Tests/TyrannosaurusTrx.Tests.fsproj
COPY src/TyrannosaurusTrx.TrxMerger/TyrannosaurusTrx.TrxMerger.csproj src/TyrannosaurusTrx.TrxMerger/TyrannosaurusTrx.TrxMerger.csproj
COPY src/TyrannosaurusTrx.sln src/TyrannosaurusTrx.sln

WORKDIR /home/app/src

RUN dotnet restore

WORKDIR /home/app/src

COPY src .

RUN dotnet test

ENV TRX_OUTDIR=/home/app/out
ENV TRX_TESTSDIR=/home/app/out-tests
ENV TRX_TESTSFAILED=/home/app/out-failed

RUN mkdir $env:TRX_OUTDIR
RUN mkdir $env:TRX_TESTSDIR

COPY example-tests example-tests

RUN pwsh example-tests/RunTests.ps1

WORKDIR /home/app/src/TyrannosaurusTrx

CMD dotnet run --framework net8.0 -- -p ($env:TRX_TESTSDIR) --recurse -m ($env:TRX_OUTDIR) -r ($env:TRX_OUTDIR)

FROM base as publish

CMD pwsh ./src/TyrannosaurusTrx/BuildTool.ps1