using System.Text.Json;
using NHibernate.Extensions.UnitTest.TestDb;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class SnowFlakeTest : BaseTest {

    [OneTimeSetUp]
    public override void OneTimeSetUp() {
        base.OneTimeSetUp();
    }

    [Test]
    public void _01_CanQuerySnowFlakeId() {
        using (var session = TestDbSessionFactory.OpenSession()) {
            var entities = session.Query<SnowFlakeTestEntity>()
                .ToList();
            foreach (var entity in entities) {
                Assert.That(entity.Id > 0);
                Console.WriteLine(JsonSerializer.Serialize(entity));
            }
        }
    }

    [Test]
    public void _02_CanInsertSnowFlakeId() {
        using (var session = TestDbSessionFactory.OpenSession()) {
            var entity = new SnowFlakeTestEntity {
                Name = Guid.NewGuid().ToString("N")
            };
            session.Save(entity);
            Assert.That(entity.Id > 0);
            Console.WriteLine($"Id: {entity.Id}");
        }
    }
}
