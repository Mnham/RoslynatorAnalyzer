#!/usr/bin/env sh

mkdir /repository
cp -R . /repository/

dotnet /app/publish/Roslynator.Analyzer.dll --repository /repository
if [ ! $? -eq 0 ]; then
  exit 1
fi