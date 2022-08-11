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
        Assert.IsNotNull(TestDbSessionFactory);

        var dvdRentalSession = TestDbSessionFactory.OpenSession();
        var connStr = dvdRentalSession.Connection.ConnectionString;
        Console.WriteLine(connStr);
        dvdRentalSession.Close();

        var testDbSession = TestDbSessionFactory.OpenSession();
        var connStr2 = testDbSession.Connection.ConnectionString;
        Console.WriteLine(connStr2);
        testDbSession.Close();

        Assert.AreNotEqual(connStr, connStr2);
    }

    [Test]
    public void _02_CanQueryActors() {
        var factory = TestDbSessionFactory;
        using (var session = factory.OpenSession()) {
            var actors = session.Query<Actor>().ToList();
            Assert.IsNotEmpty(actors);
        }
    }

    [Test]
    public void _03_CanInsertUpdateDeleteAuthors() {
        using (var session = OpenTestDbSession()) {
            var author = new Actor {
                FirstName = "Simon",
                LastName = "Zhang",
                LastUpdate = DateTime.Now
            };
            session.Save(author);
            session.Flush();
            Assert.Greater(author.ActorId, 0);
            session.Delete(author);
            session.Flush();
        }
    }

}
