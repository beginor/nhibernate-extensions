using NHibernate.Mapping.Attributes;
using PropertyAttribute = NHibernate.Mapping.Attributes.PropertyAttribute;

namespace NHibernate.Extensions.UnitTest.TestDb;

[Class(Table = "snow_flake_test", Schema = "public")]
public class SnowFlakeTestEntity {

    [Id(Name = "Id", Column = "id", Type="long", Generator = "trigger-identity")]
    public virtual long Id { get; set; }

    [Property(Column = "name", Type = "string", Length = 32)]
    public virtual string Name { get; set; }

}
