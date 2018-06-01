using System.Data;
using NHibernate.SqlTypes;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql {

    public class NpgSqlType : SqlType {

        public NpgsqlDbType NpgDbType { get; }

        public NpgSqlType(DbType dbType, NpgsqlDbType npgDbType)
            : base(dbType) {
            NpgDbType = npgDbType;
        }

        public NpgSqlType(DbType dbType, NpgsqlDbType npgDbType, int length)
            : base(dbType, length) {
            NpgDbType = npgDbType;
        }

        public NpgSqlType(DbType dbType, NpgsqlDbType npgDbType, byte precision, byte scale)
            : base(dbType, precision, scale) {
            NpgDbType = npgDbType;
        }

    }

}
