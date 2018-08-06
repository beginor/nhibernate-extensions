using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHibernate.Cfg;
using NHibernate.Linq;
using NHibernate.Extensions.UnitTest.NpgSql.Data;
using Npgsql;
using Xunit;

namespace NHibernate.Extensions.UnitTest.NpgSql {

    public class NpgSqlTest {

        private ISessionFactory factory;

        public NpgSqlTest() {
            NpgsqlConnection.GlobalTypeMapper.UseJsonNet();
            var configuration = new Configuration();
            var configFile = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "hibernate.config"
            );
            configuration.Configure(configFile);
            factory = configuration.BuildSessionFactory();
        }

        [Fact]
        public void CanDoCrud() {
            using (var session = factory.OpenSession()) {
                var entity = new TestEntity {
                    Name = "Test 1",
                    Tags = new [] { "hello", "world" },
                    JsonField = JToken.Parse("{ \"val\": 1 }"),
                    JsonbField = JToken.Parse("{ \"val\": 1 }"),
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

            using (var session = factory.OpenSession()) {
                var query = session.Query<TestEntity>();
                var entities = query.ToList();
                Assert.NotNull(entities);
                Console.WriteLine($"Entity count: {entities.Count}");

                using (var tx = session.BeginTransaction()) {
                    foreach (var e in entities) {
                        Console.WriteLine(JsonConvert.SerializeObject(e));
                        session.Delete(e);
                    }
                    tx.Commit();
                }
            }
        }

    }

}
