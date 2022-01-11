using NHibernate.Extensions.UnitTest.TestDb;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class VersionTest : BaseTest {

    private ISessionFactory factory;

    [OneTimeSetUp]
    public override void OneTimeSetUp() {
        base.OneTimeSetUp();
        factory = TestDbSessionFactory;
    }

    [Test]
    public void _01_CanQueryVersionTable() {
        using (var session = factory.OpenSession()) {
            var query = session.Query<VersionTable>();
            var data = query.ToList();
            Assert.NotNull(data);
        }
    }

    [Test]
    public void _02_CanUpdateVersion() {
        var entity = new VersionTable {
            Name = "test " + DateTime.Now
        };
        using (var session = factory.OpenSession()) {
            session.Save(entity);
            session.Flush();
            Assert.Greater(entity.Id, 0);

            Assert.Greater(entity.Version, 0);
            var v1 = entity.Version;

            entity.Name = "update " + DateTime.Now;
            session.Update(entity);
            session.Flush();
            var v2 = entity.Version;

            Assert.Greater(v2, v1);

            session.Delete(entity);
            session.Flush();
        }

    }

}
