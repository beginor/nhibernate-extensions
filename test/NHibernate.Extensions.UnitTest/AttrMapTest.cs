using NUnit.Framework.Legacy;
using NHibernate.Mapping.Attributes;
using NHibernate.Extensions.UnitTest.TestDb;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class AttrMapTest {

    [Test]
    public void _01_CanSerializeXmlMap() {
        var serializer = HbmSerializer.Default;
        var stream = serializer.Serialize(
            typeof(SnowFlakeTestEntity).Assembly
        );

        var err = serializer.Error.ToString();

        ClassicAssert.IsEmpty(err);

        var reader = new StreamReader(stream);
        var xml = reader.ReadToEnd();
        Console.WriteLine(xml);
    }

}
