using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql {

    public class DoubleArrayType : ArrayType<double> {

        protected override NpgSqlType GetNpgSqlType() => new NpgSqlType(
            DbType.Object,
            NpgsqlDbType.Array | NpgsqlDbType.Double
        );

    }

}
