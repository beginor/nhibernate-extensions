using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql {

    public class BooleanArrayType : ArrayType<bool> {

        protected override NpgSqlType GetNpgSqlType() => new NpgSqlType(
            DbType.Object,
            NpgsqlDbType.Array | NpgsqlDbType.Boolean
        );

    }

}
