using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql;

public static class ArrayTypeUtil {

    public static readonly IDictionary<System.Type, NpgsqlDbType> KnownArrayTypes
        = new ConcurrentDictionary<System.Type, NpgsqlDbType> {
            [typeof(bool)] = NpgsqlDbType.Boolean,
            [typeof(short)] = NpgsqlDbType.Smallint,
            [typeof(int)] = NpgsqlDbType.Integer,
            [typeof(long)] = NpgsqlDbType.Bigint,
            [typeof(float)] = NpgsqlDbType.Real,
            [typeof(double)] = NpgsqlDbType.Double,
            [typeof(decimal)] = NpgsqlDbType.Numeric,
            [typeof(string)] = NpgsqlDbType.Varchar,
            [typeof(DateTime)] = NpgsqlDbType.Timestamp,
            [typeof(DateTimeOffset)] = NpgsqlDbType.TimestampTz,
            [typeof(TimeSpan)] = NpgsqlDbType.Time
        };

}
