﻿using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql;

public class BooleanArrayType : ArrayType<bool> {

    protected override NpgSqlType GetNpgSqlType() => new (
        DbType.Object,
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        NpgsqlDbType.Array | NpgsqlDbType.Boolean
    );

}
