using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql;

public static class ArrayTypeUtil {

    public static readonly IDictionary<System.Type, NpgsqlDbType> KnownTypes
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

    public static System.Type GetArrayType(System.Type type) {
        if (!KnownTypes.ContainsKey(type)) {
            throw new HibernateException($"The ArrayType of '{type.Name}' is not supported.");
        }
        var arrayType = typeof(ArrayType<>);
        var genericArrayType = arrayType.MakeGenericType(type);
        return genericArrayType;
    }

}
