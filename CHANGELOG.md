# Changelogs for NHibernate.Extensions

## 9.0.5

- Rename `NHibernate.NetCore` to `NHibernate.Extensions.NetCore`;

## 9.0.4

- Update to .NET 9.0.7;
- Update packages;
- manage package versions centrally.

## 9.0.3

- Update to .NET 9.0.5;
- Update packages;

## 9.0.2

- Update to .NET 9.0.3;
- Update Npgsql to 9.0.3;
- Update other packages;
- New solution format (slnx);

## 9.0.1

- Update to .NET 9.0.2;
- Update packages;

## 9.0.0

- Update to .NET 9.0.0;
- Update packages;
- Remove obsoleted types;
- Move custom types to namespace `NHibernate.Extensions.Npgsql.UserTypes`;

## 8.0.14

- Update to .NET 8.0.10;

## 8.0.13

- Rename `NpgSql` to `Npgsql`;
- Mark array types inherits from `ArrayType<T>` as obsolete, please use `ArrayType<T>`;
- Mark `SetCustomParameter` `AddCustomScalar` as obsolete, no need to call these methods;
- Obsoleted types and methods will be remove from `9.0.x` .

## 8.0.12

- Optimize array extension methods;
- Add user type aliases;

## 8.0.11

- Add `array_contains` and `array_intersects` extension methods to hql query;
- Add `ArrayContains` and `ArrayIntersects` extension method to linq query;

Please refer [ArrTest](https://github.com/beginor/nhibernate-extensions/blob/master/test/NHibernate.Extensions.UnitTest/ArrTest.cs) for more details.

## 8.0.10

- Update Npgsql to 8.0.4;

## 8.0.9

- Map `StringArrayType` to `NpgsqlDbType.Array | NpgsqlDbType.Varchar`;

## 8.0.8

- Update NHibernate to 5.5.2;

## 8.0.7

- Update Npgsql to 8.0.3;

## 8.0.6

- Update NHibernate to 5.5.1;
- Refactor to use keyed service;

## 8.0.5

- Fix json types with Npgsql 8.x release.

## 8.0.4

- Update Npgsql to 8.0.2;

## 8.0.3

- add `AddCustomScalar` and `SetCustomParameter` extension method for `ISQLQuery`;

## 8.0.2

- Add NumericArray, thanks to [Stephan](https://github.com/stephanstapel) !

## 8.0.1

- Update NHibernate to 5.5.0;
- Update Npgsql to 8.0.1;
- Update NUnit to 4.0.0;
- Other nuget package updates.

## 8.0.0

- Update to .NET 8.0.0;
- Update Npgsql to 8.0.0-rc.2;

## 7.0.9

- Update Npgsql to 7.0.6;
- Update Microsoft.Data.Sqlite.Core to 7.0.11;

## 7.0.8

- Update NHibernate to 5.4.6;
- Update Microsoft.NET.Test.Sdk to 17.7.2;
- Update SQLitePCLRaw.bundle_e_sqlite3 to 2.1.6;

## 7.0.7

- Update to .NET 7.0.10;
- Update NHibernate to 5.4.5;
- Update Dapper to 2.0.151;
- Update Microsoft.Data.Sqlite.Core to 7.0.10;
- Update Microsoft.NET.Test.Sdk to 17.6.3;

## 7.0.6

- Update NHibernate to 5.4.4;
- Update to .NET 7.0.9;

## 7.0.5

- Update to .NET 7.0.8;
- Update NHibernate to 5.4.3;
- Update Dapper to 2.0.143;
- Update Microsoft.NET.Test.Sdk to 17.6.3;

## 7.0.4

- Update to .NET 7.0.7;
- Update Npgsql to 7.0.4;

## 7.0.3

- Update to .NET 7.0.4;
- Update Hibernate to 5.4.2;

## 7.0.2

- Update to .NET 7.0.2;
- Update NHibernate to 5.4.1;

## 7.0.1

- Update to .NET 7.0.1;
- Update Npgsql to 7.0.1;

## 7.0.0

- Update to .NET 7.0.0;
