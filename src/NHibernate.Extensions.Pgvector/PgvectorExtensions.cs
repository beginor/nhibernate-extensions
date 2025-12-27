using Pgvector;

namespace NHibernate.Extensions.Pgvector;

public static class PgvectorExtensions {

    extension(Vector v1) {

        public double L1Distance(Vector v2) { return double.NaN; }

        public double L2Distance(Vector v2) { return double.NaN; }

        public double CosineDistance(Vector v2) { return double.NaN; }

    }

    extension(HalfVector v1) {

        public double L1Distance(HalfVector v2) { return double.NaN; }

        public double L2Distance(HalfVector v2) { return double.NaN; }

        public double CosineDistance(HalfVector v2) { return double.NaN; }

    }

    extension(SparseVector v1) {

        public double L1Distance(SparseVector v2) { return double.NaN; }

        public double L2Distance(SparseVector v2) { return double.NaN; }

        public double CosineDistance(SparseVector v2) { return double.NaN; }

    }

}
