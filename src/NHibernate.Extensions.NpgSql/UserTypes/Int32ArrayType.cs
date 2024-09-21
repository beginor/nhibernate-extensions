using System;
using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.Npgsql;

[Obsolete("Please use ArrayType<int> instead.")]
public class Int32ArrayType : ArrayType<int> {

    protected override NpgsqlType GetNpgsqlType() => new (
        DbType.Object,
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        NpgsqlDbType.Array | NpgsqlDbType.Integer
    );

}
