#!/bin/bash -e
export PACKAGE_VERSION="9.0.7"
export PACKAGE_RELEASE_NOTES="Update to .NET 9.0.8; Upgrade packages;"
export PACKAGE_TAGS="nhibernate, npgsql, dotnet, dotnet core, array type, json type"
export PACKAGE_PROJECT_URL="https://github.com/beginor/nhibernate-extensions"

PROJECTS=( \
  "src/NHibernate.Extensions.NetCore/NHibernate.Extensions.NetCore.csproj" \
  "src/NHibernate.Extensions.Npgsql/NHibernate.Extensions.Npgsql.csproj" \
  "src/NHibernate.Extensions.Sqlite/NHibernate.Extensions.Sqlite.csproj" \
)

for PROJ in "${PROJECTS[@]}"
do
  echo "packing $PROJ"
  dotnet pack $PROJ -c Release --output ./nupkgs/
done

dotnet nuget push ./nupkgs/*.nupkg -s nuget.org -k $(cat ~/.nuget/github.txt) \
  --skip-duplicate

rm -rf ./nupkgs
