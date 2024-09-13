using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql;

public class DoubleArrayType : ArrayType<double> {

    protected override NpgSqlType GetNpgSqlType() => new (
        DbType.Object,
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        NpgsqlDbType.Array | NpgsqlDbType.Double
    );

}
