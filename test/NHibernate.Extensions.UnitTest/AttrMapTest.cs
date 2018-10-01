using System;
using System.IO;
using NHibernate.Extensions.UnitTest.NpgSql.Data;
using NHibernate.Mapping.Attributes;
using NUnit.Framework;

namespace NHibernate.Extensions.UnitTest {

    [TestFixture]
    public class AttrMapTest {

        [Test]
        public void _01_CanSerializeXmlMap() {
            var serializer = HbmSerializer.Default;
            var stream = serializer.Serialize(
                typeof(SnowFlakeTestEntity).Assembly
            );

            var err = serializer.Error.ToString();

            Assert.IsEmpty(err);

            var reader = new StreamReader(stream);
            var xml = reader.ReadToEnd();
            Console.WriteLine(xml);
        }

    }

}
