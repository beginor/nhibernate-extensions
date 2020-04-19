using System;
using System.Linq;
using System.Text.Json;
using NHibernate.Extensions.UnitTest.TestDb;
using NUnit.Framework;

namespace NHibernate.Extensions.UnitTest {

    [TestFixture]
    public class ArrayTypeTest : BaseTest {

        [OneTimeSetUp]
        public override void OneTimeSetUp() {
            base.OneTimeSetUp();
        }

        [Test]
        public void CanDoCrud() {
            using (var session = TestDbSessionFactory.OpenSession()) {
                var entity = new TestEntity {
                    Name = "Test 1",
                    Tags = new [] { "hello", "world" },
                    JsonField = JsonSerializer.Deserialize<JsonElement>("{ \"val\": 1 }"),
                    JsonbField = JsonSerializer.Deserialize<JsonElement>("{ \"val\": 1 }"),
                    UpdateTime = DateTime.Now,
                    Int16Arr = new short[] { 1, 2, 3 },
                    Int32Arr = new [] { 1, 2, 3 },
                    Int64Arr = new [] { 1L, 2L, 3L },
                    FloatArr = new [] { 1.1F, 2.2F, 3.3F },
                    DoubleArr = new [] { 1.1, 2.2, 3.3 },
                    BooleanArr = new [] { true, false }
                };
                session.Save(entity);
                session.Flush();
                session.Clear();

                Assert.True(entity.Id > 0);

                Console.WriteLine($"entity id: {entity.Id}");
            }

            using (var session = TestDbSessionFactory.OpenSession()) {
                var query = session.Query<TestEntity>();
                var entities = query.ToList();
                Assert.NotNull(entities);
                Console.WriteLine($"Entity count: {entities.Count}");

                using (var tx = session.BeginTransaction()) {
                    foreach (var e in entities) {
                        Console.WriteLine(JsonSerializer.Serialize(e));
                        session.Delete(e);
                    }
                    tx.Commit();
                }
            }
        }

    }

}
