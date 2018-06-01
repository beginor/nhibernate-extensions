using System.Data;
using NHibernate.SqlTypes;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql {

    public class JsonbType : JsonType {

        public override SqlType[] SqlTypes => new SqlType[] {
            new NpgSqlType(DbType.Binary, NpgsqlDbType.Jsonb)
        };

    }

}
