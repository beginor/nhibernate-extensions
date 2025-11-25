using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;
using Pgvector;

namespace NHibernate.Extensions.Pgvector.UserTypes;

public class SparseVectorType  : IUserType {

       bool IUserType.Equals(object x, object y) {
        var vx = (SparseVector)x;
        var vy = (SparseVector)y;
        return vx.Equals(vy);
    }

    int IUserType.GetHashCode(object x) {
        return ((SparseVector)x).GetHashCode();
    }

    object IUserType.NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner) {
        if (names.Length != 1) {
            throw new InvalidOperationException("Only expecting one column...");
        }
        return (SparseVector)rs[names[0]];
    }

    void IUserType.NullSafeSet(DbCommand cmd, object? value, int index, ISessionImplementor session) {
        var parameter = (NpgsqlParameter)cmd.Parameters[index];
        if (value == null) {
            parameter.Value = DBNull.Value;
        }
        else {
            var vector = (SparseVector)value;
            parameter.Value = vector;
            parameter.DataTypeName = "sparsevec";
        }
    }

    object IUserType.DeepCopy(object value) {
        var vector = (SparseVector)value;
        var dest = new Memory<float>(new float[vector.Values.Length]);
        vector.Values.CopyTo(dest);
        var result = new SparseVector(dest);
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

    SqlType[] IUserType.SqlTypes => [new PgvectorSqlType(DbType.Object, PgvectorType.SparseVector)];

    System.Type IUserType.ReturnedType => typeof(SparseVector);

    bool IUserType.IsMutable => false;

}
