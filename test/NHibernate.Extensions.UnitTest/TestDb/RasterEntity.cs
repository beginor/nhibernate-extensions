using System.Text.Json.Serialization;
using NHibernate.Extensions.Npgsql.UserTypes;
using NHibernate.Mapping.Attributes;
using PropertyAttribute = NHibernate.Mapping.Attributes.PropertyAttribute;

namespace NHibernate.Extensions.UnitTest.TestDb;

[Class(Schema = "public", Table = "rasters")]
public class RasterEntity {
    [Id(Name = nameof(Id), Column = "id", Type = "long", Generator = "assigned")]
    public virtual long Id { get; set; }
    [Property(Name = nameof(Render), Column = "render", TypeType = typeof(JsonType<RasterRender>))]
    public virtual RasterRender Render { get; set; }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SingleBandRender), "singleBand")]
[JsonDerivedType(typeof(MultiBandRender), "multiBand")]
public abstract class RasterRender {
}

public class SingleBandRender : RasterRender {
    public int Band { get; set; }
    public IList<ColorMap> ColorMaps { get; set; } = [];
}

public class MultiBandRender : RasterRender {
    public BandRender Red { get; set; } = BandRender.Red;
    public BandRender Green { get; set; } = BandRender.Green;
    public BandRender Blue { get; set; } = BandRender.Blue;
}

public class BandRender {
    public int Band { get; set; } = 0;
    public double MinValue { get; set; } = 0;
    public double MaxValue { get; set; } = 255;

    public static readonly BandRender Red = new() {
        Band = 3,
        MinValue = 0,
        MaxValue = 255
    };
    public static readonly BandRender Green = new() {
        Band = 4,
        MinValue = 0,
        MaxValue = 255
    };
    public static readonly BandRender Blue = new() {
        Band = 5,
        MinValue = 0,
        MaxValue = 255
    };
}

public class ColorMap {
    public double Value { get; set; }
    public string Color { get; set; } = "#00000000";
    public string Description { get; set; } = string.Empty;
}
