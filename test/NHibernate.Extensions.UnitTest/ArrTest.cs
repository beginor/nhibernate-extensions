using NHibernate.Linq;

using NHibernate.Extensions.NpgSql;
using NHibernate.Extensions.UnitTest.TestDb;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class ArrTest : BaseTest {

    [OneTimeSetUp]
    public override void OneTimeSetUp() {
        base.OneTimeSetUp();
        using var session = OpenTestDbSession();
        var entity = new ArrTestEntity {
            IntArr = [1, 2, 3],
            StrArr = ["a", "b", "c"]
        };
        session.Save(entity);
        entity = new ArrTestEntity {
            IntArr = [4, 5, 6],
            StrArr = ["d", "e", "f"]
        };
        session.Save(entity);
        session.Flush();
        session.Clear();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() {
        using var session = OpenTestDbSession();
        var query = session.CreateSQLQuery("truncate table public.arr_test");
        query.ExecuteUpdate();
    }

    [Test]
    public async Task _01_Can_QueryArrayEntity() {
        Assert.That(TestDbSessionFactory, Is.Not.Null);

        using var session = TestDbSessionFactory.OpenSession();
        var entities = await session.Query<ArrTestEntity>().ToListAsync();
        Assert.That(entities, Is.Not.Null);
        Assert.That(entities.Count, Is.GreaterThan(0));
    }

    [Test]
    public void _01_CanQueryArayContainsWithHql() {
        using var session = TestDbSessionFactory.OpenSession();
        var num = 1;
        var query1 = session.CreateQuery(
            "from ArrTestEntity e where array_contains(e.IntArr, :num)"
        );
        query1.SetParameter("num", num, NHibernateUtil.Int32);
        var data1 = query1.List<ArrTestEntity>();
        Assert.That(data1, Is.Not.Empty);

        var str = "a";
        var query2 = session.CreateQuery(
            "from ArrTestEntity e where array_contains(e.StrArr, :str)"
        );
        query2.SetParameter("str", str, NHibernateUtil.String);
        var data2 = query2.List<ArrTestEntity>();
        Assert.That(data2, Is.Not.Empty);

        var query3 = session.CreateQuery(
            "from ArrTestEntity e where array_contains(e.StrArr, :str) and array_contains(e.IntArr, :num)"
        );
        query3.SetParameter("str", str, NHibernateUtil.String);
        query3.SetParameter("num", num, NHibernateUtil.Int32);
        var data3 = query3.List<ArrTestEntity>();
        Assert.That(data3, Is.Not.Empty);
    }

    [Test]
    public void _03_CanQueryArrayContainsWithLinq() {
        using var session = TestDbSessionFactory.OpenSession();
        var num = 1;
        var query1 = session.Query<ArrTestEntity>().Where(
            x => x.IntArr.ArrayContains(num)
        );
        var data1 = query1.ToList();
        Assert.That(data1, Is.Not.Empty);

        var str = "a";
        var query2 = session.Query<ArrTestEntity>().Where(
            x => x.StrArr.ArrayContains(str)
        );
        var data2 = query2.ToList();
        Assert.That(data2, Is.Not.Empty);

        var query3 = session.Query<ArrTestEntity>().Where(
            x => x.IntArr.ArrayContains(num) && x.StrArr.ArrayContains(str)
        );
        var data3 = query3.ToList();
        Assert.That(data3, Is.Not.Empty);
    }

    [Test]
    public void _04_CanQueryArrayIntersects() {
        using var session = TestDbSessionFactory.OpenSession();

        int[] intArr = [2, 3];
        var query = session.Query<ArrTestEntity>()
        .Where(
            e => e.IntArr.Intersect(intArr).Any()
        );
        var data = query.ToList();
        Assert.That(data, Is.Not.Null);
        Assert.That(data.Count, Is.GreaterThan(0));
    }

}
