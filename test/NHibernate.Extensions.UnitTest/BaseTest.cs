using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.NetCore;
using NHibernate.Mapping.Attributes;
using Npgsql;
using NUnit.Framework;

namespace NHibernate.Extensions.UnitTest {

    public abstract class BaseTest {

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

    }

}
