#!/bin/bash -e
export PACKAGE_VERSION="9.0.5"
export PACKAGE_RELEASE_NOTES="Rename NHibernate.NetCore to NHibernate.Extensions.NetCore"
export PACKAGE_TAGS="nhibernate, npgsql, dotnet, dotnet core, array type, json type"
export PACKAGE_PROJECT_URL="https://github.com/beginor/nhibernate-extensions"

dotnet pack src/NHibernate.Extensions.NetCore/NHibernate.Extensions.NetCore.csproj -c Release
dotnet nuget push src/NHibernate.Extensions.NetCore/bin/Release/NHibernate.Extensions.NetCore.$PACKAGE_VERSION.nupkg -s nuget.org -k $(cat ~/.nuget/key.txt)
rm -rf src/NHibernate.Extensions.NetCore/bin/Release

dotnet pack src/NHibernate.Extensions.Npgsql/NHibernate.Extensions.Npgsql.csproj -c Release
dotnet nuget push src/NHibernate.Extensions.Npgsql/bin/Release/NHibernate.Extensions.Npgsql.$PACKAGE_VERSION.nupkg -s nuget.org -k $(cat ~/.nuget/key.txt)
rm -rf src/NHibernate.Extensions.Npgsql/bin/Release

dotnet pack src/NHibernate.Extensions.Sqlite/NHibernate.Extensions.Sqlite.csproj -c Release
dotnet nuget push src/NHibernate.Extensions.Sqlite/bin/Release/NHibernate.Extensions.Sqlite.$PACKAGE_VERSION.nupkg -s nuget.org -k $(cat ~/.nuget/key.txt)
rm -rf src/NHibernate.Extensions.Sqlite/bin/Release
