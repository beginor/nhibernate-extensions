# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 项目概述

这是一个 .NET 10.0 类库项目，为 NHibernate 提供 PostgreSQL（Npgsql）、Pgvector 和 SQLite 的扩展。包含 4 个可发布的 NuGet 包和 1 个测试项目。

### 包依赖关系

```
NHibernate.Extensions.NetCore   (IServiceCollection 扩展，日志桥接)
NHibernate.Extensions.Npgsql    (PostgreSQL 驱动：json/jsonb、数组类型、ArrayContains/ArrayIntersects)
NHibernate.Extensions.Pgvector---> NHibernate.Extensions.Npgsql  (vector/halfvec/sparsevec 类型，距离查询)
NHibernate.Extensions.Sqlite    (SQLite 驱动，基于 Microsoft.Data.Sqlite.Core)
```

所有包均目标框架 `net10.0`，通过 `Directory.Packages.props` 进行中央包管理，且启用了 `<Nullable>enable</Nullable>`。

### 关键架构模式

- 每个扩展包提供：自定义 `Driver` 类、`Dialect`，以及可选的 `UserTypes` 和 `LinqToHqlGeneratorsRegistry`
- 实体映射：PostgreSQL 实体使用 `NHibernate.Mapping.Attributes`（基于特性，序列化为 XML），SQLite 实体使用 `ModelMapper`（基于代码）
- 测试以 `BaseTest` 为公共基类，通过 `IServiceCollection` 的 `AddHibernate()` 扩展方法注册 PostgreSQL 和 SQLite 两个 `ISessionFactory`
- `BaseTest.OneTimeSetUp()` 每个测试类只运行一次。`BaseTest` 中的 `_02_CanQueryTestDb` 测试会查询 PostgreSQL 的 `public.authors` 表，没有运行中的 PostgreSQL 服务时会失败

## 前置条件

- .NET 10.0 SDK
- PostgreSQL 服务器（大部分测试需要），需包含 `test_db` 数据库、`pgvector` 扩展，以及表结构与 `test/NHibernate.Extensions.UnitTest/TestDb/` 中的实体映射一致
- PostgreSQL 连接字符串位于 `test/NHibernate.Extensions.UnitTest/config/hibernate.config`，按需修改账号密码

## 构建与测试

```bash
# 构建所有项目
dotnet build

# 运行全部测试（没有 PostgreSQL 服务时部分测试会失败）
dotnet test

# 仅运行不依赖 PostgreSQL 的测试
dotnet test --filter "FullyQualifiedName~SqliteTest|FullyQualifiedName~AttrMapTest|FullyQualifiedName~NpgsqlTypeTest"

# 运行指定测试类
dotnet test --filter "FullyQualifiedName~SqliteDriverTest"

# 运行单个测试方法
dotnet test --filter "FullyQualifiedName~SqliteDriverTest._01_CanBuildSessionFactory"
```

## 版本说明

1. 大版本与项目使用的 .net 运行时版本相同， 比如当前发布包的版本是 10.0.1 ，表示当前类库是 .net 10.0 的类库， 进行了第一次更新；
2. 每次更新增加一个小版本，即 0.0.1 ，表示是第几次更新；
3. 每发布一个版本，都要有对应的 git 标签，并推送到远程服务器；

## 打包与发布

采用下面的步骤进行发布：

1. 更新 CHANGELOG.md , 在文件前面加入添加说明更新的内容；
2. 按照 **版本说明** 修改 `push-package.sh` 中版本号变量 `PACKAGE_VERSION`， 根据实际改动的内容修改发布说明变量 `PACKAGE_RELEASE_NOTES`;
3. 执行修改过的脚本 `push-package.sh` ；
4. 对代码打上对应的标签，并推送到远程服务器；示例：
   ```bash
   git tag 10.0.2
   git push origin 10.0.2
   ```
