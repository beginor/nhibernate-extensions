using System;
using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.Npgsql;

[Obsolete("Please use ArrayType<float> instead.")]
public class FloatArrayType : ArrayType<float> {

    protected override NpgsqlType GetNpgsqlType() => new (
        DbType.Object,
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        NpgsqlDbType.Array | NpgsqlDbType.Real
    );

}
