using System.Data;
using System.Data.Common;
using System.Text.Json;
using Dapper;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class DapperTest : BaseTest {

    [OneTimeSetUp]
    public override void OneTimeSetUp() {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        base.OneTimeSetUp();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() {
    }

    [Test]
    public void CanQueryAuthors() {
        using (var session = TestDbSessionFactory.OpenSession()) {
            var conn = session.Connection;
            var authors = conn.Query<AuthorEntity>(
                "select * from public.authors"
            );
            Assert.That(authors.Count(), Is.GreaterThanOrEqualTo(0));

            authors = conn.Query<AuthorEntity>(
                "select * from public.authors where authors.id = any(@Ids)",
                new {
                    Ids = new [] { 1, 2, 3 } .AsEnumerable()
                }
            );
            Assert.That(authors.Count(), Is.LessThanOrEqualTo(3));
        }
    }

    [Test]
    public void CanUseTypeHandler() {
        using (var session = TestDbSessionFactory.OpenSession()) {
            var conn = session.Connection;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new DateTimeHandler());
            var entity = conn.Query<TestTableEntity>(
                "select * from public.test_table order by update_time limit 1"
            ).FirstOrDefault();
            Assert.That(entity, Is.Not.Null);

            var updated = conn.Execute(
                "update public.test_table set update_time = @updateTime where id = @id",
                new { id = entity.Id, updateTime = DateTime.Now }
            );
            Assert.That(updated, Is.EqualTo(1));
        }
    }

    [Test]
    public void CanQueryActor() {
        using var session = TestDbSessionFactory.OpenSession();
        var conn = session.Connection;
        var sql = "select actor_id, first_name, last_name, last_update from public.actor where last_name = @lastName";
        var reader = (DbDataReader)conn.ExecuteReader(sql, new { lastName = "zhang" });
        while (reader.Read()) {
            var id = reader.GetInt64("actor_id");
            var firstName = reader.GetString("first_name");
            var lastName = reader.GetString("last_name");
            var lastUpdate = reader.GetDateTime("last_update");
            Console.WriteLine($"id: {id}, first_name: {firstName}, last_name: {lastName}, last_update: {lastUpdate}");
        }
    }

}

public class AuthorEntity {
    public int AuthorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime LastUpdate { get; set; }
}

public class BookEntity {
    public int BookId { get; set; }
    public string Title { get; set; }
    public int AuthorId { get; set; }
}

public class TestTableEntity {
    public virtual long Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string[] Tags { get; set; }
    public virtual JsonElement JsonField { get; set; }
    public virtual JsonElement JsonbField { get; set; }
    public virtual DateTime? UpdateTime { get; set; }
    public virtual short[] Int16Arr { get; set; }
    public virtual int[] Int32Arr { get; set; }
    public virtual long[] Int64Arr { get; set; }
    public virtual float[] FloatArr { get; set; }
    public virtual double[] DoubleArr { get; set; }
    public virtual bool[] BooleanArr { get; set; }
}

public class DateTimeHandler : Dapper.SqlMapper.TypeHandler<DateTime> {

    public override void SetValue(IDbDataParameter parameter, DateTime value) {
        Console.WriteLine("set value!");
        parameter.Value = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }

    public override DateTime Parse(object value) {
        Console.WriteLine("parse!");
        var localTime = (DateTime) value;
        return DateTime.SpecifyKind(localTime, DateTimeKind.Utc);
    }

}
