using System.Data;
using NHibernate.SqlTypes;
using NpgsqlTypes;

namespace NHibernate.Extensions.Npgsql;

public class NpgsqlType : SqlType {

    public NpgsqlDbType NpgDbType { get; }

    public NpgsqlType(DbType dbType, NpgsqlDbType npgDbType)
        : base(dbType) {
        NpgDbType = npgDbType;
    }

    public NpgsqlType(DbType dbType, NpgsqlDbType npgDbType, int length)
        : base(dbType, length) {
        NpgDbType = npgDbType;
    }

    public NpgsqlType(DbType dbType, NpgsqlDbType npgDbType, byte precision, byte scale)
        : base(dbType, precision, scale) {
        NpgDbType = npgDbType;
    }

}
