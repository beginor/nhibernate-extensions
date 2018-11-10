using System;
using System.Linq;
using Newtonsoft.Json;
using NHibernate.Extensions.UnitTest.TestDb;
using NUnit.Framework;

namespace NHibernate.Extensions.UnitTest {

    public class SnowFlakeTest : BaseTest {

        [Test]
        public void _01_CanQuerySnowFlakeId() {
            using (var session = TestDbSessionFactory.OpenSession()) {
                var entities = session.Query<SnowFlakeTestEntity>()
                    .ToList();
                foreach (var entity in entities) {
                    Assert.True(entity.Id > 0);
                    Console.WriteLine(JsonConvert.SerializeObject(entity));
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
                Assert.True(entity.Id > 0);
                Console.WriteLine($"Id: {entity.Id}");
            }
        }
    }

}
