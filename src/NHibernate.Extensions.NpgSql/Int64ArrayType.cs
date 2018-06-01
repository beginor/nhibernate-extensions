using System.Data;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql {

    public class Int64ArrayType : ArrayType<long> {

        protected override NpgSqlType GetNpgSqlType() => new NpgSqlType(
            DbType.Object,
            NpgsqlDbType.Array | NpgsqlDbType.Bigint
        );

    }

}
