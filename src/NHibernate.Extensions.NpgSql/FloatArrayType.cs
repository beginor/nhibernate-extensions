using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql {

    public class FloatArrayType : ArrayType<float> {

        protected override NpgSqlType GetNpgSqlType() => new NpgSqlType(
            DbType.Object,
            NpgsqlDbType.Array | NpgsqlDbType.Real
        );

    }

}
