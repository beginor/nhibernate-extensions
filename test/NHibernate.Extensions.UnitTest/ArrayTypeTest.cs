using System.Text.Json;
using NHibernate.Type;
using NHibernate.Extensions.NpgSql;
using NUnit.Framework.Legacy;
using NHibernate.Extensions.UnitTest.TestDb;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class ArrayTypeTest : BaseTest {

    [OneTimeSetUp]
    public override void OneTimeSetUp() {
        base.OneTimeSetUp();
    }

    [Test]
    public void _01_CanDoCrud() {
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

            ClassicAssert.True(entity.Id > 0);

            Console.WriteLine($"entity id: {entity.Id}");
        }

        using (var session = TestDbSessionFactory.OpenSession()) {
            var query = session.Query<TestEntity>();
            var entities = query.ToList();
            ClassicAssert.NotNull(entities);
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

    [Test]
    public void _02_CanSaveTypedJson() {
        using var session = TestDbSessionFactory.OpenSession();
        var json = new JsonValue {
            Value = new ConnectionString {
                Server = "127.0.0.1",
                Port = 1433,
                Database = "northwind",
                Username = "sa",
                Password = "password"
            }
        };
        session.Save(json);
        session.Flush();
        ClassicAssert.Greater(json.Id, 0);
        Console.WriteLine(json.Id);
        var val = session.Get<JsonValue>(json.Id);
        ClassicAssert.NotNull(val);
        ClassicAssert.AreEqual(json.Id, val.Id);
        session.Delete(json);
        session.Flush();
    }

    [Test]
    public void _03_CanDoSqlQuery() {
        using var session = TestDbSessionFactory.OpenSession();
        var sql = @" select id, str_arr, int_arr
            from public.arr_test
            where int_arr && :arr_param
            order by id desc;
        ";

        var query = session.CreateSQLQuery(sql);
        query.AddScalar("id", NHibernateUtil.Int64);
        query.AddCustomScalar<StringArrayType>("str_arr");
        query.AddCustomScalar<Int32ArrayType>("int_arr");
        query.SetCustomParameter<Int32ArrayType>("arr_param", new [] { 2, 3 });

        var result = query.List();
        foreach (Array row in result) {
            Console.WriteLine(JsonSerializer.Serialize(row));
            // foreach (var field in row) {
            //     Console.WriteLine(field);
            // }
        }
    }
}
