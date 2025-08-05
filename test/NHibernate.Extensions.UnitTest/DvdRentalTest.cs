using NHibernate.Linq;
using NHibernate.Extensions.UnitTest.TestDb;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class DvdRentalTest : BaseTest {

    [OneTimeSetUp]
    public override void OneTimeSetUp() {
        base.OneTimeSetUp();
    }

    [Test]
    public void _01_CanSetupSessionFactory() {
        Assert.That(TestDbSessionFactory, Is.Not.Null);

        var dvdRentalSession = TestDbSessionFactory.OpenSession();
        var connStr = dvdRentalSession.Connection.ConnectionString;
        Console.WriteLine(connStr);
        dvdRentalSession.Close();

        var testDbSession = TestDbSessionFactory.OpenSession();
        var connStr2 = testDbSession.Connection.ConnectionString;
        Console.WriteLine(connStr2);
        testDbSession.Close();

        Assert.That(connStr, Is.Not.EqualTo(connStr2));
    }

    [Test]
    public void _02_CanQueryActors() {
        var factory = TestDbSessionFactory;
        using var session = factory.OpenSession();
        var actors = session.Query<Actor>().ToList();
        Assert.That(actors, Is.Not.Empty);
    }

    [Test]
    public void _03_CanInsertUpdateDeleteAuthors() {
        using var session = OpenTestDbSession();
        var author = new Actor {
            FirstName = "Simon",
            LastName = "Zhang",
            LastUpdate = DateTime.Now
        };
        session.Save(author);
        session.Flush();
        Assert.That(author.ActorId, Is.GreaterThan(0));
        session.Delete(author);
        session.Flush();
    }

    [Test]
    public void _04_FutureTest() {
        using var session = OpenTestDbSession();
        var actors = session.Query<Actor>().Where(act => act.LastName == "zhang").ToFuture();
        foreach (var actor in actors.GetEnumerable()) {
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(actor));
        }
        var actorCount = session.QueryOver<Actor>().ToRowCountQuery().FutureValue<int>();
        if (actorCount.Value > 0) {
            Console.WriteLine($"Actor count: {actorCount.Value}");
        }
    }

    [Test]
    public void _06_UpdateTest() {
        using var session = OpenTestDbSession();
        session.Query<Actor>().Where(
            a => a.LastName == "zhang"
        ).Update(c => new Actor {
            LastName = "zhimin",
            LastUpdate = DateTime.Now
        });
    }

}
