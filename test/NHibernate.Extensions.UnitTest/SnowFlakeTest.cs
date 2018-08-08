using System;
using System.Linq;
using Newtonsoft.Json;
using NHibernate.Extensions.UnitTest.NpgSql.Data;
using Xunit;

namespace NHibernate.Extensions.UnitTest {

    public class SnowFlakeTest : BaseTest {

        [Fact]
        public void _01_CanQuerySnowFlakeId() {
            using (var session = factory.OpenSession()) {
                var flakes = session.Query<SnowFlakeTestEntity>()
                    .ToList();
                foreach (var f in flakes) {
                    Assert.True(f.Id > 0);
                    Console.WriteLine(JsonConvert.SerializeObject(f));
                }
            }
        }
    }

}
