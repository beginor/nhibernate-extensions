using System.Text.Json;
using NHibernate.Extensions.NpgSql;
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

            Assert.That(entity.Id > 0);

            Console.WriteLine($"entity id: {entity.Id}");
        }

        using (var session = TestDbSessionFactory.OpenSession()) {
            var query = session.Query<TestEntity>();
            var entities = query.ToList();
            Assert.That(entities, Is.Not.Null);
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
        Assert.That(json.Id, Is.GreaterThan(0));
        Console.WriteLine(json.Id);
        var val = session.Query<JsonValue>().First(x => x.Id == json.Id);
        Assert.That(val, Is.Not.Null);
        Assert.That(json.Id, Is.EqualTo(val.Id));
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

    [Test]
    public void _04_CanDoQueryDynamicJson() {
        using var session = TestDbSessionFactory.OpenSession();
        var query = session.CreateSQLQuery("select id, value from public.json_values");
        query.AddScalar("id", NHibernateUtil.Int64);
        query.AddCustomScalar<JsonbType>("value");
        var list = query.List();
        foreach (var item in list) {
            Console.WriteLine(JsonSerializer.Serialize(item));
        }
    }

}
