using NpgsqlTypes;
using NUnit.Framework.Legacy;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class NpgSqlTypeTest {

    [Test]
    public void _01_CanReadArrayNpgSqlType() {
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        var type = NpgsqlDbType.Array | NpgsqlDbType.Bigint;
        var res = (NpgsqlDbType)((int)type - (int)NpgsqlDbType.Array);
        ClassicAssert.IsTrue(res == NpgsqlDbType.Bigint);
    }

    [Test]
    public void _02_EqualsArrayType() {
        var arrayTypeInt = (int)NpgsqlDbType.Array + (int)NpgsqlDbType.Bigint;
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        var arrayType = NpgsqlDbType.Array | NpgsqlDbType.Bigint;
        ClassicAssert.AreEqual((int)arrayType, arrayTypeInt);
    }

}
