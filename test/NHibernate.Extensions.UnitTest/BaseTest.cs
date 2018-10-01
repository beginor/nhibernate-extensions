using System;
using System.IO;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using Npgsql;
using Xunit;

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
//            var serializer = HbmSerializer.Default;
//            var stream = serializer.Serialize(
//                typeof(BaseTest).Assembly
//            );
//
//            var err = serializer.Error.ToString();
//
//            Assert.Empty(err);
//
//            var reader = new StreamReader(stream);
//            var xml = reader.ReadToEnd();
//            configuration.AddXml(xml);

            factory = configuration.BuildSessionFactory();
        }

    }

}
