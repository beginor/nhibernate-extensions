using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql;

public class Int32ArrayType : ArrayType<int> {

    protected override NpgSqlType GetNpgSqlType() => new NpgSqlType(
        DbType.Object,
        NpgsqlDbType.Array | NpgsqlDbType.Integer
    );

}
