using System;
using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.Npgsql;

[Obsolete("Please use ArrayType<short> instead.")]
public class Int16ArrayType : ArrayType<short> {

    protected override NpgsqlType GetNpgsqlType() => new (
        DbType.Object,
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        NpgsqlDbType.Array | NpgsqlDbType.Smallint
    );

}
