using System.Data;
using NHibernate.SqlTypes;
using Pgvector;

namespace NHibernate.Extensions.Pgvector;

public class PgvectorSqlType : SqlType {

    public PgvectorType PgvectorType { get; }

    public PgvectorSqlType(DbType dbType, PgvectorType pgvectorType) : base(dbType) {
        PgvectorType = pgvectorType;
    }

}

public enum PgvectorType {
    Vector = 1,
    HalfVector = 2,
    SparseVector = 3,
}
