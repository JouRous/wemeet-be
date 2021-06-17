#!/bin/bash

set -e

run_cmd="dotnet run"

until dotnet ef database update; do
sleep 1
done

exec $run_cmd