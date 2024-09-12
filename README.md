# Some useful extensions for NHibernate

Some useful extensions for NHibernate used in my projects.

## NHibernate.Extensions.NpgSql

- Extended PostgreSQL driver [NpgSqlDriver](https://github.com/beginor/nhibernate-extensions/blob/master/src/NHibernate.Extensions.NpgSql/NpgSqlDriver.cs) for NHibernate, with support of:

  - Type of `json` and `jsonb`
  - Array type of `boolean`, `double`, `float (real)`, `int`, `short (small int)`, `long (big int)` and `string (character)`；
  - Add `array_contains` and `array_intersects` function to hql query;
  - Add `ArrayContains` and `ArrayIntersects` extension to linq query;

- Extended PostgreSQL dialect [NpgSqlDialect](https://github.com/beginor/nhibernate-extensions/blob/master/src/NHibernate.Extensions.NpgSql/NpgSqlDialect.cs) for schema export of supported types.

## NHibernate.Extensions.Sqlite

NHibernate driver for the Microsoft.Data.Sqlite.Core data provider for .NET.

In order to use this driver, you must have the nuget package [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/) and [SQLitePCLRaw.bundle_e_sqlite3](https://www.nuget.org/packages/SQLitePCLRaw.bundle_e_sqlite3/) installed for NHibernate to load.

It's a .NET Standard 2.0 Driver, Can run on any platform that dotnet core support.

## NHibernate.NetCore

Extensions for NHibernate when used in dotnet core:

- Use `Microsoft.Extensions.Logging.ILoggerFactory` as NHibernate's logger factory;
- Service Collection Extension of `AddHibernate` method for integration with `IServiceProvider`;
- `OrderBy(string propertyName)`, `OrderByDescending(string propertyName)` and
  `AddOrderBy(string propertyName, bool isAsc)` extension methods to `IQueryable<T>`, 
  make fun with dynamic order by.

Example usage:

```cs
public void ConfigureServices(IServiceCollection services) {
    // where is your hibernate.config path
    var path = System.IO.Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "hibernate.config"
    );
    // add NHibernate services;
    services.AddHibernate(path);
}

public void Configure(
    IApplicationBuilder app,
    IHostingEnvironment env,
    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory
) {
    // Use loggerFactory as NHibernate logger factory.
    loggerFactory.UseAsHibernateLoggerFactory();
    /* other code goes here */
}
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

- `ISessionFactory` is registerd as a singleton service, you should not dispose it;
- `ISession` is registerd as a scoped service, so you do not need to dispose it by hand;
