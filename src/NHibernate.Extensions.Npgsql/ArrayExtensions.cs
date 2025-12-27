using System.Linq;

namespace NHibernate.Extensions.Npgsql;

public static class ArrayExtensions {

    extension<T>(T[] array) {

        public bool ArrayContains(T element) {
            return array.Contains(element);
        }

        public bool ArrayIntersects(T[] other) {
            return array.Intersect(other).Any();
        }

    }

}
