using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Extensions.UnitTest.TestDb;
using NHibernate.Linq;
using NHibernate.Mapping.Attributes;
using NHibernate.NetCore;
using NHibernate.Tool.hbm2ddl;

namespace NHibernate.Extensions.UnitTest;

public class BaseTest {

    protected IServiceProvider ServiceProvider { get; private set; }

    protected ISessionFactory TestDbSessionFactory => ServiceProvider.GetSessionFactory();

    [OneTimeSetUp]
    public virtual void OneTimeSetUp() {
        // global setup
        // NpgsqlConnection.GlobalTypeMapper.UseJsonNet();
        var services = new ServiceCollection();
        // add default config
        var defaultConfigFile = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "hibernate.config"
        );
        var defaultCfg = new Configuration();
        defaultCfg.Configure(defaultConfigFile);
        // use default attr serializer;
        var serializer = HbmSerializer.Default;
        var xmlStream = serializer.Serialize(
            typeof(SnowFlakeTestEntity).Assembly
        );
        // ensure serialize error is empty;
        var err = serializer.Error.ToString();
        Assert.IsEmpty(err);
        // add to config
        using var reader = new StreamReader(xmlStream);
        var xml = reader.ReadToEnd();
        Console.WriteLine(xml);
        defaultCfg.AddXml(xml);
        services.AddHibernate(defaultCfg);
        // build service provider
        ServiceProvider = services.BuildServiceProvider();
    }

    [Test]
    public void _01_CanResolveSessionFactories() {
        Assert.NotNull(ServiceProvider);
        Assert.NotNull(TestDbSessionFactory);
    }

    [Test]
    public async Task _02_CanQueryTestDb() {
        using (var session = TestDbSessionFactory.OpenSession()) {
            var author = await session.Query<Author>().FirstOrDefaultAsync();
            // Assert.NotNull(author);
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
