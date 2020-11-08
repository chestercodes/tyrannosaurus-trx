# tyrannosaurus-trx

A trx file utility with most of the code copied from https://github.com/rndsolutions/trx-merger

The changes i've made are:

- Turning into dotnet tool
- Change the console app api
- Fixed a couple of bugs in the generator
- Changed the outputted report structure

## Installation

The cli is a `dotnet tool` and can be installed via the dotnet sdk with:

```
dotnet tool install --global TyrannosaurusTrx
```

## Usage

The cli will not run if no paths or a functionality isn't specified. 

---

To generate a report of all of the trx files in a folder `C:/some/folder` to the path `C:/some/report.html` 

````
t-trx -p C:/some/folder -r C:/some/report.html
```

To merge all of the trx files in a folder `C:/some/folder` to the path `C:/some/merged.trx` 

````
t-trx -p C:/some/folder -m C:/some/merged.trx
```

---

The full usage is:

```
USAGE: t-trx [--help] [--trx-paths [<paths>...]] [--recurse] [--merge-out <path>] [--report-out <path>]

OPTIONS:

    --trx-paths, -p [<paths>...]
                          Specify trx paths or directory
    --recurse             Recurse through directories looking for .trx files
    --merge-out, -m <path>
                          Merge output path or directory
    --report-out, -r <path>
                          Html report output path or directory
    --help                display this list of options.
```

## Tested with

I've tested this on windows with the example-tests folder.


