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

```
t-trx -p C:/some/folder -r C:/some/report.html
```

To merge all of the trx files in a folder `C:/some/folder` to the path `C:/some/merged.trx` 

```
t-trx -p C:/some/folder -m C:/some/merged.trx
```

To do all of the above recursively 

```
t-trx -p C:/some/folder -m C:/some/merged.trx -r C:/some/report.html --recurse
```

To generate a report from a single trx file to the default report name for a directory

```
t-trx -p C:/some/folder/some.trx -r C:/some
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

I've tested this on windows and linux with the example-tests folder.

## Rough changelog

 version | description
 ---     | ---
 0.9.5 and below | basic tool setup and migration to 3.1 and 5
 0.9.6 | Make github actions build and push tool
 1.0.0 | Added test, removed old code and merged in PR that fixed the skipped tests and completed label
 1.0.1 | Fixed bug relating to parameterised test names creating invalid chars for the HTML
 1.0.2 | Added .Net 6.0 Target
