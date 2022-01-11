using System.Data;
using Microsoft.Data.Sqlite;
using NHibernate.Cfg;
using NHibernate.Extensions.UnitTest.TestDb;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Tool.hbm2ddl;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class SqliteDriverTest {

    private ISessionFactory sessionFactory;

    public SqliteDriverTest() {
        var configuration = CreateConfiguration();
        sessionFactory = configuration.BuildSessionFactory();
    }

    private static Configuration CreateConfiguration() {
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
        Assert.IsNotNull(sessionFactory);
        using (var session = sessionFactory.OpenSession()) {
            var authors = session.Query<Author>()
                .Where(a => a.AuthorId > 0)
                .ToList();
            Assert.IsNotEmpty(authors);
            var count = session.Query<Author>().LongCount(a => a.AuthorId > 0);
            Assert.GreaterOrEqual(count, 0);
        }
    }

    [Test]
    public void _02_CanSaveDelete() {
        using (var session = sessionFactory.OpenSession()) {
            var author = new Author {
                Name = "Unit Test"
            };
            session.Save(author);
            session.Flush();
            Assert.Greater(author.AuthorId, 0);
            session.Delete(author);
            session.Flush();
        }
    }

    [Test]
    public void _03_CanDoschemaExport() {
        var exporter = new SchemaExport(CreateConfiguration());
        exporter.Execute(true, false, false);
    }

}

public class AuthorMappingSqlite : ClassMapping<Author> {

    public AuthorMappingSqlite() {
        // Schema("main");
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
        Bag(p => p.Books, map => {
            map.Key(k => k.Column("author_id"));
            map.Inverse(true);
            map.Cascade(Cascade.DeleteOrphans);
        }, c => {
            c.OneToMany();
        });
    }

}

public class BookMappingSqlite : ClassMapping<Book> {

    public BookMappingSqlite() {
        Table("books");
        Id(e => e.BookId, map => {
            map.Column("id");
            map.Type(NHibernateUtil.Int32);
            map.Generator(Generators.Identity);
        });
        Property(p => p.Title, map => {
            map.Column("title");
            map.Type(NHibernateUtil.String);
        });
        ManyToOne(p => p.Author, map => {
            map.Column("author_id");
            map.Fetch(FetchKind.Join);
            map.ForeignKey("fk_books_author_id");
        });
    }

}
