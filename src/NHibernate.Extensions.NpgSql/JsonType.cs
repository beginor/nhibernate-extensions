using System;
using System.Data;
using System.Data.Common;
using System.Text.Json;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql;

public class JsonType<T> : IUserType {

    public virtual SqlType[] SqlTypes => new SqlType[] {
        new NpgSqlType(DbType.Object, NpgsqlDbType.Json)
    };

    public System.Type ReturnedType => typeof(T);

    public bool IsMutable => true;

    public object Assemble(object cached, object owner) {
        return cached;
    }

    public object? DeepCopy(object? value) {
        if (value == null) {
            return null;
        }
        var json = JsonSerializer.Serialize(value);
        var obj = JsonSerializer.Deserialize(json, value.GetType());
        return obj;
    }

    public object Disassemble(object value) {
        return value;
    }

    public new bool Equals(object? x, object? y) {
        if (x == null && y == null) {
            return true;
        }
        if (x == null || y == null) {
            return false;
        }
        return ((T)x).Equals((T)y);
    }

    public int GetHashCode(object? x) {
        return x == null ? 0 : x.GetHashCode();
    }

    public object? NullSafeGet(
        DbDataReader rs,
        string[] names,
        ISessionImplementor session,
        object owner
    ) {
        if (names.Length != 1) {
            throw new InvalidOperationException(
                "Only expecting one column..."
            );
        }
        if (rs[names[0]] is string val) {
            return JsonSerializer.Deserialize<T>(val);
        }
        return null;
    }

    public void NullSafeSet(
        DbCommand cmd,
        object? value,
        int index,
        ISessionImplementor session
    ) {
        var parameter = (NpgsqlParameter)cmd.Parameters[index];
        parameter.Value = value ?? DBNull.Value;
    }

    public object Replace(object original, object target, object owner) {
        return original;
    }

}

public class JsonbType<T> : JsonType<T> {

    public override SqlType[] SqlTypes => new SqlType[] {
        new NpgSqlType(DbType.Binary, NpgsqlDbType.Jsonb)
    };

}

public class JsonType : JsonType<JsonElement> { }

public class JsonbType : JsonbType<JsonElement> { }
