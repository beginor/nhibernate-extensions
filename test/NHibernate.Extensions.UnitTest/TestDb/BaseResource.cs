using NHibernate.Mapping.Attributes;
using PropertyAttribute = NHibernate.Mapping.Attributes.PropertyAttribute;

namespace NHibernate.Extensions.UnitTest.TestDb;

[Class(Schema = "public", Table="base_resources")]
[Discriminator(Column = "type")]
public class BaseResource {

    [Id(Name = nameof(Id), Column = "id", Type = "long", Generator = "trigger-identity")]
    public virtual long Id { get; set; }

    [Property(Name=nameof(Name), Column = "name", Type = "string", Length = 32, NotNull = true)]
    public virtual string Name { get; set; }

    [Property(Name=nameof(Type), Column = "type", Type = "string", Length = 64, NotNull = true, Insert = false, Update = false)]
    public virtual string Type { get; set; }

}

[Subclass(DiscriminatorValue = "DataApi", ExtendsType = typeof(BaseResource), Lazy = true)]
public class DataApi : BaseResource {
    [Join(Schema = "public", Table = "data_apis", Fetch = JoinFetch.Select)]
    [Key(Column = "id")]
    [Property(Name = nameof(Statement), Column = "statement", Length = 128, NotNull = true)]
    [Property(Name = nameof(Parameters), Column = "parameters", Length = 128, NotNull = true)]
    public virtual string Statement { get; set; }
    public virtual string Parameters { get; set; }
}

[Subclass(DiscriminatorValue = "Slpk", ExtendsType = typeof(BaseResource), Lazy = true)]
public class Slpk : BaseResource {
    [Join(Schema = "public", Table = "slpks", Fetch = JoinFetch.Select)]
    [Key(Column = "id")]
    [Property(Name = nameof(Path), Column = "path", Length = 128, NotNull = true)]
    public virtual string Path { get; set; }

}
