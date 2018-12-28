using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Extensions.UnitTest.DvdRental;
using NHibernate.Extensions.UnitTest.TestDb;
using NHibernate.Linq;
using NHibernate.NetCore;
using NHibernate.Mapping.Attributes;
using Npgsql;
using NUnit.Framework;

namespace NHibernate.Extensions.UnitTest {

    public class BaseTest {

        protected IServiceProvider ServiceProvider { get; private set; }

        protected ISessionFactory TestDbSessionFactory => ServiceProvider.GetSessionFactory();

        protected ISessionFactory DvdRentalSessionFactory => ServiceProvider.GetSessionFactory("dvd_rental");

        [OneTimeSetUp]
        public void OneTimeSetUp() {
            // global setup
            NpgsqlConnection.GlobalTypeMapper.UseJsonNet();
            var services = new ServiceCollection();
            // add default config
            var defaultConfigFile = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "hibernate.config"
            );
            services.AddHibernate(defaultConfigFile);
            // add dvd_rental
            var dvdRental = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "dvd_rental.config"
            );
            services.AddHibernate("dvd_rental", dvdRental);
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

    }

}
