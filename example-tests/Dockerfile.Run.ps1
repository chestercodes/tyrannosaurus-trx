$dir = $PsScriptRoot

$label = "tyrannosaurus-tests"

docker build -t $label .

docker run $label

$containerId = docker container ls -q --all --filter "ancestor=tyrannosaurus-tests"

docker cp $containerId`:/res ./out
