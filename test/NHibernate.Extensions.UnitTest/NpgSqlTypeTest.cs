using System;
using NHibernate.Extensions.NpgSql;
using NpgsqlTypes;
using NUnit.Framework;

namespace NHibernate.Extensions.UnitTest {

    [TestFixture]
    public class NpgSqlTypeTest {

        [Test]
        public void _01_CanReadArrayNpgSqlType() {
            NpgsqlDbType type = NpgsqlDbType.Array | NpgsqlDbType.Bigint;
            if ((int) type < 0) {
                NpgsqlDbType res = (NpgsqlDbType)((int)type - (int)NpgsqlDbType.Array);
            }
            Assert.That(type == NpgsqlDbType.Array);
        }

    }

}
