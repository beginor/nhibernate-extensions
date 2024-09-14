namespace NHibernate.Extensions.UnitTest.TestDb;

using NHibernate.Mapping.Attributes;
using PropertyAttribute = NHibernate.Mapping.Attributes.PropertyAttribute;

[Class(Schema = "public", Table = "npg_times")]
public class NpgTime {
    [Id(Name = nameof(Id), Column = "id", Type = "long", Generator = "trigger-identity")]
    public virtual long Id { get; set; }
    [Property(Name = nameof(LocalTime), Column = "local_time", Type = "timestamp", NotNull = false)]
    public virtual DateTime? LocalTime { get; set; }
    [Property(Name = nameof(UtcTime), Column = "utc_time", Type = "timestamptz", NotNull = false)]
    public virtual DateTime? UtcTime { get; set; }

    public override string ToString() {
        return $"{nameof(Id)}: {Id}, {nameof(LocalTime)}: {LocalTime}, {nameof(UtcTime)}: {UtcTime}";
    }

}
