using NHibernate.Extensions.UnitTest.TestDb;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class InheritanceMappingTest : BaseTest {

    [Test]
    public void _00_CanInitData() {
        using var session = TestDbSessionFactory.OpenSession();
        var query = session.Query<BaseResource>();
        var data = query.ToList();
        if (data.Count == 0) {
            var api = new DataApi {
                Name = "test api",
                Statement = "select * from public.data_apis",
                Parameters = "a, b"
            };
            session.Save(api);
            var slpk = new Slpk {
                Name = "test slpk",
                Path = "c:/test/"
            };
            session.Save(slpk);
            session.Flush();
            session.Clear();
        }
    }

    [Test]
    public void _01_CanQueryParent() {
        using var session = TestDbSessionFactory.OpenSession();
        var query = session.Query<BaseResource>().Select(e => new BaseResource {
            Id = e.Id,
            Name = e.Name,
            Type = e.Type
        });
        var data = query.ToList();
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(data));
    }

    [Test]
    public void _02_CanQueryChild() {
        using var session = TestDbSessionFactory.OpenSession();
        var query = session.Query<DataApi>();
        var data = query.ToList();
        Console.WriteLine(data.Count);
    }

}
