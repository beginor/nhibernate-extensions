using System.Text.Json;
using NHibernate.Extensions.NpgSql;
using NHibernate.Mapping.Attributes;
using PropertyAttribute = NHibernate.Mapping.Attributes.PropertyAttribute;

namespace NHibernate.Extensions.UnitTest.TestDb;

[Class(Table = "test_table", Schema = "public")]
public class TestEntity {

    [Id(Name = nameof(Id), Column = "id", Type = "long", Generator = "trigger-identity")]
    public virtual long Id { get; set; }

    [Property(Column = "name", Type = "string", Length = 32)]
    public virtual string Name { get; set; }

    [Property(Column = "tags", Type = "string[]")]
    public virtual string[] Tags { get; set; }

    [Property(Column = "json_field", Type = "json")]
    public virtual JsonElement JsonField { get; set; }

    [Property(Column = "jsonb_field", Type = "jsonb")]
    public virtual JsonElement JsonbField { get; set; }

    [Property(Column = "update_time", Type = "timestamp")]
    public virtual DateTime? UpdateTime { get; set; }

    [Property(Column = "int16_arr", Type = "short[]")]
    public virtual short[] Int16Arr { get; set; }

    [Property(Column = "int32_arr", Type = "int[]")]
    public virtual int[] Int32Arr { get; set; }

    [Property(Column = "int64_arr", Type = "long[]")]
    public virtual long[] Int64Arr { get; set; }

    [Property(Column = "real_arr", Type = "float[]")]
    public virtual float[] FloatArr { get; set; }

    [Property(Column = "double_arr", Type = "double[]")]
    public virtual double[] DoubleArr { get; set; }

    [Property(Column = "bool_arr", Type = "bool[]")]
    public virtual bool[] BooleanArr { get; set; }

}
