# MasterMemoryとMessagePcakのコード生成をする

#!/bin/bash
set -euxo pipefail
cd "$(dirname "$0")" #cwd tools/scripts

cd ../../client

dotnet tool restore

dotnet dotnet-mmgen \
    -i "Assets/Project/Scripts/Infra/Master" \
    -o "Assets/Project/Scripts/Infra/Master" \
    -n "Omino.Infra.Master"

dotnet mpc \
    -i "./" \
    -o "Assets/Project/Scripts/Master/MessagePacks" \
