using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;
using Pgvector;

namespace NHibernate.Extensions.Pgvector.UserTypes;

public class HalfVectorType : IUserType {

    bool IUserType.Equals(object x, object y) {
        var vx = (HalfVector)x;
        var vy = (HalfVector)y;
        return vx.Equals(vy);
    }

    int IUserType.GetHashCode(object x) {
        return ((HalfVector)x).GetHashCode();
    }

    object IUserType.NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner) {
        if (names.Length != 1) {
            throw new InvalidOperationException("Only expecting one column...");
        }
        return (HalfVector)rs[names[0]];
    }

    void IUserType.NullSafeSet(DbCommand cmd, object? value, int index, ISessionImplementor session) {
        var parameter = (NpgsqlParameter)cmd.Parameters[index];
        if (value == null) {
            parameter.Value = DBNull.Value;
        }
        else {
            var vector = (HalfVector)value;
            parameter.Value = vector;
            parameter.DataTypeName = "halfvec";
        }
    }

    object IUserType.DeepCopy(object value) {
        var vector = (HalfVector)value;
        var dest = new Memory<Half>(new Half[vector.Memory.Length]);
        vector.Memory.CopyTo(dest);
        var result = new HalfVector(dest);
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

    SqlType[] IUserType.SqlTypes => [new PgvectorSqlType(DbType.Object, PgvectorType.HalfVector)];

    System.Type IUserType.ReturnedType => typeof(HalfVector);

    bool IUserType.IsMutable => false;

}
