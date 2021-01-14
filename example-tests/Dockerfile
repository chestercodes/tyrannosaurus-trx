FROM mcr.microsoft.com/dotnet/core/sdk:3.1
SHELL ["pwsh", "-command"]
ENV TRX_OUTDIR=/res
ENV TRX_TESTSDIR=/res/tests
ENV TRX_TESTSFAILED=/res/failed

RUN dotnet tool install --global TyrannosaurusTrx
RUN mkdir $env:TRX_OUTDIR
RUN mkdir $env:TRX_TESTSDIR

WORKDIR /src

COPY . .

RUN ls

RUN pwsh RunTests.ps1

RUN /root/.dotnet/tools/t-trx -p ($env:TRX_TESTSDIR) --recurse -m ($env:TRX_OUTDIR) -r ($env:TRX_OUTDIR)

RUN /root/.dotnet/tools/t-trx -p ('{0}/*Pass*' -f $env:TRX_TESTSDIR) \
    --recurse -m ('{0}/Pass.trx' -f $env:TRX_OUTDIR) \
    -r ('{0}/Pass.html' -f $env:TRX_OUTDIR)

#RUN if(test-path ($env:TESTSFAILED)){ exit 1 }
