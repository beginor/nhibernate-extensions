using System.Linq;

namespace NHibernate.Extensions.NpgSql;

public static class ArrayExtensions {

    public static bool ArrayContains<T>(this T[] array, T element) {
        return array.Contains(element);
    }

    public static bool ArrayIntersects<T>(this T[] array, T[] other) {
        return array.Intersect(other).Any();
    }

}
