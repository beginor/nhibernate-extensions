using System.Data;
using System.Text.Json;
using Dapper;
using NHibernate.Linq;
using NHibernate.Extensions.UnitTest.TestDb;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class JsonPolymorphicTest : BaseTest {

    [OneTimeSetUp]
    public override void OneTimeSetUp() {
        base.OneTimeSetUp();
    }

    [Test]
    public async Task _01_CanReadRaster() {
        using var session = OpenTestDbSession();
        var query = session.Query<RasterEntity>();
        var rasters = await query.ToListAsync();
        Assert.That(rasters.Count, Is.GreaterThan(0));
        Console.WriteLine(rasters.Count);
    }

    [Test]
    public void _02_CanDeserializeRaster() {
        RasterRender multiBand = new MultiBandRender();
        var json = JsonSerializer.Serialize(multiBand, JsonSerializerOptions.Web);
        Console.WriteLine(json);
        // json = "{\"Red\": {\"Band\": 1, \"MaxValue\": 255, \"MinValue\": 0}, \"Type\": \"MultiBand\",\"Blue\": {\"Band\": 3, \"MaxValue\": 255, \"MinValue\": 0}, \"Green\": {\"Band\": 2, \"MaxValue\": 255, \"MinValue\": 0}}";
        // var jsonDoc = JsonDocument.Parse(json);
        var render = JsonSerializer.Deserialize<RasterRender>(json);
        Assert.That(render, Is.Not.Null);
        Console.WriteLine($"render type: {render.GetType()}");
    }

}
