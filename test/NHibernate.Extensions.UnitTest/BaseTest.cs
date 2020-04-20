using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Extensions.UnitTest.DvdRental;
using NHibernate.Extensions.UnitTest.TestDb;
using NHibernate.Linq;
using NHibernate.Mapping.Attributes;
using NHibernate.NetCore;
using NHibernate.Tool.hbm2ddl;
using Npgsql;
using NUnit.Framework;

namespace NHibernate.Extensions.UnitTest {

    public class BaseTest {

        protected IServiceProvider ServiceProvider { get; private set; }

        protected ISessionFactory TestDbSessionFactory => ServiceProvider.GetSessionFactory();

        protected ISessionFactory DvdRentalSessionFactory => ServiceProvider.GetSessionFactory("dvd_rental");

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
            // add dvd_rental
            var dvdRental = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "dvd_rental.config"
            );
            var dvdRentalCfg = new Configuration();
            dvdRentalCfg.Configure(dvdRental);
            dvdRentalCfg.AddXml(xml);
            services.AddHibernate("dvd_rental", dvdRentalCfg);
            // build service provider
            ServiceProvider = services.BuildServiceProvider();
        }

        [Test]
        public void _01_CanResolveSessionFactories() {
            Assert.NotNull(ServiceProvider);
            Assert.NotNull(TestDbSessionFactory);
            Assert.NotNull(DvdRentalSessionFactory);
        }

        [Test]
        public async Task _02_CanQueryTestDb() {
            using (var session = TestDbSessionFactory.OpenSession()) {
                var author = await session.Query<Author>().FirstOrDefaultAsync();
                Assert.NotNull(author);
            }
        }

        [Test]
        public async Task _03_CanQueryDvdRental() {
            using (var session = DvdRentalSessionFactory.OpenSession()) {
                var actor = await session.Query<Actor>().FirstOrDefaultAsync();
                Assert.NotNull(actor);
            }
        }

        [Test]
        public void _04_CanDoSchemaExport() {
            var export = new SchemaExport(
                ServiceProvider.GetService<Configuration>()
            );
            export.Execute(true, false, false);
        }

    }

}
