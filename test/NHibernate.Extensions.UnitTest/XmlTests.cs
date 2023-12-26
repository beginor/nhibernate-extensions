using System.Xml;
using NUnit.Framework.Legacy;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class XmlTests : BaseTest {

    [Test]
    public void _01_CanQueryXmlEntity() {
        using var session = TestDbSessionFactory.OpenSession();
        var entities = session.Query<XmlTestEntity>().ToList();
        ClassicAssert.IsTrue(entities.Count >= 0);
        foreach (var entity in entities) {
            Console.WriteLine($"id: {entity.Id}");
            Console.WriteLine($"statement: {entity.Statement}");
        }
    }

    [Test]
    public void _02_CanSaveXmlEntity() {
        using var session = TestDbSessionFactory.OpenSession();
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml("<Statement></Statement>");
        var entity = new XmlTestEntity {
            Statement = xmlDoc
        };
        session.Save(entity);
        ClassicAssert.Greater(entity.Id, 0);
        Console.WriteLine(entity.Id);
    }

}
