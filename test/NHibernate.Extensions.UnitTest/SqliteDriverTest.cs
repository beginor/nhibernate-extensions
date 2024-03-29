using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework.Legacy;
using NHibernate.Extensions.UnitTest.Sqlite;
using NHibernate.NetCore;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class SqliteDriverTest : BaseTest {

    private ISessionFactory SessionFactory => ServiceProvider.GetSessionFactory("sqlite");

    [Test]
    public void _00_CanQuerySqlite() {
        var builder = new SqliteConnectionStringBuilder();
        builder.DataSource = "./test_db.db3";
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
        ClassicAssert.IsNotNull(SessionFactory);
        using (var session = SessionFactory.OpenSession()) {
            var authors = session.Query<Author>()
                .Where(a => a.AuthorId > 0)
                .ToList();
            ClassicAssert.IsNotEmpty(authors);
            var count = session.Query<Author>().LongCount(a => a.AuthorId > 0);
            ClassicAssert.GreaterOrEqual(count, 0);
        }
    }

    [Test]
    public void _02_CanSaveDelete() {
        using (var session = SessionFactory.OpenSession()) {
            var author = new Author {
                Name = "Unit Test"
            };
            session.Save(author);
            session.Flush();
            ClassicAssert.Greater(author.AuthorId, 0);
            session.Delete(author);
            session.Flush();
        }
    }

    [Test]
    public void _03_CanDoschemaExport() {
        var exporter = new SchemaExport(ServiceProvider.GetRequiredKeyedService<Configuration>("sqlite"));
        exporter.Execute(true, false, false);
    }

}
