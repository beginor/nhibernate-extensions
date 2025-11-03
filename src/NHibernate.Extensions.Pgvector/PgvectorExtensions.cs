using System;
using Pgvector;

namespace NHibernate.Extensions.Pgvector;

public static class PgvectorExtensions {

    public static double L1Distance(this Vector v1, Vector v2) {
        return double.NaN;
    }

    public static double L2Distance(this Vector v1, Vector v2) {
        return double.NaN;
    }

    public static double CosineDistance(this Vector v1, Vector v2) {
        return double.NaN;
    }

}
