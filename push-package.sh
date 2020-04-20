#!/bin/bash -e
dotnet pack src/NHibernate.NetCore/NHibernate.NetCore.csproj -c Release
dotnet nuget push src/NHibernate.NetCore/bin/Release/NHibernate.NetCore.3.1.3.nupkg -s nuget.org -k $(cat ~/.nuget/key.txt)
rm -rf src/NHibernate.NetCore/bin/Release
