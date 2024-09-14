using System;
using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql;

[Obsolete("Please use ArrayType<decimal> instead.")]
public class NumericArrayType : ArrayType<decimal> {

    protected override NpgSqlType GetNpgSqlType() {
        return new NpgSqlType(
            DbType.Object,
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            NpgsqlDbType.Array | NpgsqlDbType.Numeric
        );
    }

}
