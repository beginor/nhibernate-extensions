using System.Xml;
using NHibernate.Mapping.Attributes;
using PropertyAttribute = NHibernate.Mapping.Attributes.PropertyAttribute;

namespace NHibernate.Extensions.UnitTest;

[Class(Schema = "public", Table = "xml_test")]
public class XmlTestEntity {


    [Id(Name = "Id", Column = "id", Generator = "trigger-identity")]
    public virtual long Id { get; set; }

    [Property(Name = "Statement", Column = "statement", Type = "xml", Length = 32)]
    public virtual XmlDocument Statement { get; set; }

}
