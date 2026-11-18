# Some useful extensions for NHibernate

Some useful extensions for NHibernate used in my projects.

All packages target `net10.0`.

| Package | Description |
| --- | --- |
| [NHibernate.Extensions.Npgsql](https://www.nuget.org/packages/NHibernate.Extensions.Npgsql/) | PostgreSQL driver with `json`/`jsonb`, array types and array predicates |
| [NHibernate.Extensions.Pgvector](https://www.nuget.org/packages/NHibernate.Extensions.Pgvector/) | `vector`/`halfvec`/`sparsevec` types and distance queries (based on the Npgsql package) |
| [NHibernate.Extensions.Sqlite](https://www.nuget.org/packages/NHibernate.Extensions.Sqlite/) | NHibernate driver for `Microsoft.Data.Sqlite.Core` |
| [NHibernate.Extensions.NetCore](https://www.nuget.org/packages/NHibernate.Extensions.NetCore/) | `IServiceCollection` integration and logging bridge |

## Configuration

Point the driver, dialect and linq generator registry of your `hibernate.config` to the classes
provided by the package you use:

```xml
<hibernate-configuration xmlns="urn:nhibernate-configuration-2.2">
  <session-factory>
    <property name="connection.connection_string">server=127.0.0.1;port=5432;database=test_db;user id=postgres;password=pgsql;</property>
    <!-- use NpgsqlDriver / NpgsqlDialect if you don't need pgvector types -->
    <property name="connection.driver_class">NHibernate.Extensions.Pgvector.PgvectorDriver,NHibernate.Extensions.Pgvector</property>
    <property name="dialect">NHibernate.Extensions.Pgvector.PgvectorDialect,NHibernate.Extensions.Pgvector</property>
    <property name="linqtohql.generatorsregistry">NHibernate.Extensions.Pgvector.LinqToHqlGeneratorsRegistry,NHibernate.Extensions.Pgvector</property>
  </session-factory>
</hibernate-configuration>
```

## NHibernate.Extensions.Npgsql

- Extended PostgreSQL driver [NpgsqlDriver](https://github.com/beginor/nhibernate-extensions/blob/master/src/NHibernate.Extensions.Npgsql/NpgsqlDriver.cs) for NHibernate:

  - Connections are created from a `NpgsqlDataSource` with `EnableDynamicJson()` enabled, so `JsonElement`, `JsonDocument` and POCO json are handled by Npgsql itself;
  - Both `DbType` and `NpgsqlDbType` are set for parameters of the custom sql types below;

- Extended PostgreSQL dialect [NpgsqlDialect](https://github.com/beginor/nhibernate-extensions/blob/master/src/NHibernate.Extensions.Npgsql/NpgsqlDialect.cs) for schema export of supported types.

- Supported types:

  - `json` and `jsonb` — [JsonType / JsonbType](./src/NHibernate.Extensions.Npgsql/UserTypes/JsonType.cs) map `JsonElement` by default, and any POCO with the generic `JsonType<T>` / `JsonbType<T>` (`T` can also be `JsonDocument`);
  - Array type of `boolean`, `short (small int)`, `int`, `long (big int)`, `float (real)`, `double`, `decimal`, `string (character)`, `DateTime` and `DateTimeOffset`;
  - `timestamp` and `timestamptz` — [TimeStampType](./src/NHibernate.Extensions.Npgsql/UserTypes/TimeStampType.cs) and [TimeStampTzType](./src/NHibernate.Extensions.Npgsql/UserTypes/TimeStampTzType.cs) pin the `NpgsqlDbType` explicitly instead of guessing it from `DateTime.Kind`;

- Add `array_contains(arr, v)` (translated to `:v = any(arr)`) and `array_intersects(a, b)` (translated to `a && b`) functions to hql query;
- Add `ArrayContains` and `ArrayIntersects` extension methods to linq query, translated to the functions above by [ArrayHqlGenerator](./src/NHibernate.Extensions.Npgsql/Generators/ArrayHqlGenerator.cs);

```c#
var strArr = new[] { "a", "b" };
var entities = session.Query<ArrTestEntity>()
    .Where(x => x.IntArr.ArrayContains(3) && x.StrArr.ArrayIntersects(strArr))
    .ToList();
```

Refer [ArrTestEntity](./test/NHibernate.Extensions.UnitTest/TestDb/ArrTestEntity.cs) for mapping, and [ArrTest](./test/NHibernate.Extensions.UnitTest/ArrTest.cs) / [JsonElementTest](./test/NHibernate.Extensions.UnitTest/JsonElementTest.cs) for sample query usage;

## NHibernate.Extensions.Pgvector

- Extended PostgreSQL driver [PgvectorDriver](https://github.com/beginor/nhibernate-extensions/blob/master/src/NHibernate.Extensions.Pgvector/PgvectorDriver.cs) for NHibernate, with support for:
  - Add support of [pgvector](https://github.com/pgvector/pgvector/) with [pgvector-dotnet](https://github.com/pgvector/pgvector-dotnet) driver (`UseVector()` on the data source, and `CREATE EXTENSION IF NOT EXISTS vector` when a connection is created);
  - Add custom user types of `vector`, `halfvec` and `sparsevec`, registered by [PgvectorDialect](https://github.com/beginor/nhibernate-extensions/blob/master/src/NHibernate.Extensions.Pgvector/PgvectorDialect.cs);
  - Sql, Hql and Linq query with distances of pgvector;

Linq sample usages:

```c#
var query = session.Query<ChunkEmbeddingEntity>().Select(x => new {
    x.Id,
    x.Text,
    x.Embedding,
    x.CreateTime,
    Distance = x.Embedding.L1Distance(target)
}).Skip(0).Take(3);
```

Query with Order: 

```c#
var query2 = session.Query<ChunkEmbeddingEntity>()
    .OrderBy(x => x.Embedding.CosineDistance(target))
    .Select(x => new {
        x.Id,
        x.Text,
        x.CreateTime
    }).Skip(0).Take(3);
```

And sql query is also supported:

```c#
using var session = OpenTestDbSession();
var sql = """
    select id, text, created_at, embedding
    from public.chunk_embeddings
    order by (embedding <=> :target)
    limit 3;
""";
var query = session.CreateSQLQuery(sql);
query.AddEntity(typeof(ChunkEmbeddingEntity));
query.SetParameter("target", target);
var data = query.List<ChunkEmbeddingEntity>();
```

Distance functions registered in [PgvectorDialect](./src/NHibernate.Extensions.Pgvector/PgvectorDialect.cs):

| Hql function | pgvector operator | Linq extension method |
| --- | --- | --- |
| `l1_distance` | `<+>` | `L1Distance()` |
| `l2_distance` | `<->` | `L2Distance()` |
| `cosine_distance` | `<=>` | `CosineDistance()` |
| `inner_product` | `<#>` | - |
| `hamming_distance` | `<~>` | - |
| `jaccard_distance` | `<%>` | - |

The linq extension methods are declared in [PgvectorExtensions](./src/NHibernate.Extensions.Pgvector/PgvectorExtensions.cs) and translated by [PgvectorHqlGenerator](./src/NHibernate.Extensions.Pgvector/Generators/PgvectorHqlGenerator.cs); they exist only to be translated into sql, so don't call them on objects in memory. The functions without a linq extension method can be used in hql or sql queries directly.

Refer [ChunkEmbeddingEntity](./test/NHibernate.Extensions.UnitTest/TestDb/ChunkEmbeddingEntity.cs) for mapping, and [VectorTest](./test/NHibernate.Extensions.UnitTest/VectorTest.cs) for sample query usage.

## NHibernate.Extensions.Sqlite

NHibernate driver for the Microsoft.Data.Sqlite.Core data provider for .NET.

In order to use this driver, you must have the nuget package [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/) and [SQLitePCLRaw.bundle_e_sqlite3](https://www.nuget.org/packages/SQLitePCLRaw.bundle_e_sqlite3/) installed for NHibernate to load.

It runs on any platform .NET supports.

The package also provides [SqliteDialect](./src/NHibernate.Extensions.Sqlite/SqliteDialect.cs), which keeps column data types out of the reserved words (`IncludeDataTypesInReservedWords = false`) so schema export and schema comparison do not quote them.

## NHibernate.Extensions.NetCore

Extensions for NHibernate when used in dotnet core:

- Service Collection Extension of `AddHibernate` method for integration with `IServiceProvider`:
  - configure from the default `hibernate.config`, a config file path, an `XmlReader`, an embedded resource (`Assembly` + `resourceName`) or an `Action<Configuration>`;
  - `AddHibernate(cfg, buildSessionFactoryOnRegister)` controls whether `BuildSessionFactory()` runs at registration time (fail fast on mapping errors) or lazily on first resolve;
  - more than one session factory is supported with keyed services: register with `AddHibernate("sqlite", cfg)` and resolve with `GetSessionFactory("sqlite")`;
- `UseAsHibernateLoggerFactory()` to route NHibernate internal logs to `Microsoft.Extensions.Logging`;

Example usage:

```cs
var builder = WebApplication.CreateBuilder(args);

// add NHibernate services, where is your hibernate.config path
builder.Services.AddHibernate(
    Path.Combine(AppContext.BaseDirectory, "hibernate.config")
);
// a second, keyed session factory
builder.Services.AddHibernate(
    "sqlite",
    Path.Combine(AppContext.BaseDirectory, "hibernate.sqlite.config")
);

var app = builder.Build();

// Use the host logger factory as NHibernate logger factory.
app.Services.GetRequiredService<ILoggerFactory>().UseAsHibernateLoggerFactory();

// and get a session factory when needed
var sqliteFactory = app.Services.GetSessionFactory("sqlite");
```

And then use `ISessionFactory` in your controller:

```cs
[Route("api/[controller]")]
public class SamplesController : Controller {

    private ISessionFactory sessionFactory;

    public SamplesController(ISessionFactory sessionFactory) {
        this.sessionFactory = sessionFactory;
    }

    protected override void Dispose(bool disposing) {
        if (disposing) {
            sessionFactory = null;
        }
    }

    [HttpGet("")]
    public async Task<IList<Sample>> GetAll() {
        try {
            using (var session = sessionFactory.OpenSession()) {
                var result = await session.Query<Sample>().ToListAsync();
                return result;
            }
        }
        catch (Exception ex) {
            return StatusCode(500, ex.Message);
        }
    }
}
```

Or just use `ISession` in your controller:

```cs
[Route("api/[controller]")]
public class SamplesController : Controller {

    private ISession session;

    public SamplesController(ISession session) {
        this.session = session;
    }

    protected override void Dispose(bool disposing) {
        if (disposing) {
            session = null;
        }
    }

    [HttpGet("")]
    public async Task<IList<Sample>> GetAll() {
        try {
            var result = await session.Query<Sample>().ToListAsync();
            return result;
        }
        catch (Exception ex) {
            return StatusCode(500, ex.Message);
        }
    }
}
```

- `ISessionFactory` is registered as a singleton service, you should not dispose it;
- `ISession` is registered as a scoped service, so you do not need to dispose it by hand;
