﻿using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;
using NpgsqlTypes;

namespace NHibernate.Extensions.Npgsql.UserTypes;

public class ArrayType<T> : IUserType {

    public SqlType[] SqlTypes => [GetNpgsqlType()];

    public System.Type ReturnedType => typeof(T[]);

    public bool IsMutable => false;

    public object Assemble(object cached, object owner) {
        return cached;
    }

    public object? DeepCopy(object value) {
        if (!(value is T[] arr)) {
            return null;
        }
        var result = new T[arr.Length];
        Array.Copy(arr, result, arr.Length);
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
        return ((T[])x).Equals((T[])y);
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
            throw new InvalidOperationException("Only expecting one column...");
        }
        return rs[names[0]] as T[];
    }

    public void NullSafeSet(
        DbCommand cmd,
        object? value,
        int index,
        ISessionImplementor session
    ) {
        var parameter = (NpgsqlParameter)cmd.Parameters[index];
        if (value == null) {
            parameter.Value = DBNull.Value;
        }
        else {
            parameter.NpgsqlDbType = GetNpgsqlType().NpgDbType;
            if (!(value is T[] arr)) {
                throw new InvalidOperationException(
                    $"\"{parameter.ParameterName}\" is not {typeof(T)}[]"
                );
            }
            parameter.Value = arr;
        }
    }

    public object Replace(object original, object target, object owner) {
        return original;
    }

    protected virtual NpgsqlType GetNpgsqlType() {
        var type = typeof(T);
        if (!ArrayTypeUtil.KnownTypes.TryGetValue(type, out var dbType)) {
            throw new NotSupportedException($"Unknown type {typeof(T)}");
        }
        return new NpgsqlType(
            DbType.Object,
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            NpgsqlDbType.Array | dbType
        );
    }
}
