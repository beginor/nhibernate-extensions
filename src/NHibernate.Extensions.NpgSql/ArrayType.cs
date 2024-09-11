using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql;

public class ArrayType<T> : IUserType {

    public SqlType[] SqlTypes => [GetNpgSqlType()];

    public System.Type ReturnedType => typeof(T[]);

    public bool IsMutable => true;

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
            parameter.NpgsqlDbType = GetNpgSqlType().NpgDbType;
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

    protected virtual NpgSqlType GetNpgSqlType() {
        var type = typeof(T);
        if (!ArrayTypeUtil.KnownArrayTypes.ContainsKey(type)) {
            throw new NotSupportedException($"Unknown type {typeof(T)}");
        }
        return new NpgSqlType(
            DbType.Object,
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            NpgsqlDbType.Array | ArrayTypeUtil.KnownArrayTypes[type]
        );
    }
}

public static class ArrayExtensions {

    public static bool ArrayContains<T>(this T[] array, T element) {
        return array.Contains(element);
    }

}
