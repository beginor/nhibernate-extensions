using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;
using NpgsqlTypes;

namespace NHibernate.Extensions.Npgsql;

public class TimeStampType : IUserType {

    public virtual SqlType[] SqlTypes => [
        new NpgsqlType(DbType.DateTime, NpgsqlDbType.Timestamp)
    ];

    public System.Type ReturnedType => typeof(DateTime);

    public bool IsMutable => true;

    public object Assemble(object cached, object owner) {
        return cached;
    }

    public object? DeepCopy(object? value) {
        if (value == null) {
            return null;
        }
        var val = (DateTime)value;
        var result = DateTime.FromBinary(val.ToBinary());
        return result;
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
        return ((DateTime)x).Equals((DateTime)y);
    }

    public int GetHashCode(object? x) {
        return x == null ? 0 : x.GetHashCode();
    }

    public object? NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner) {
        if (names.Length != 1) {
            throw new InvalidOperationException("Only expecting one column...");
        }
        var name = names[0];
        if (rs.IsDBNull(name)) {
            return null;
        }
        return rs.GetDateTime(name);
    }

    public void NullSafeSet(DbCommand cmd, object? value, int index, ISessionImplementor session) {
        var parameter = (NpgsqlParameter)cmd.Parameters[index];
        parameter.Value = value ?? DBNull.Value;
        parameter.NpgsqlDbType = NpgsqlDbType.Timestamp;
    }

    public object Replace(object original, object target, object owner) {
        return original;
    }
}
