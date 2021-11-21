using NHibernate.Mapping.Attributes;


namespace NHibernate.Extensions.UnitTest.TestDb;

[Class(Schema = "public", Table = "version_table")]
public class VersionTable {

    [Id(Name = "Id", Column = "id", Generator = "trigger-identity")]
    public virtual long Id { get; set; }

    [Property(Column = "name", Type = "string", Length = 32)]
    public virtual string Name { get; set; }

    [Property(Column = "description", Type = "string", Length = 32)]
    public virtual string Description { get; set; }

    [Version(Column = "version", Type = "int")]
    public virtual int Version { get; set; }

}

