using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;
using NpgsqlTypes;
using Pgvector;

namespace NHibernate.Extensions.Pgvector.UserTypes;

public class VectorType : IUserType {

    bool IUserType.Equals(object x, object y) {
        var vx = (Vector)x;
        var vy = (Vector)y;
        return vx.Equals(vy);
    }

    int IUserType.GetHashCode(object x) {
        return ((Vector)x).GetHashCode();
    }

    object IUserType.NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner) {
        if (names.Length != 1) {
            throw new InvalidOperationException("Only expecting one column...");
        }
        return (Vector)rs[names[0]];
    }

    void IUserType.NullSafeSet(DbCommand cmd, object? value, int index, ISessionImplementor session) {
        var parameter = (NpgsqlParameter)cmd.Parameters[index];
        if (value == null) {
            parameter.Value = DBNull.Value;
        }
        else {
            var vector = (Vector)value;
            parameter.Value = vector;
            parameter.DataTypeName = "vector";
        }
    }

    object IUserType.DeepCopy(object value) {
        var vector = (Vector)value;
        var dest = new Memory<float>(new float[vector.Memory.Length]);
        vector.Memory.CopyTo(dest);
        var result = new Vector(dest);
        return result;
    }

    object IUserType.Replace(object original, object target, object owner) {
        return original;
    }

    object IUserType.Assemble(object cached, object owner) {
        return cached;
    }

    object IUserType.Disassemble(object value) {
        return value;
    }

    SqlType[] IUserType.SqlTypes => [new PgvectorSqlType(DbType.Object, PgvectorType.Vector)];

    System.Type IUserType.ReturnedType => typeof(Vector);

    bool IUserType.IsMutable => false;

}
