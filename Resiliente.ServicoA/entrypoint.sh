#!/bin/bash

set -e
run_cmd="dotnet Resiliente.ServicoA.dll"

>&2 echo "Esperando o carregamento da infra"
sleep 10

exec $run_cmd
