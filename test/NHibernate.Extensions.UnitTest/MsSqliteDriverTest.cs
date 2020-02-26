using System;
using System.Data;
using Microsoft.Data.Sqlite;
using NHibernate.Cfg;
using NHibernate.Extensions.UnitTest.TestDb;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NUnit.Framework;

namespace NHibernate.Extensions.UnitTest {

    [TestFixture]
    public class MsSqliteDriverTest {

        private ISessionFactory sessionFactory;

        public MsSqliteDriverTest() {
            // try {
            //     var configuration = new Configuration();
            //     configuration.Configure("hibernate.sqlite.config");
            //     var mapper = new ModelMapper();
            //     mapper.AddMapping<AuthorMappingSqlite>();
            //     var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            //     configuration.AddMapping(mapping);
            //     sessionFactory = configuration.BuildSessionFactory();
            // }
            // catch (Exception ex) {
            //     Console.WriteLine(ex);
            // }
        }

        [Test]
        public void _00_CanQuerySqlite() {
            var builder = new SqliteConnectionStringBuilder();
            builder.DataSource = "/Users/zhang/Projects/nhibernate/nhibernate-extensions/test/NHibernate.Extensions.UnitTest/test_db.db3";
            builder.ForeignKeys = true;
            var connStr = builder.ToString();
            var conn = new SqliteConnection(connStr);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "select * from main.authors";
            var reader = cmd.ExecuteReader();
            while (reader.Read()) {
                Console.WriteLine($"{reader.GetInt32("id")}， {reader.GetString("name")}");
            }
        }

        [Test]
        public void _01_CanBuildSessionFactory() {
            Assert.IsNotNull(sessionFactory);
        }

    }

    public class AuthorMappingSqlite : ClassMapping<Author> {

        public AuthorMappingSqlite() {
            Schema("main");
            Table("authors");
            Id(e => e.AuthorId, m => {
                m.Column("id");
                m.Type(NHibernateUtil.Int32);
                m.Generator(Generators.Identity);
            });
            Property(e => e.Name, m => {
                m.Column("name");
                m.Type(NHibernateUtil.String);
                m.Length(16);
                m.NotNullable(true);
            });
        }

    }

}
