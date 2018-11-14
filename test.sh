#!/usr/bin/env bash

#exit if any command fails
set -e

project="Invio.Extensions.Authentication.JwtBearer"

dotnet test ./test/${project}.Tests/${project}.Tests.csproj \
  --configuration Release \
  /p:CollectCoverage="true" \
  /p:CoverletOutputFormat="opencover" \
  /p:CoverletOutput="../../coverage/coverage.opencover.xml"