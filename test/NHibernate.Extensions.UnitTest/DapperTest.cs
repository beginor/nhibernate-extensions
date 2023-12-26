using System.Data;
using System.Text.Json;
using Dapper;
using NUnit.Framework.Legacy;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class DapperTest : BaseTest {

    [OneTimeSetUp]
    public override void OneTimeSetUp() {
        base.OneTimeSetUp();
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
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
            ClassicAssert.Greater(authors.Count(), 0);

            authors = conn.Query<AuthorEntity>(
                "select * from public.authors where authorid = any(@Ids)",
                new {
                    Ids = new [] { 1, 2, 3 } .AsEnumerable()
                }
            );
            ClassicAssert.LessOrEqual(authors.Count(), 3);
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
            ClassicAssert.NotNull(entity);

            var updated = conn.Execute(
                "update public.test_table set update_time = @updateTime where id = @id",
                new { id = entity.Id, updateTime = DateTime.Now }
            );
            ClassicAssert.AreEqual(1, updated);

        }
    }

}

public class AuthorEntity {
    public int AuthorId { get; set; }
    public string Name { get; set; }
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
