using System.Linq;
using System.Linq.Expressions;
using NHibernate.Linq;

using NHibernate.Extensions.Npgsql;
using NHibernate.Extensions.Npgsql.UserTypes;
using NHibernate.Extensions.UnitTest.TestDb;
using NHibernate.Type;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class ArrTest : BaseTest {

    private readonly IList<long> entityIdList = new List<long>();

    public ArrTest() {
        Console.WriteLine("ArrTest::Constructor");
    }

    [OneTimeSetUp]
    public override void OneTimeSetUp() {
        base.OneTimeSetUp();
        using var session = OpenTestDbSession();
        var entity = new ArrTestEntity {
            IntArr = [1, 2, 3],
            StrArr = ["a", "b", "c"]
        };
        session.Save(entity);
        Assert.That(entity.Id, Is.GreaterThan(0));
        entityIdList.Add(entity.Id);
        entity = new ArrTestEntity {
            IntArr = [4, 5, 6],
            StrArr = ["d", "e", "f"]
        };
        session.Save(entity);
        Assert.That(entity.Id, Is.GreaterThan(0));
        entityIdList.Add(entity.Id);
        session.Flush();
        session.Clear();
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(entityIdList));
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() {
        using var session = OpenTestDbSession();
        var query = session.CreateSQLQuery("truncate table public.arr_test");
        query.ExecuteUpdate();
        entityIdList.Clear();
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
            $"from ArrTestEntity e where array_contains(e.IntArr, :{nameof(num)})"
        );
        query1.SetParameter(nameof(num), num, NHibernateUtil.Int32);
        var data1 = query1.List<ArrTestEntity>();
        Assert.That(data1, Is.Not.Empty);

        var str = "a";
        var query2 = session.CreateQuery(
            $"from ArrTestEntity e where array_contains(e.StrArr, :{nameof(str)})"
        );
        query2.SetParameter(nameof(str), str, NHibernateUtil.String);
        var data2 = query2.List<ArrTestEntity>();
        Assert.That(data2, Is.Not.Empty);

        var query3 = session.CreateQuery(
            $"from ArrTestEntity e where array_contains(e.StrArr, :{nameof(str)})"
            + $" and array_contains(e.IntArr, :{nameof(num)})"
        );
        query3.SetParameter(nameof(str), str, NHibernateUtil.String);
        query3.SetParameter(nameof(num), num, NHibernateUtil.Int32);
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
    public void _04_CanQueryArrayIntersectsWithHql() {
        using var session = TestDbSessionFactory.OpenSession();

        string[] strArr = ["a", "c"];
        var query1 = session.CreateQuery(
            $"from ArrTestEntity e where array_intersects(e.StrArr, :{nameof(strArr)})"
        );
        query1.SetParameter(nameof(strArr), strArr, NHibernateUtil.Custom(typeof(ArrayType<string>)));
        var data1 = query1.List<ArrTestEntity>();
        Assert.That(data1, Is.Not.Empty);

        int[] intArr = [1, 3];
        var query2 = session.CreateQuery(
            $"from ArrTestEntity e where array_intersects(e.IntArr, :{nameof(intArr)})"
        );
        query2.SetParameter(nameof(intArr), intArr, NHibernateUtil.Custom(typeof(ArrayType<int>)));
        var data2 = query1.List<ArrTestEntity>();
        Assert.That(data2, Is.Not.Empty);

        var query3 = session.CreateQuery(
            $"from ArrTestEntity e where array_intersects(e.StrArr, :{nameof(strArr)})"
            + $" and array_intersects(e.IntArr, :{nameof(intArr)})"
        );
        query3.SetParameter(nameof(strArr), strArr, NHibernateUtil.Custom(typeof(ArrayType<string>)));
        query3.SetParameter(nameof(intArr), intArr, NHibernateUtil.Custom(typeof(ArrayType<int>)));
        var data3 = query3.List<ArrTestEntity>();
        Assert.That(data3, Is.Not.Empty);
    }

    [Test]
    public async Task _05_CanQueryArrayIntersectsWithLinq() {
        using var session = TestDbSessionFactory.OpenSession();

        string[] strArr = ["a", "c"];
        var query1 = session.Query<ArrTestEntity>().Where(
            x => x.StrArr.ArrayIntersects(strArr)
        );
        var data1 = await query1.ToListAsync();
        Assert.That(data1, Is.Not.Empty);

        int[] intArr = [1, 3];
        var query2 = session.Query<ArrTestEntity>().Where(
            x => x.IntArr.ArrayIntersects(intArr)
        );
        var data2 = await query2.ToListAsync();
        Assert.That(data2, Is.Not.Empty);

        var query3 = session.Query<ArrTestEntity>().Where(
            x => x.StrArr.ArrayIntersects(strArr) && x.IntArr.ArrayIntersects(intArr)
        );
        var data3 = await query3.ToListAsync();
        Assert.That(data3, Is.Not.Empty);
    }

    [Test]
    public void _06_CanQueryContains() {
        using var session = TestDbSessionFactory.OpenSession();
        var idList = entityIdList.ToArray();
        var query = session.Query<ArrTestEntity>().Where(
            x => idList.Contains(x.Id)
        );
        var data = query.ToList();
        Assert.That(data, Is.Not.Empty);

        var idArr = idList.ToArray();
        var query2 = session.Query<ArrTestEntity>().Where(
            x => idArr.ArrayContains(x.Id)
        );
        var data2 = query2.ToList();
        Assert.That(data2, Is.Not.Empty);
    }

    [Test]
    public void _07_CanQueryWithExpression() {
        using var session = TestDbSessionFactory.OpenSession();
        var query = session.Query<ArrTestEntity>();
        Expression<Func<ArrTestEntity, bool>> expr1 = x => x.Id == 0;
        Expression<Func<ArrTestEntity, bool>> expr2 = x => x.Id == 2;
        var invokedExpr = Expression.Invoke (expr2, expr1.Parameters);
        var pred = Expression.Lambda<Func<ArrTestEntity, bool>>(Expression.OrElse (expr1.Body, invokedExpr), expr1.Parameters);
        query = query.Where(pred);
        var data = query.ToList();
        Assert.That(data, Is.Empty);
    }

}
