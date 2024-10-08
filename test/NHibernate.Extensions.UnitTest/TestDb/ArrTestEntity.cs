using NHibernate.Mapping.Attributes;
using PropertyAttribute = NHibernate.Mapping.Attributes.PropertyAttribute;

namespace NHibernate.Extensions.UnitTest.TestDb;

[Class(Schema = "public", Table = "arr_test")]
public class ArrTestEntity {
    [Id(Name = nameof(Id), Column = "id", Type = "long", Generator = "trigger-identity")]
    public virtual long Id { get; set; }
    [Property(Column = "int_arr", Type = "int[]")]
    public virtual int[] IntArr { get; set; }
    [Property(Column = "str_arr", Type = "string[]")]
    public virtual string[] StrArr { get; set; }
}
