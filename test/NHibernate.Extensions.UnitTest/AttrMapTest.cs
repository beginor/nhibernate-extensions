using System;
using System.IO;
using NHibernate.Extensions.UnitTest.NpgSql.Data;
using NHibernate.Mapping.Attributes;
using Xunit;

namespace NHibernate.Extensions.UnitTest {

    public class AttrMapTest {

        [Fact]
        public void _01_CanSerializeXmlMap() {
            var serializer = HbmSerializer.Default;
            var stream = serializer.Serialize(
                typeof(SnowFlakeTestEntity).Assembly
            );

            var err = serializer.Error.ToString();

            Assert.Empty(err);

            var reader = new StreamReader(stream);
            var xml = reader.ReadToEnd();
            Console.WriteLine(xml);
        }

    }

}
