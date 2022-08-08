#!/bin/bash -e
PACKAGE_VERSION="6.0.10"
dotnet pack src/NHibernate.NetCore/NHibernate.NetCore.csproj -c Release
dotnet nuget push src/NHibernate.NetCore/bin/Release/NHibernate.NetCore.$PACKAGE_VERSION.nupkg -s nuget.org -k $(cat ~/.nuget/key.txt)
rm -rf src/NHibernate.NetCore/bin/Release

dotnet pack src/NHibernate.Extensions.NpgSql/NHibernate.Extensions.NpgSql.csproj -c Release
dotnet nuget push src/NHibernate.Extensions.NpgSql/bin/Release/NHibernate.Extensions.NpgSql.$PACKAGE_VERSION.nupkg -s nuget.org -k $(cat ~/.nuget/key.txt)
rm -rf src/NHibernate.Extensions.NpgSql/bin/Release

dotnet pack src/NHibernate.Extensions.Sqlite/NHibernate.Extensions.Sqlite.csproj -c Release
dotnet nuget push src/NHibernate.Extensions.Sqlite/bin/Release/NHibernate.Extensions.Sqlite.$PACKAGE_VERSION.nupkg -s nuget.org -k $(cat ~/.nuget/key.txt)
rm -rf src/NHibernate.Extensions.Sqlite/bin/Release
