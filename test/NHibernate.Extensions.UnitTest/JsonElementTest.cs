using System.Text.Json;
using NHibernate.Linq;

using NHibernate.Extensions.UnitTest.TestDb;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class JsonElementTest : BaseTest {

    [OneTimeSetUp]
    public override void OneTimeSetUp() {
        base.OneTimeSetUp();
    }

    [Test]
    public async Task _01_CanReadJsonElement() {
        using var session = OpenTestDbSession();
        var entity = new TestEntity {
            Name = "Test 1",
            Tags = ["hello", "world"],
            JsonField = JsonSerializer.Deserialize<JsonElement>("{ \"val\": 1 }"),
            JsonbField = JsonSerializer.Deserialize<JsonElement>("{ \"val\": 1 }"),
            UpdateTime = DateTime.Now,
            Int16Arr = [1, 2, 3],
            Int32Arr = [1, 2, 3],
            Int64Arr = [1L, 2L, 3L],
            FloatArr = [1.1F, 2.2F, 3.3F],
            DoubleArr = [1.1, 2.2, 3.3],
            BooleanArr = [true, false]
        };
        await session.SaveAsync(entity);
        await session.FlushAsync();
        session.Clear();
        var id = entity.Id;
        Console.WriteLine($"Entity Id is: {id}");
        Assert.That(id, Is.GreaterThan(0));
        //
        var data = await session.Query<TestEntity>()
            .Select(x => new { x.Id, x.JsonbField, x.JsonField })
            .ToListAsync()
            .ConfigureAwait(false);
        Console.WriteLine($"Entity count: {data.Count}");
        foreach (var item in data) {
            Console.WriteLine($"id: {item.Id}");
            Console.WriteLine($"json: {item.JsonField}");
            Console.WriteLine($"jsonb: {item.JsonbField}");
        }
        Assert.That(data.Count, Is.GreaterThan(0));
        //
        await session.DeleteAsync(entity);
        await session.FlushAsync();
        session.Clear();
    }

}
