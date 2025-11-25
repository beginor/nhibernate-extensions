using NHibernate.Dialect.Function;
using NHibernate.Extensions.Pgvector.UserTypes;
using NHibernate.SqlTypes;
using NHibernate.Type;
using Pgvector;

namespace NHibernate.Extensions.Pgvector;

public class PgvectorDialect : NHibernate.Extensions.Npgsql.NpgsqlDialect {

    public PgvectorDialect() {
        RegisterFunctions();
        RegisterUserTypes();
    }

    public override string GetTypeName(SqlType sqlType) {
        if (sqlType is PgvectorSqlType pgvectorSqlType) {
            var vectorType = pgvectorSqlType.PgvectorType;
            switch (vectorType) {
                case PgvectorType.Vector:
                    return "vector";
                case PgvectorType.HalfVector:
                    return "halfvec";
                case PgvectorType.SparseVector:
                    return "sparsevec";
            }
        }
        return base.GetTypeName(sqlType);
    }

    private void RegisterFunctions() {
        // l1_distance => v1 <+> v2
        RegisterFunction("l1_distance", new SQLFunctionTemplate(NHibernateUtil.Double, "?1 <+> ?2"));
        // l2_distance => v1 <-> v2
        RegisterFunction("l2_distance", new SQLFunctionTemplate(NHibernateUtil.Double, "?1 <-> ?2"));
        // cosine_distance => v1 <=> v2
        RegisterFunction("cosine_distance", new SQLFunctionTemplate(NHibernateUtil.Double, "?1 <=> ?2"));
        // inner_product => v1 <#> v2
        RegisterFunction("inner_product", new SQLFunctionTemplate(NHibernateUtil.Double, "?1 <#> ?2"));
        // hamming distance (binary vectors) => v1 <~> v2
        RegisterFunction("hamming_distance", new SQLFunctionTemplate(NHibernateUtil.Double, "?1 <~> ?2"));
        // jaccard_distance (binary vectors) => v1 <%> v2
        RegisterFunction("jaccard_distance", new SQLFunctionTemplate(NHibernateUtil.Double, "?1 <%> ?2"));
    }

    private void RegisterUserTypes() {
        TypeFactory.RegisterType(typeof(Vector), NHibernateUtil.Custom(typeof(VectorType)), ["vector"]);
        TypeFactory.RegisterType(typeof(HalfVector), NHibernateUtil.Custom(typeof(HalfVectorType)), ["halfvec"]);
        TypeFactory.RegisterType(typeof(SparseVector), NHibernateUtil.Custom(typeof(SparseVectorType)), ["sparsevec"]);
    }

}
