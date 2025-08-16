using System.Data;
using System.Text.Json;
using Dapper;
using NHibernate.Linq;
using NHibernate.Extensions.Npgsql;
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

    [Test]
    public async Task _03_CanSaveRender() {
        using var session = OpenTestDbSession();
        var transaction = session.BeginTransaction();
        var idArr = new List<long>();
        var raster1 = new RasterEntity {
            Id = 1,
            Render = new SingleBandRender()
        };
        idArr.Add(raster1.Id);
        await session.SaveAsync(raster1);
        var raster2 = new RasterEntity {
            Id = 2,
            Render = new MultiBandRender()
        };
        idArr.Add(raster2.Id);
        await session.SaveAsync(raster2);
        await session.FlushAsync();
        session.Clear();
        await transaction.CommitAsync();

        var entities = await session.Query<RasterEntity>().Where(
            e => idArr.ToArray().ArrayContains(e.Id)
        ).ToListAsync();
        Assert.That(entities.Count, Is.GreaterThan(0));

        await session.Query<RasterEntity>().Where(
            e => idArr.ToArray().ArrayContains(e.Id)
        ).DeleteAsync();
    }

}
