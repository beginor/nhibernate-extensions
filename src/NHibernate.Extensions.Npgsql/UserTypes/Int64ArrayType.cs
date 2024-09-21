using System;
using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.Npgsql;

[Obsolete("Please use ArrayType<long> instead.")]
public class Int64ArrayType : ArrayType<long> {

    protected override NpgsqlType GetNpgsqlType() => new(
        DbType.Object,
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        NpgsqlDbType.Array | NpgsqlDbType.Bigint
    );

}
