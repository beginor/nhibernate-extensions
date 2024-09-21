using System;
using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.Npgsql;

[Obsolete("Please use ArrayType<decimal> instead.")]
public class NumericArrayType : ArrayType<decimal> {

    protected override NpgsqlType GetNpgsqlType() {
        return new NpgsqlType(
            DbType.Object,
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            NpgsqlDbType.Array | NpgsqlDbType.Numeric
        );
    }

}
