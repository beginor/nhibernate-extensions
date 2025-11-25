using Pgvector;

namespace NHibernate.Extensions.Pgvector;

public static class PgvectorExtensions {

    public static double L1Distance(this Vector v1, Vector v2) {
        return double.NaN;
    }

    public static double L1Distance(this HalfVector v1, HalfVector v2) {
        return double.NaN;
    }

    public static double L1Distance(this SparseVector v1, SparseVector v2) {
        return double.NaN;
    }

    public static double L2Distance(this Vector v1, Vector v2) {
        return double.NaN;
    }

    public static double L2Distance(this HalfVector v1, HalfVector v2) {
        return double.NaN;
    }

    public static double L2Distance(this SparseVector v1, SparseVector v2) {
        return double.NaN;
    }

    public static double CosineDistance(this Vector v1, Vector v2) {
        return double.NaN;
    }

    public static double CosineDistance(this HalfVector v1, HalfVector v2) {
        return double.NaN;
    }

    public static double CosineDistance(this SparseVector v1, SparseVector v2) {
        return double.NaN;
    }

}
