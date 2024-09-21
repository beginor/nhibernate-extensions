using System;
using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.Npgsql;

[Obsolete("Please use ArrayType<string> instead.")]
public class StringArrayType : ArrayType<string> {

    protected override NpgsqlType GetNpgsqlType() {
        return new NpgsqlType(
            DbType.Object,
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            NpgsqlDbType.Array | NpgsqlDbType.Varchar
        );
    }

}
