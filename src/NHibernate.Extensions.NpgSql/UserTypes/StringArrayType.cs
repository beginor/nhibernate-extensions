using System;
using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql;

[Obsolete("Please use ArrayType<string> instead.")]
public class StringArrayType : ArrayType<string> {

    protected override NpgSqlType GetNpgSqlType() {
        return new NpgSqlType(
            DbType.Object,
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            NpgsqlDbType.Array | NpgsqlDbType.Varchar
        );
    }

}
