# Some useful extensions for NHibernate

Some useful extensions for NHibernate used in my projects.

## NHibernate.Extensions.NpgSql

Extended PostgreSQL driver for NHibernate, with support of:

- Type of `json` and `jsonb`
- Array type of `boolean`, `double`, `float (real)`, `int`, `short (small int)`,
  `long (big int)` and `string (character)`；

## NHibernate.NetCore

Extensions for NHibernate when used in dotnet core:

- Use `Microsoft.Extensions.Logging.ILoggerFactory` as NHibernate's logger factory;
- Service Collection Extension of `AddHibernate` method for integration with `IServiceProvider`,

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
                var result = await session.Query<Sample>().ToList();
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
            var result = await session.Query<Sample>().ToList();
            return result;
        }
        catch (Exception ex) {
            return StatusCode(500, ex.Message);
        }
    }
}
```

> `ISessionFactory`/`ISession` is registerd as scoped, do not need to dispose it by hand.
