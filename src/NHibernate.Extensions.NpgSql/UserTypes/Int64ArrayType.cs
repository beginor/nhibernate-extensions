using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql;

public class Int64ArrayType : ArrayType<long> {

    protected override NpgSqlType GetNpgSqlType() => new(
        DbType.Object,
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        NpgsqlDbType.Array | NpgsqlDbType.Bigint
    );

}
