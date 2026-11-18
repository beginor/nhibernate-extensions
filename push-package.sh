#!/bin/bash -e
# Local packing helper. Publishing is done by .github/workflows/publish-nuget.yml
# through nuget.org Trusted Publishing (OIDC), so no API key is needed here.
cd "$(dirname "$0")"
source ./release.env

PROJECTS=( \
  "src/NHibernate.Extensions.NetCore/NHibernate.Extensions.NetCore.csproj" \
  "src/NHibernate.Extensions.Npgsql/NHibernate.Extensions.Npgsql.csproj" \
  "src/NHibernate.Extensions.Pgvector/NHibernate.Extensions.Pgvector.csproj" \
  "src/NHibernate.Extensions.Sqlite/NHibernate.Extensions.Sqlite.csproj" \
)

for PROJ in "${PROJECTS[@]}"
do
  echo "packing $PROJ"
  dotnet pack $PROJ -c Release --output ./nupkgs/
done

echo "packages are in ./nupkgs/, push a tag to publish them:"
echo "  git tag $PACKAGE_VERSION && git push origin $PACKAGE_VERSION"
