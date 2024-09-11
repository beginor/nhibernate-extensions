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
    public void _01_CanQueryWithHql() {
        using var session = TestDbSessionFactory.OpenSession();
        var num = 3;
        var hql = "from ArrTestEntity e where array_contains(e.IntArr, :num)";
        var query = session.CreateQuery(hql);
        query.SetParameter("num", num, NHibernateUtil.Int32);
        var data = query.List<ArrTestEntity>();
        Assert.That(data, Is.Not.Null);
        Assert.That(data.Count, Is.GreaterThan(0));
    }

    [Test]
    public void _03_CanQueryArrayContains() {
        using var session = TestDbSessionFactory.OpenSession();
        var num = 6;
        var query = session.Query<ArrTestEntity>().Where(
            x => x.IntArr.ArrayContains(num)
        );
        var data = query.ToList();
        Assert.That(data, Is.Not.Null);
        Assert.That(data.Count, Is.GreaterThan(0));

        var str = "a";
        query = session.Query<ArrTestEntity>().Where(
            x => x.StrArr.ArrayContains(str)
        );
        data = query.ToList();
        Assert.That(data, Is.Not.Null);
        Assert.That(data.Count, Is.GreaterThan(0));
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
