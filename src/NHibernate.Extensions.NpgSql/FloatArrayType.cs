using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql;

public class FloatArrayType : ArrayType<float> {

    protected override NpgSqlType GetNpgSqlType() => new (
        DbType.Object,
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        NpgsqlDbType.Array | NpgsqlDbType.Real
    );

}
