using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NHibernate.Cfg;
using NHibernate.Extensions.NetCore;
using NHibernate.Linq;
using NHibernate.Mapping.Attributes;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Mapping.ByCode;

using NHibernate.Extensions.UnitTest.Sqlite;
using NHibernate.Extensions.UnitTest.TestDb;
using Author = NHibernate.Extensions.UnitTest.TestDb.Author;
using MsILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace NHibernate.Extensions.UnitTest;

public class BaseTest {

    protected IServiceProvider ServiceProvider { get; private set; }

    protected ISessionFactory TestDbSessionFactory => ServiceProvider.GetSessionFactory();
    protected ISessionFactory SqliteSessionFactory => ServiceProvider.GetSessionFactory("sqlite");

    [OneTimeSetUp]
    public virtual void OneTimeSetUp() {
        // global setup
        // NpgsqlConnection.GlobalTypeMapper.UseJsonNet();
        var services = new ServiceCollection();
        services.AddHibernate(CreateTestDbConfiguration());
        services.AddHibernate("sqlite", CreateSqliteConfiguration());
        services.AddLogging(builder => {
            builder.AddConsole();
        });
        // build service provider
        ServiceProvider = services.BuildServiceProvider();
        var loggerFactory = ServiceProvider.GetRequiredService<MsILoggerFactory>();
        loggerFactory.UseAsHibernateLoggerFactory();
    }

    private Configuration CreateTestDbConfiguration() {
        // add default config
        var defaultConfigFile = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "hibernate.config"
        );
        var config = new Configuration();
        config.Configure(defaultConfigFile);
        // use default attr serializer;
        var serializer = HbmSerializer.Default;
        var xmlStream = serializer.Serialize(
            typeof(SnowFlakeTestEntity).Assembly
        );
        // ensure serialize error is empty;
        var err = serializer.Error.ToString();
        Assert.That(err, Is.Empty);
        // add to config
        using var reader = new StreamReader(xmlStream);
        var xml = reader.ReadToEnd();
        Console.WriteLine(xml);
        config.AddXml(xml);
        return config;
    }

    private Configuration CreateSqliteConfiguration() {
        var configuration = new Configuration();
        configuration.Configure("hibernate.sqlite.config");
        var mapper = new ModelMapper();
        mapper.AddMapping<AuthorMappingSqlite>();
        mapper.AddMapping<BookMappingSqlite>();
        var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
        configuration.AddMapping(mapping);
        return configuration;
    }

    [Test]
    public void _01_CanResolveSessionFactories() {
        Assert.That(ServiceProvider, Is.Not.Null);
        Assert.That(TestDbSessionFactory, Is.Not.Null);
        Assert.That(SqliteSessionFactory, Is.Not.Null);
    }

    [Test]
    public async Task _02_CanQueryTestDb() {
        using (var session = TestDbSessionFactory.OpenSession()) {
            var author = await session.Query<Author>().FirstOrDefaultAsync();
            // Assert.That(author, Is.Not.Null);
        }
    }

    [Test]
    public void _04_CanDoSchemaExport() {
        var export = new SchemaExport(
            ServiceProvider.GetService<Configuration>()
        );
        export.Execute(true, false, false);
    }

    protected ISession OpenTestDbSession() {
        return TestDbSessionFactory.OpenSession();
    }

}
