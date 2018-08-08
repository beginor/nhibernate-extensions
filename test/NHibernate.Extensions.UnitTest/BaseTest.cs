using System;
using NHibernate.Cfg;
using Npgsql;

namespace NHibernate.Extensions.UnitTest {

    public abstract class BaseTest {

        protected ISessionFactory factory { get; private set; }

        public BaseTest() {
            NpgsqlConnection.GlobalTypeMapper.UseJsonNet();
            var configuration = new Configuration();
            var configFile = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "hibernate.config"
            );
            configuration.Configure(configFile);
            // HbmSerializer.Default.Validate = true;
            // configuration.AddInputStream(
            //     HbmSerializer.Default.Serialize(
            //         typeof(SnowFlakeTestEntity).Assembly
            //     )
            // );
            factory = configuration.BuildSessionFactory();
        }

    }

}
